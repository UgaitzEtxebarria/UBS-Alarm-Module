using System;
using System.Collections.Generic;
using System.Xml;

namespace UBSAlarmModule.Clases
{
    public class Alarms
    {
        private UBSLib.UBSModule ubsMod;
        private string strAlarmDescFilename = "Alarmas/alarmas.xml";    //Archivo donde se leen las alarmas y sus descripciones
        private string m_communication = "message";                     //Tipo de comunicacion utilizada en el UBS
        private Dictionary<string, AlarmBlock> m_alarmBlocks;           //La lista de los "Bloques de Alarmas" (1 Bloque = 31 Alarmas) definidos por un texto, por si hay variables que compartir

        public event EventHandler CambioDeAlarma;

        ////////////////////////////////////////////////////////////////////////
        //Constructor
        public Alarms(UBSLib.UBSModule ubs)
        {
            ubsMod = ubs;
            this.m_alarmBlocks = new Dictionary<string, AlarmBlock>();
            LoadAlarms();
        }

        ////////////////////////////////////////////////////////////////////////
        //Activar la alarma usando el ID
        public bool ActivarAlarma(string id)
        {
            if (ubsMod.Status == UBSLib.UBSModuleStatus.Closing || ubsMod.Status == UBSLib.UBSModuleStatus.Closed)
                return false;
            bool solu = false;
            foreach (AlarmBlock ab in this.m_alarmBlocks.Values)
            {
                List<string> ids = new List<string>();
                if (!ab.GetIds(out ids) || !ids.Contains(id))
                    continue;
                solu = ab.ActivarAlarma(id);
                CambioDeAlarma?.Invoke(ab.Name, null);
                break;
            }
            return solu;
        }

        ////////////////////////////////////////////////////////////////////////
        //Desactivar la alarma usando el ID
        public bool DesactivarAlarma(string id)
        {
            if (ubsMod.Status == UBSLib.UBSModuleStatus.Closing || ubsMod.Status == UBSLib.UBSModuleStatus.Closed)
                return false;
            bool solu = false;
            foreach (AlarmBlock ab in this.m_alarmBlocks.Values)
            {
                List<string> ids = new List<string>();
                if (!ab.GetIds(out ids) || !ids.Contains(id))
                    continue;
                solu = ab.DesactivarAlarma(id);
                CambioDeAlarma?.Invoke(ab.Name, null);
                break;
            }
            return solu;
        }

        /////////////////////////
        //Ver si la alarma con cierto Id está activa o no
        public bool AlarmaActiva(string id)
        {
            bool solu = false;

            foreach (AlarmBlock ab in this.m_alarmBlocks.Values)
            {
                List<string> ids = new List<string>();
                if (!ab.GetIds(out ids) || !ids.Contains(id))
                    continue;
                solu = ab.AlarmaActiva(id);
                break;
            }
            return solu;
        }

        /////////////////////////
        //Carga de los IDs, bits, descripciones y títulos de las alarmas.
        private void LoadAlarms()
        {
            try
            {
                ubsMod.Log("Cargando descripciones de alarmas.");

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(strAlarmDescFilename);
                XmlNodeList listModules = xmlDoc.GetElementsByTagName("Alarms");
                XmlNodeList listBlocks = ((XmlElement)listModules[0]).GetElementsByTagName("AlarmBlock");

                if (listBlocks.Count != 0)
                {
                    int BlockNum = 0;
                    foreach (XmlElement block in listBlocks)
                    {
                        XmlNodeList listModule = block.GetElementsByTagName("Alarm");

                        if (listModule.Count > 32)
                            ubsMod.Error("En el bloque de alarmas " + listBlocks.ToString() + " hay mas de 32 alarmas, modificar el fichero " + strAlarmDescFilename, true, false);

                        AlarmBlock ab;
                        BlockNum++;
                        if (block.Attributes.Count > 0)
                            ab = new AlarmBlock(block.GetAttribute("Id"), false);
                        else
                            ab = new AlarmBlock(BlockNum.ToString(), false);

                        int AlarmNum = 0;
                        foreach (XmlElement node in listModule)
                        {
                            XmlNodeList aID = node.GetElementsByTagName("ID");
                            XmlNodeList aTitulo = node.GetElementsByTagName("Title");
                            XmlNodeList aDesc = node.GetElementsByTagName("Description");

                            ab.AñadirAlarma(aID[0].InnerText, AlarmNum++, aTitulo[0].InnerText, aDesc[0].InnerText);
                        }

                        m_alarmBlocks.Add(ab.Name, ab);
                    }
                }
                ubsMod.Log("Descripciones de alarmas cargadas.");
            }
            catch (Exception e)
            {
                ubsMod.Error("Ocurrio un error leyendo el fichero de descripciones de alarmas. [" + strAlarmDescFilename + "]. " + e.Message, true, false);
            }
        }

        ////////////////////////////////////////////////////////////////////////
        //Getter/setter de la cadena de bits de las alarmas
        /*public List<int> AlarmNum
        {
            get
            {
                List<int> get = new List<int>();
                foreach (AlarmBlock ab in m_alarmBlocks.Values)
                    get.Add(ab.AlarmNum);
                return get; }
            set
            {
                if (value.Count != this.m_alarmBlocks.Count)
                    throw new Exception("El número de bloques de alarmas no concuerda con los existentes.");
                for (int i = 0; i < this.m_alarmBlocks.Count; i++)
                    this.m_alarmBlocks[communicationNames[i]].AlarmNum = value[i];
            }
        }*/

        ////////////////////////////////////////////////////////////////////////
        //Get de la descripcion y titulo dependiendo del ID, si no existe devuelve null
        public bool Descripcion(string id, out DescripcionAlarma value)
        {
            value = null;
            foreach (AlarmBlock ab in this.m_alarmBlocks.Values)
                if (ab.Descripcion(id, out value))
                    break;
            return value != null;
        }

        ////////////////////////////////////////////////////////////////////////
        //Devuelve en el bool si hay algun valor, y en el out la lista de las claves de todos los bloques con todos los ID
        public bool GetIds(out List<string> keys)
        {
            keys = new List<string>();
            foreach (AlarmBlock ab in this.m_alarmBlocks.Values)
            {
                List<string> tmp = new List<string>();
                if (ab.GetIds(out tmp))
                    keys.AddRange(tmp);
            }

            return (keys.Count > 0);
        }

        ////////////////////////////////////////////////////////////////////////
        //Devuelve en el bool si hay algun valor, y en el out la lista de las claves del bloque con nombre "blockName" con todos sus ID
        public bool GetIds(string blockName, out List<string> keys)
        {
            keys = new List<string>();
            return this.m_alarmBlocks[blockName].GetIds(out keys);
        }

        ////////////////////////////////////////////////////////////////////////
        //Devuelve en el bool si existe algun bloque con el Id especificado
        public bool ContainsId(string Id)
        {
            return m_alarmBlocks.ContainsKey(Id);
        }

        ////////////////////////////////////////////////////////////////////////
        //Devuelve la cantidad de alarmas
        public int Count()
        {
            int solu = 0;
            foreach (AlarmBlock ab in this.m_alarmBlocks.Values)
                solu += ab.Count();
            return solu;
        }

        ////////////////////////////////////////////////////////////////////////
        //Define el numero del bloque "blockName"
        public bool SetBlock(string blockName, uint num)
        {
            if (ubsMod.Status == UBSLib.UBSModuleStatus.Closing || ubsMod.Status == UBSLib.UBSModuleStatus.Closed)
                return false;
            if (!m_alarmBlocks.ContainsKey(blockName))
                return false;
            m_alarmBlocks[blockName].AlarmNum = num;
            CambioDeAlarma?.Invoke(m_alarmBlocks[blockName].Name, null);
            return true;
        }

        ////////////////////////////////////////////////////////////////////////
        //Getter/setter de la comunicación a usar
        public string Communication
        {
            get { return this.m_communication; }
            set { this.m_communication = value; }
        }
    }
}
