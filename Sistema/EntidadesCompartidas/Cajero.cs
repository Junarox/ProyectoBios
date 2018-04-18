using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EntidadesCompartidas
{
    [DataContract]
    public class Cajero : Usuario
    {
        private string _horaInicio;
        private string _horaFin;

        [DataMember]
        public string HoraInicio
        {
            get
            {
                return _horaInicio;
            }

            set
            {
                try
                {

                    if (Convert.ToInt32(value) > 2459 || Convert.ToInt32(value) < 0000)
                        throw new Exception("La Hora de Inicio es incorrecta.");
                    _horaInicio = value;
                }
                catch (FormatException)
                {
                    throw new Exception("Formato Incorrecto.");
                }
            }
        }

        [DataMember]
        public string HoraFin
        {
            get
            {
                return _horaFin;
            }

            set
            {
                try
                {

                    if (Convert.ToInt32(value) > 2459 || Convert.ToInt32(value) < 0000)
                        throw new Exception("La Hora de Fin es incorrecta.");
                    _horaFin = value;
                }
                catch (FormatException)
                {
                    throw new Exception("Formato Incorrecto.");
                }
            }
        }

        public Cajero(int ci, string usuario, string clave, string nomCompleto, string horaInicio, string horaFin)
            : base(ci, usuario, clave, nomCompleto)
        {
            HoraInicio = horaInicio;
            HoraFin = horaFin;
        }

        public Cajero() : base()
        {
            HoraInicio = "0000";
            HoraFin = "0000";
        }
    }
}
