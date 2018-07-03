using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EntidadesCompartidas;

namespace Persistencia
{
    public interface IPContrato
    {
        void AltaContrato(Contrato _contrato, string usuario, string clave);
        void BajaContrato(Contrato _contrato, string usuario, string clave);
        void ModContrato(Contrato _contrato, string usuario, string clave);
        Contrato BuscarContrato(int _CodEmpresa, int _CodTipo, string usuario, string clave);
        List<Contrato> ListarContrato(int CodEmpresa, string usuario, string clave);
        List<Contrato> ListarTodosLosContratos(string usuario, string clave);
        DateTime ChequearFacturaPaga(string[] _factura, string usuario, string clave);
    }
}
