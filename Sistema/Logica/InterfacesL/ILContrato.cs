using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EntidadesCompartidas;
using System.Xml.Linq;
using System.Xml;

namespace Logica
{
    public interface ILContrato
    {
        void AltaContrato(Contrato _contrato);
        void BajaContrato(Contrato _contrato);
        void ModContrato(Contrato _contrato);
        Contrato BuscarContrato(int _CodigoEmpresa, int _CodTipo);
        List<Contrato> ListarContrato(Empresa Empresa);
        string GenerarXMLContratos();
        DateTime ChequearFacturaPaga(string _factura);
    }
}
