using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntidadesCompartidas;

namespace Logica
{
    internal class LPago : ILPago
    {
        private static LPago _instancia = null;
        private LPago() { }
        public static LPago GetInstancia()
        {
            if (_instancia == null)
                _instancia = new LPago();
            return _instancia;
        }

        public List<Pago> ListarPagos()
        {
            return Persistencia.FabricaP.GetPPago().ListarPagos();
        }

        public void AltaPago(Pago _pago)
        {
           Persistencia.FabricaP.GetPPago().AltaPago(_pago);
        }

        public List<LineaPago> ListarFacturas(int _NumeroInterno)
        {
            return Persistencia.FabricaP.GetPPago().ListarFacturas(_NumeroInterno);
        }

    }
}
