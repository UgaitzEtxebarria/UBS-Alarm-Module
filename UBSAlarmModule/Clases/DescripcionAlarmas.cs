﻿
using System.Collections.Generic;

namespace UBSAlarmModule.Clases
{
    class DescripcionAlarmas
    {
        Dictionary<int, DescripcionAlarma> m_alarmas;   //Lista de las descripciones de la alarma clasificadas por su identificador

        public DescripcionAlarmas()
        {
            m_alarmas = new Dictionary<int, DescripcionAlarma>();
        }

        public Dictionary<int, DescripcionAlarma> AlarmasDescription
        {
            get { return m_alarmas; }
            set { this.m_alarmas = value; }
        }
    }
}
