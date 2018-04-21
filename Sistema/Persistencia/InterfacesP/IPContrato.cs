using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EntidadesCompartidas;

namespace Persistencia
{
    public interface IPContrato
    {
        void AltaContrato(Contrato _contrato);
        void BajaContrato(Contrato _contrato);
        void ModContrato(Contrato _contrato);
        Contrato BuscarContrato(int _CodEmpresa, int _CodTipo);
        List<Contrato> ListarContrato(int CodEmpresa);
        List<Contrato> ListarTodosLosContratos();
        DateTime ChequearFacturaPaga(string[] _factura);
    }
}
