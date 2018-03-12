using System;
using System.Collections;
using System.Collections.Generic;

namespace UBSAlarmModule.Clases
{
    public class AlarmBlock
    {
        private Dictionary<string, int> m_AlarmBits;
        private Dictionary<string, DescripcionAlarma> m_AlarmDescriptions;

        private BitArray Numero;
        public string Name { get; set; }
        private bool BitResumen;

        ////////////////////////////////////////////////////////////////////////
        //Constructor
        public AlarmBlock()
        {
            this.Name = "";
            this.Numero = new BitArray(32, false);
            this.BitResumen = true;

            this.m_AlarmBits = new Dictionary<string, int>();
            this.m_AlarmDescriptions = new Dictionary<string, DescripcionAlarma>();
        }

        ////////////////////////////////////////////////////////////////////////
        //Constructor
        public AlarmBlock(string name, bool bitResumen)
        {
            this.Name = name;
            this.Numero = new BitArray(32, false);
            this.BitResumen = false;

            this.m_AlarmBits = new Dictionary<string, int>();
            this.m_AlarmDescriptions = new Dictionary<string, DescripcionAlarma>();
        }

        ////////////////////////////////////////////////////////////////////////
        //Añadir nueva alarma al registro
        public void AñadirAlarma(string id, int bit, string titulo, string descripcion)
        {
            this.m_AlarmBits.Add(id, bit);
            DescripcionAlarma da = new DescripcionAlarma(id, titulo, descripcion);
            this.m_AlarmDescriptions.Add(id, da);
        }

        ////////////////////////////////////////////////////////////////////////
        //Activar la alarma usando el ID
        public bool ActivarAlarma(string id)
        {
            try
            {
                Numero.Set(m_AlarmBits[id], true);
                return true;
              }
            catch (Exception e)
            {
                return false;
            }
        }

        ////////////////////////////////////////////////////////////////////////
        //Desactivar la alarma usando el ID
        public bool DesactivarAlarma(string id)
        {
            try
            {
                Numero.Set(m_AlarmBits[id], false);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        ////////////////////////////////////////////////////////////////////////
        //Ver si la alarma con cierto Id está activa o no
        public bool AlarmaActiva(string id)
        {
            return Numero.Get(m_AlarmBits[id]);
        }

        ////////////////////////////////////////////////////////////////////////
        //Getter/setter de la cadena de bits de las alarmas
        public uint AlarmNum
        {
            get {
                int[] array = new int[1];
                Numero.CopyTo(array, 0);
                return (uint) array[0];
            }
            set { Numero = new BitArray(new[] { (int)value });
            }
        }

        ////////////////////////////////////////////////////////////////////////
        //Get de la descripcion y titulo dependiendo del ID, si no existe devuelve null
        public bool Descripcion(string id, out DescripcionAlarma value)
        {
            if (this.m_AlarmDescriptions.ContainsKey(id))
            {
                value = this.m_AlarmDescriptions[id];
                return true;
            }
            value = null;
            return false;
        }

        ////////////////////////////////////////////////////////////////////////
        //Devuelve en el bool si hay algun valor, y en el out la lista de las claves con todos los ID
        public bool GetIds(out List<string> keys)
        {
            keys = new List<string>(this.m_AlarmBits.Keys);
            return (keys.Count > 0);
        }

        ////////////////////////////////////////////////////////////////////////
        //Devuelve la cantidad de alarmas
        public int Count()
        {
            return m_AlarmBits.Count;
        }
    }
}