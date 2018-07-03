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

        public List<Pago> ListarPagos(Usuario logueo)
        {
            return Persistencia.FabricaP.GetPPago().ListarPagos(logueo.Usu, logueo.Clave);
        }

        public void AltaPago(Pago pago, Usuario logueo)
        {
           Persistencia.FabricaP.GetPPago().AltaPago(pago,logueo.Usu, logueo.Clave);
        }

        public List<LineaPago> ListarFacturas(int numeroInterno, Usuario logueo)
        {
            return Persistencia.FabricaP.GetPPago().ListarFacturas(numeroInterno, logueo.Usu,logueo.Clave);
        }

    }
}
