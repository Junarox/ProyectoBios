using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EntidadesCompartidas
{
    [KnownType(typeof(Gerente))]
    [KnownType(typeof(Cajero))]

    [DataContract]
    public abstract class Usuario
    {
        private int _ci;
        private string _usuario;
        private string _clave;
        private string _nomCompleto;

        [DataMember]
        public int Ci
        {
            get
            {
                return _ci;
            }

            set
            {
                if (value.ToString().Length != 8)
                {
                    throw new FormatException("El Documento debe contener 8 digitos.");
                }

                _ci = value;
            }
        }

        [DataMember]
        public string Usu
        {
            get
            {
                return _usuario;
            }

            set
            {
                if (value.Trim().Length == 0)
                {
                    throw new Exception("El campo Usuario no puede estar vacío.");
                }
                if (value.Length > 30)
                {
                    throw new Exception("El campo Usuario no puede contener mas de 30 caracteres.");
                }
                _usuario = value;
            }
        }

        [DataMember]
        public string Clave
        {
            get
            {
                return _clave;
            }

            set
            {
                if (value.Trim().Length == 0)
                {
                    throw new Exception("El campo Clave no puede estar vacío.");
                }
                if (value.Length > 30)
                {
                    throw new Exception("El campo Clave no puede contener mas de 7 caracteres.");
                }
                _clave = value;
            }
        }

        [DataMember]
        public string NomCompleto
        {
            get
            {
                return _nomCompleto;
            }

            set
            {
                if (value.Trim().Length == 0)
                {
                    throw new Exception("El campo Nombre no puede estar vacío.");
                }
                if (value.Length > 30)
                {
                    throw new Exception("El campo Nombre no puede contener mas de 30 caracteres.");
                }
                _nomCompleto = value;
            }
        }

        protected Usuario(int ci, string usuario, string clave, string nomCompleto)
        {
            Ci = ci;
            Usu = usuario;
            Clave = clave;
            NomCompleto = nomCompleto;
        }

        public Usuario()
        {
            Ci = 21001002;
            Usu = "N/A";
            Clave = "BiosMoney" + Ci.ToString();
            NomCompleto = "N/A";
        }
    }
}
