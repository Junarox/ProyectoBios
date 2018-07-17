using System.ServiceModel;
using EntidadesCompartidas;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;
namespace MiServicio
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMiServicio" in both code and config file together.
    [ServiceContract]
    public interface IMiServicio
    {
        #region Contrato
        [OperationContract]
        void AltaContrato(Contrato _contrato);

        [OperationContract]
        void BajaContrato(Empresa Empresa, int CodTipo);

        [OperationContract]
        void ModContrato(Contrato _contrato);

        [OperationContract]
        Contrato BuscarContrato(int _CodigoEmpresa, int _CodTipo);

        [OperationContract]
        List<Contrato> ListarContrato(Empresa Empresa);

        [OperationContract]
        string GenerarXMLContratos();

        [OperationContract]
        DateTime ChequearFacturaPaga(string _factura);
        #endregion

        #region Empresa
        [OperationContract]
        void AltaEmpresa(Empresa _empresa);

        [OperationContract]
        void ModEmpresa(Empresa _empresa);

        [OperationContract]
        void BajaEmpresa(Empresa _empresa);

        [OperationContract]
        Empresa BuscarEmpresa(int _codigo);

        [OperationContract]
        List<Empresa> ListarEmpresa();
        #endregion

        #region Pago
        [OperationContract]
        void AltaPago(Pago _pago);

        [OperationContract]
        List<Pago> ListarPagos();

        [OperationContract]
        List<LineaPago> ListarFacturas(int _NumeroInterno);
        #endregion

        #region Usuario
        [OperationContract]
        Usuario Logueo(string usu, string clave);

        [OperationContract]
        void Alta(Usuario _usu);

        [OperationContract]
        void BajaCajero(Usuario _cajero);

        [OperationContract]
        Cajero BuscarCajero(int Ci);

        [OperationContract]
        List<Cajero> ListarCajero();

        [OperationContract]
        void Modificar(Usuario _cajero);

        [OperationContract]
        void ModificarClave(Usuario _usu, string clave1, string clave2);

        [OperationContract]
        void ActualizarHorasExtra(Cajero _cajero, DateTime _fecha, int _minutosExtra);
        #endregion
    }
}
