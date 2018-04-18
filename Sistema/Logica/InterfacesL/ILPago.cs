using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntidadesCompartidas;

namespace Logica
{
    public interface ILPago
    {
        void AltaPago(Pago _pago);
        List<Pago> ListarPagos();
        List<LineaPago> ListarFacturas(int _NumeroInterno);
    }
}
