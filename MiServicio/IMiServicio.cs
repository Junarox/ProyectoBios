using EntidadesCompartidas;
using Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MiServicioWCF
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IMiServicio" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IMiServicio
    {
        #region Empleados

        [OperationContract]
        Usuario Logueo(string usuario, string clave);

        [OperationContract]
        void Alta(Usuario usuario, Usuario logueo);

        [OperationContract]
        List<Gerente> ListarGerentes(Usuario logueo);

        [OperationContract]
        void ModificarClave(Usuario usuario, string clave1, string clave2);

        #region Cajeros
        [OperationContract]
        void BajaCajero(Usuario cajero, Usuario logueo);

        [OperationContract]
        Cajero BuscarCajero(int Ci, Usuario logueo);

        [OperationContract]
        void Modificar(Usuario cajero, Usuario logueo);

        [OperationContract]
        List<Cajero> ListarCajeros(Usuario logueo);

        [OperationContract]
        void ActualizarHorasExtra(Cajero _cajero, DateTime _fecha, int _minutosExtra);
        #endregion
        #endregion

        #region Empresas
        [OperationContract]
        void AltaEmpresa(Empresa empresa, Usuario logueo);

        [OperationContract]
        void ModEmpresa(Empresa empresa, Usuario logueo);

        [OperationContract]
        void BajaEmpresa(Empresa empresa, Usuario logueo);

        [OperationContract]
        Empresa BuscarEmpresa(int codigo, Usuario logueo);

        [OperationContract]
        List<Empresa> ListarEmpresa(Usuario logueo);
        #endregion

        #region Contratos
        [OperationContract]
        void AltaContrato(Contrato contrato, Usuario logueo);

        [OperationContract]
        void BajaContrato(Contrato contrato, Usuario logueo);

        [OperationContract]
        void ModContrato(Contrato contrato, Usuario logueo);

        [OperationContract]
        Contrato BuscarContrato(int codigoEmpresa, int codTipo, Usuario logueo);

        [OperationContract]
        List<Contrato> ListarContrato(Empresa empresa, Usuario logueo);

        [OperationContract]
        string GenerarXMLContratos();

        [OperationContract]
        DateTime ChequearFacturaPaga(string factura);
        #endregion

        #region Pagos
        [OperationContract]
        void AltaPago(Pago pago, Usuario logueo);

        [OperationContract]
        List<Pago> ListarPagos(Usuario logueo);

        [OperationContract]
        List<LineaPago> ListarFacturas(int _NumeroInterno, Usuario logueo);
        #endregion
    }
}
