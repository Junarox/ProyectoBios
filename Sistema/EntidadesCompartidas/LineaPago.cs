using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EntidadesCompartidas
{
    [DataContract]
    public class LineaPago
    {
        private int _Monto;
        private DateTime _FechaVencimiento;
        private int _CodigoCliente;
        private Contrato _Contrato;


        [DataMember]
        public string CodContrato { get { return Contrato.CodContrato.ToString(); } set { } }

        [DataMember]
        public Contrato Contrato
        {
            get { return _Contrato; }
            set { _Contrato = value; }
        }

        [DataMember]
        public int CodigoCliente
        {
            get { return _CodigoCliente; }
            set { _CodigoCliente = value; }
        }

        [DataMember]
        public int Monto
        {
            get { return _Monto; }
            set { _Monto = value; }
        }

        [DataMember]
        public DateTime FechaVencimiento
        {
            get { return _FechaVencimiento; }
            set { _FechaVencimiento = value; }
        }

        public LineaPago(int pMonto, DateTime pFechaVencimiento, int pCodigoCliente, Contrato pContrato)
        {
            Monto = pMonto;
            FechaVencimiento = pFechaVencimiento;
            CodigoCliente = pCodigoCliente;
            Contrato = pContrato;
        }

        public LineaPago()
        {
            Monto = 0;
            FechaVencimiento = DateTime.Now;
            CodigoCliente = 0;
            Contrato = new Contrato();
        }
    }
}
