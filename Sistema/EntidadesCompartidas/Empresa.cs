using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EntidadesCompartidas
{
    [DataContract]
    public class Empresa
    {
        private int _Codigo;
        private long _Rut;
        private string _Nombre;
        private string _DirFiscal;
        private long _Tel;

        [DataMember]
        public int Codigo
        {
            get { return _Codigo; }
            set
            {
                if (value.ToString().Trim().Length != 4)
                    throw new FormatException("El Código debe contener 4 dígitos.");
                else
                    _Codigo = value;
            }
        }

        [DataMember]
        public long Rut
        {
            get { return _Rut; }
            set
            {
                if (value.ToString().Trim().Length != 12)
                    throw new FormatException("El Rut debe contener 12 dígitos.");
                _Rut = value;
            }
        }

        [DataMember]
        public string Nombre
        {
            get { return _Nombre; }
            set
            {
                if (value.Length > 100)
                    throw new FormatException("El Nombre no puede contener más de 100 caracteres.");
                else if (value.Equals(""))
                    throw new FormatException("El Nombre no puede estar vacío.");
                else
                    _Nombre = value;
            }
        }

        [DataMember]
        public string DirFiscal
        {
            get { return _DirFiscal; }
            set
            {
                if (value.Length > 100)
                    throw new FormatException("La Dirección Fiscal no puede contener más de 100 caracteres.");
                else if(value.Equals(""))
                    throw new FormatException("La Dirección Fiscal no puede estar vacío.");
                else
                    _DirFiscal = value;

            }
        }

        [DataMember]
        public long Tel
        {
            get { return _Tel; }
            set { _Tel = value; }
        }

        public Empresa(long pRut, int pCodigo, string pNombre, string pDirFiscal, Int64 pTel)
        {
            Codigo = pCodigo;
            Rut = pRut;
            Nombre = pNombre;
            DirFiscal = pDirFiscal;
            Tel = pTel;
        }

        public Empresa()
        {
            Rut = 111111111111;
            Codigo = 1000;
            DirFiscal = "Calle 1, 1234";
            Tel = 1231456;
        }
    }
}
