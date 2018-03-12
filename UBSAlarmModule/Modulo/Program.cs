using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using UBSAlarmModule.Clases;

namespace UBSAlarmModule
{
    public abstract class AlarmModule : UBSLib.UBSVisualModule
    {
        protected Alarms m_alarmas;

        //////////////////////////////////////////////////////////////////////////////////
        public AlarmModule(string _id)
            : base(_id)
        { }


        //////////////////////////////////////////////////////////////////////////////////
        public override bool Init()
        {
            base.Init();
            WindowForm = new AlarmForm(this);

            this.m_alarmas = new Alarms(this);
            this.m_alarmas.CambioDeAlarma += ActualizarAlarma;
            this.m_alarmas.Communication = this.GetGlobalParameter("Communication").ToString();
            return true;
        }

        //////////////////////////////////////////////////////////////////////////////////

        public override bool Destroy()
        {

            return true;
        }

        //////////////////////////////////////////////////////////////////////////////////
        public override void HandleMessages(UBSLib.UBSMessage message)
        {
            if (Status != UBSLib.UBSModuleStatus.Closing || Status != UBSLib.UBSModuleStatus.Closed)
            {
                m_alarmas.Communication = this.GetGlobalParameter("Communication").ToString();

                string strMessage = message.ToString();

                //Parametros del mensaje
                string[] strParams = strMessage.Split('#');
                //

                //Si el primer valor del mensaje es una de los identificadores de las variables compartida se aplica ese valor
                if (m_alarmas.ContainsId(strParams[0]))
                {
                    switch (m_alarmas.Communication)
                    {
                        case "message":

                            break;
                        case "twincat":
                            m_alarmas.SetBlock(strParams[0], Convert.ToUInt32(strParams[1]));
                            break;
                        default:
                            Notify("La comunicación configurada en el modulo Alarma no existe: " + m_alarmas.Communication, true, false);
                            break;
                    }
                }
                else
                {//Si solo se envia un codigo, significa que es la alarma a activar/desactivar

                    //El segundo valor del mensaje debe ser el true o false por si se quiere activar o desactivar
                    if (bool.Parse(strParams[1]))   //Activar
                        m_alarmas.ActivarAlarma(strParams[0]);
                    else
                    {   //Desactivar
                        if (m_alarmas.AlarmaActiva(strParams[0]))
                            m_alarmas.DesactivarAlarma(strParams[0]);
                    }
                }
            }
        }

        //////////////////////////////////////////////////////////////////////////////////

        private void ActualizarAlarma(object sender, EventArgs e)
        {
            try
            {
                if (Status != UBSLib.UBSModuleStatus.Closing && Status != UBSLib.UBSModuleStatus.Closed)
                {
                    List<string> keyList = new List<string>();
                    if (m_alarmas.GetIds(sender.ToString(), out keyList))
                    {
                        foreach (string val in keyList)
                        {
                            DescripcionAlarma da = new DescripcionAlarma();
                            if (m_alarmas.Descripcion(val, out da))
                            {
                                //Ver estado actual de la alarma para saber si ha sido activada o desactivada.
                                if (m_alarmas.AlarmaActiva(val))
                                {
                                    //Si ya ha sido activada con anterioridad no registrarla como activación de alarma.
                                    if (((AlarmForm)WindowForm).AñadirALista(da))
                                    {
                                        WriteConsole("Alarma " + da.ID + " activada: " + da.Titulo, true);
                                        AlarmaActivada(val);
                                        if (!ButtonColor(Id, Color.Yellow, false))
                                            Error("Error al colorear la alarma " + da.ID, true, false);
                                    }
                                }
                                else
                                {
                                    //Si ya ha sido desactivada con anterioridad no registrarla como desactivación de alarma.
                                    if (((AlarmForm)WindowForm).BorrarDeLista(da))
                                    {
                                        WriteConsole("Alarma " + da.ID + " desactivada: " + da.Titulo, true);
                                        AlarmaDesactivada(val);
                                        if (((AlarmForm)WindowForm).ListaVacia())
                                            if (!ButtonColor(Id, null, false))
                                                Error("Error al decolorear la alarma " + da.ID, true, false);
                                    }
                                }
                            }
                            else
                                WriteConsole("No se encuentra la descripción de la alarma " + val + " entre las descripciones cargadas.", true);
                        }
                    }
                    else
                        WriteConsole("No se pueden actualizar las alarmas ya que no hay ninguna cargada.", true);
                }
            }
            catch (Exception ex)
            {
                if (Status != UBSLib.UBSModuleStatus.Closing && Status != UBSLib.UBSModuleStatus.Closed)
                {
                    var st = new StackTrace(ex, true);
                    // Get the top stack frame
                    var frame = st.GetFrame(0);
                    // Get the line number from the stack frame
                    var line = frame.GetFileLineNumber();
                    Error("Error al activar una alarma. Linea de código " + line + ": " + ex.Message);
                }
            }
        }

        //////////////////////////////////////////////////////////////////////////////////

        protected abstract void AlarmaActivada(string AlarmId);

        //////////////////////////////////////////////////////////////////////////////////

        protected abstract void AlarmaDesactivada(string AlarmId);
    }
}
