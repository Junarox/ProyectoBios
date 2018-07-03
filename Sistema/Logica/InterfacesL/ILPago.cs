using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntidadesCompartidas;

namespace Logica
{
    public interface ILPago
    {
        void AltaPago(Pago pago, Usuario logueo);
        List<Pago> ListarPagos(Usuario logueo);
        List<LineaPago> ListarFacturas(int _NumeroInterno, Usuario logueo);
    }
}
