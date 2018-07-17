using Logica;
using EntidadesCompartidas;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;
namespace MiServicio
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MiServicio" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MiServicio.svc or MiServicio.svc.cs at the Solution Explorer and start debugging.
    public class MiServicio : IMiServicio
    {
        #region Contrato
        void IMiServicio.AltaContrato(Contrato _contrato)
        {
            FabricaL.GetContrato().AltaContrato(_contrato);
        }


        void IMiServicio.BajaContrato(Empresa Empresa, int CodTipo)
        {
            FabricaL.GetContrato().BajaContrato(Empresa, CodTipo);
        }


        void IMiServicio.ModContrato(Contrato _contrato)
        {
            FabricaL.GetContrato().ModContrato(_contrato);
        }


        Contrato IMiServicio.BuscarContrato(int _CodigoEmpresa, int _CodTipo)
        {
            return (FabricaL.GetContrato().BuscarContrato(_CodigoEmpresa, _CodTipo));
        }


        List<Contrato> IMiServicio.ListarContrato(Empresa Empresa)
        {
            return (FabricaL.GetContrato().ListarContrato(Empresa));
        }


        string IMiServicio.GenerarXMLContratos()
        {
            return (FabricaL.GetContrato().GenerarXMLContratos());
        }


        DateTime IMiServicio.ChequearFacturaPaga(string _factura)
        {
            return (FabricaL.GetContrato().ChequearFacturaPaga(_factura));
        }
        #endregion

        #region Empresa
        void IMiServicio.AltaEmpresa(Empresa _empresa)
        {
            FabricaL.GetEmpresa().AltaEmpresa(_empresa);
        }

        void IMiServicio.ModEmpresa(Empresa _empresa)
        {
            FabricaL.GetEmpresa().ModEmpresa(_empresa);
        }

        void IMiServicio.BajaEmpresa(Empresa _empresa)
        {
            FabricaL.GetEmpresa().BajaEmpresa(_empresa);
        }

        Empresa IMiServicio.BuscarEmpresa(int _codigo)
        {
            return (FabricaL.GetEmpresa().BuscarEmpresa(_codigo));
        }
        List<Empresa> IMiServicio.ListarEmpresa()
        {
            return (FabricaL.GetEmpresa().ListarEmpresa());
        }
        #endregion

        #region Pago
        void IMiServicio.AltaPago(Pago _pago)
        {
            FabricaL.GetPago().AltaPago(_pago);
        }

        List<Pago> IMiServicio.ListarPagos()
        {
            return (FabricaL.GetPago().ListarPagos());
        }

        List<LineaPago> IMiServicio.ListarFacturas(int _NumeroInterno)
        {
            return (FabricaL.GetPago().ListarFacturas(_NumeroInterno));
        }
        #endregion

        #region Usuario
        Usuario IMiServicio.Logueo(string usu, string clave)
        {
            return (FabricaL.GetLUsuario().Logueo(usu, clave));
        }

        void IMiServicio.Alta(Usuario _usu)
        {
            FabricaL.GetLUsuario().Alta(_usu);
        }

        void IMiServicio.BajaCajero(Usuario _cajero)
        {
            FabricaL.GetLUsuario().BajaCajero(_cajero);
        }

        Cajero IMiServicio.BuscarCajero(int Ci)
        {
            return (FabricaL.GetLUsuario().BuscarCajero(Ci));
        }

        List<Cajero> IMiServicio.ListarCajero()
        {
            return (FabricaL.GetLUsuario().ListarCajero());
        }

        void IMiServicio.Modificar(Usuario _cajero)
        {
            FabricaL.GetLUsuario().Modificar(_cajero);
        }

        void IMiServicio.ModificarClave(Usuario _usu, string clave1, string clave2)
        {
            FabricaL.GetLUsuario().ModificarClave(_usu, clave1, clave2);
        }

        void IMiServicio.ActualizarHorasExtra(Cajero _cajero, DateTime _fecha, int _minutosExtra)
        {
            FabricaL.GetLUsuario().ActualizarHorasExtra(_cajero, _fecha, _minutosExtra);
        }
        #endregion
    }
}