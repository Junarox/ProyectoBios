using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EntidadesCompartidas
{
    [DataContract]
    public class Pago
    {
        private int _NumeroInterno;
        private DateTime _Fecha;
        private int _Monto;
        private Usuario _Cajero;
        private List<LineaPago> _LineasPago;

        [DataMember]
        public int NumeroInterno
        {
            get { return _NumeroInterno; }
            set { _NumeroInterno = value; }
        }

        [DataMember]
        public DateTime Fecha
        {
            get { return _Fecha; }
            set { _Fecha = value; }
        }

        [DataMember]
        public int Monto
        {
            get { return _Monto; }
            set { _Monto = value; }
        }

        [DataMember]
        public Usuario Cajero
        {
            get { return _Cajero; }
            set { _Cajero = value; }
        }

        [DataMember]
        public List<LineaPago> LineasPago
        {
            get { return _LineasPago; }
            set { _LineasPago = value; }
        }

        public Pago(int pNumeroInterno, DateTime pFecha, int pMonto, Usuario pCajero, List<LineaPago> pLineasPago)
        {
            NumeroInterno = pNumeroInterno;
            Fecha = pFecha;
            Monto = pMonto;
            Cajero = pCajero;
            LineasPago = pLineasPago;
        }

        public Pago() 
        {
            NumeroInterno = 0;
            Fecha = DateTime.Now;
            Monto = 0;
            Cajero = new Cajero();
            LineasPago = new List<LineaPago>();
        }
    }
}
