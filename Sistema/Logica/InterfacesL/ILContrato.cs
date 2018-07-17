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
        void AltaContrato(Contrato contrato, Usuario logueo);
        void BajaContrato(Contrato contrato, Usuario logueo);
        void ModContrato(Contrato contrato, Usuario logueo);
        Contrato BuscarContrato(int codigoEmpresa, int codTipo, Usuario logueo);
        List<Contrato> ListarContrato(Empresa empresa, Usuario logueo);
        string GenerarXMLContratos();
        DateTime ChequearFacturaPaga(string factura);
    }
}
