using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntidadesCompartidas;

namespace Persistencia
{
    public interface IPPago
    {
        void AltaPago(Pago _pago, string usuario, string clave);
        List<Pago> ListarPagos(string usuario, string clave);
        List<LineaPago> ListarFacturas(int _NumeroInterno, string usuario, string clave);
    }
}
