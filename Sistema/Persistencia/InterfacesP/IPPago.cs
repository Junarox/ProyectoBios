using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntidadesCompartidas;

namespace Persistencia
{
    public interface IPPago
    {
        void AltaPago(Pago _pago);
        List<Pago> ListarPagos();
        List<LineaPago> ListarFacturas(int _NumeroInterno);
    }
}
