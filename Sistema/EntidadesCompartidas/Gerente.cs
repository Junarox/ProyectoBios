using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EntidadesCompartidas
{
    [DataContract]
    public class Gerente : Usuario
    {
        private string _email;

        [DataMember]
        public string Email
        {
            get
            {
                return _email;
            }

            set
            {
                if (value.Trim().Length == 0)
                {
                    throw new Exception("El campo Email no puede estar vacío.");
                }
                if (value.Length > 30)
                {
                    throw new Exception("El campo Email no puede contener mas de 50 caracteres.");
                }
                _email = value;
            }
        }

        public Gerente() : base()
        {
            Email = "Default@default.com";
        }

        public Gerente(int ci, string usuario, string clave, string nomCompleto, string email)
            : base(ci, usuario, clave, nomCompleto)
        {
            Email = email;
        }
    }
}
