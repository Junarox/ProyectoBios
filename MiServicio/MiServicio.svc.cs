using EntidadesCompartidas;
using Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;

namespace MiServicio
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MiServicio" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MiServicio.svc or MiServicio.svc.cs at the Solution Explorer and start debugging.
    public class MiServicio : IMiServicio
    {


        #region Empleados

        Usuario IMiServicio.Logueo(string usuario, string clave)
        {
            return FabricaL.GetLUsuario().Logueo(usuario, clave);
        }

        void IMiServicio.Alta(Usuario usuario, Usuario logueo)
        {
            FabricaL.GetLUsuario().Alta(usuario, logueo);
        }

        List<Gerente> IMiServicio.ListarGerentes(Usuario logueo)
        {
            return FabricaL.GetLUsuario().ListarGerentes(logueo);
        }

        void IMiServicio.ModificarClave(Usuario usuario, string clave1, string clave2)
        {
            FabricaL.GetLUsuario().ModificarClave(usuario, clave1, clave2);
        }

        #region Cajeros
        void IMiServicio.BajaCajero(Usuario cajero, Usuario logueo)
        {
            FabricaL.GetLUsuario().BajaCajero(cajero, logueo);
        }

        Cajero IMiServicio.BuscarCajero(int Ci, Usuario logueo)
        {
            return FabricaL.GetLUsuario().BuscarCajero(Ci, logueo);
        }

        void IMiServicio.Modificar(Usuario cajero, Usuario logueo)
        {
            FabricaL.GetLUsuario().Modificar(cajero, logueo);
        }

        List<Cajero> IMiServicio.ListarCajeros(Usuario logueo)
        {
            return FabricaL.GetLUsuario().ListarCajeros(logueo);
        }

        void IMiServicio.ActualizarHorasExtra(Cajero _cajero, DateTime _fecha, int _minutosExtra)
        {
            FabricaL.GetLUsuario().ActualizarHorasExtra(_cajero, _fecha, _minutosExtra);
        }

        #endregion
        #endregion

        #region Empresas
        void IMiServicio.AltaEmpresa(Empresa empresa, Usuario logueo)
        {
            FabricaL.GetEmpresa().AltaEmpresa(empresa, logueo);
        }

        void IMiServicio.ModEmpresa(Empresa empresa, Usuario logueo)
        {
            FabricaL.GetEmpresa().ModEmpresa(empresa, logueo);
        }

        void IMiServicio.BajaEmpresa(Empresa empresa, Usuario logueo)
        {
            FabricaL.GetEmpresa().BajaEmpresa(empresa, logueo);
        }

        Empresa IMiServicio.BuscarEmpresa(int codigo, Usuario logueo)
        {
            return FabricaL.GetEmpresa().BuscarEmpresa(codigo);
        }

        List<Empresa> IMiServicio.ListarEmpresa(Usuario logueo)
        {
            return FabricaL.GetEmpresa().ListarEmpresa(logueo);
        }
        #endregion

        #region Contratos
        void IMiServicio.AltaContrato(Contrato contrato, Usuario logueo)
        {
            FabricaL.GetContrato().AltaContrato(contrato, logueo);
        }

        void IMiServicio.BajaContrato(Contrato contrato, Usuario logueo)
        {
            FabricaL.GetContrato().BajaContrato(contrato, logueo);
        }

        void IMiServicio.ModContrato(Contrato contrato, Usuario logueo)
        {
            FabricaL.GetContrato().ModContrato(contrato, logueo);
        }

        Contrato IMiServicio.BuscarContrato(int codigoEmpresa, int codTipo, Usuario logueo)
        {
            return FabricaL.GetContrato().BuscarContrato(codigoEmpresa, codTipo, logueo);
        }

        List<Contrato> IMiServicio.ListarContrato(Empresa empresa, Usuario logueo)
        {
            return FabricaL.GetContrato().ListarContrato(empresa, logueo);
        }

        string IMiServicio.GenerarXMLContratos()
        {
            return FabricaL.GetContrato().GenerarXMLContratos();
        }

        DateTime IMiServicio.ChequearFacturaPaga(string factura)
        {
            return FabricaL.GetContrato().ChequearFacturaPaga(factura);
        }
        #endregion

        #region Pagos
        void IMiServicio.AltaPago(Pago pago, Usuario logueo)
        {
            FabricaL.GetPago().AltaPago(pago, logueo);
        }

        List<Pago> IMiServicio.ListarPagos(Usuario logueo)
        {
            return FabricaL.GetPago().ListarPagos(logueo);
        }

        List<LineaPago> IMiServicio.ListarFacturas(int _NumeroInterno, Usuario logueo)
        {
            return FabricaL.GetPago().ListarFacturas(_NumeroInterno, logueo);
        }
        #endregion
    }
}
