using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EntidadesCompartidas
{
    [DataContract]
    public class Contrato
    {
        private Empresa _Empresa;
        private int _CodContrato;
        private string _NomContrato;

        [DataMember]
        public Empresa Empresa
        {
            get
            {
                return _Empresa;
            }

            set
            {
                if (value == null)
                    throw new Exception("EL Contrato debe corresponder a una Empresa");
                else
                    _Empresa = value;
            }
        }

        [DataMember]
        public int CodContrato
        {
            get { return _CodContrato; }
            set { _CodContrato = value; }
        }

        [DataMember]
        public string NomContrato
        {
            get { return _NomContrato; }
            set { _NomContrato = value; }
        }

        public Contrato(Empresa pEmpresa, int pCodContrato, string pNombreContrato)
        {
            Empresa = pEmpresa;
            CodContrato = pCodContrato;
            NomContrato = pNombreContrato;
        }

        public Contrato()
        {
            Empresa = new Empresa();
            CodContrato = 00;
            NomContrato = "N/A";
        }

        
    }
}
