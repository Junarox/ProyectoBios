﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BiosMoneyWeb.ServicioWCF {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Usuario", Namespace="http://schemas.datacontract.org/2004/07/EntidadesCompartidas")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(BiosMoneyWeb.ServicioWCF.Gerente))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(BiosMoneyWeb.ServicioWCF.Cajero))]
    public partial class Usuario : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int CiField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ClaveField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NomCompletoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UsuField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Ci {
            get {
                return this.CiField;
            }
            set {
                if ((this.CiField.Equals(value) != true)) {
                    this.CiField = value;
                    this.RaisePropertyChanged("Ci");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Clave {
            get {
                return this.ClaveField;
            }
            set {
                if ((object.ReferenceEquals(this.ClaveField, value) != true)) {
                    this.ClaveField = value;
                    this.RaisePropertyChanged("Clave");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NomCompleto {
            get {
                return this.NomCompletoField;
            }
            set {
                if ((object.ReferenceEquals(this.NomCompletoField, value) != true)) {
                    this.NomCompletoField = value;
                    this.RaisePropertyChanged("NomCompleto");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Usu {
            get {
                return this.UsuField;
            }
            set {
                if ((object.ReferenceEquals(this.UsuField, value) != true)) {
                    this.UsuField = value;
                    this.RaisePropertyChanged("Usu");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Gerente", Namespace="http://schemas.datacontract.org/2004/07/EntidadesCompartidas")]
    [System.SerializableAttribute()]
    public partial class Gerente : BiosMoneyWeb.ServicioWCF.Usuario {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmailField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Email {
            get {
                return this.EmailField;
            }
            set {
                if ((object.ReferenceEquals(this.EmailField, value) != true)) {
                    this.EmailField = value;
                    this.RaisePropertyChanged("Email");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Cajero", Namespace="http://schemas.datacontract.org/2004/07/EntidadesCompartidas")]
    [System.SerializableAttribute()]
    public partial class Cajero : BiosMoneyWeb.ServicioWCF.Usuario {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string HoraFinField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string HoraInicioField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string HoraFin {
            get {
                return this.HoraFinField;
            }
            set {
                if ((object.ReferenceEquals(this.HoraFinField, value) != true)) {
                    this.HoraFinField = value;
                    this.RaisePropertyChanged("HoraFin");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string HoraInicio {
            get {
                return this.HoraInicioField;
            }
            set {
                if ((object.ReferenceEquals(this.HoraInicioField, value) != true)) {
                    this.HoraInicioField = value;
                    this.RaisePropertyChanged("HoraInicio");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Empresa", Namespace="http://schemas.datacontract.org/2004/07/EntidadesCompartidas")]
    [System.SerializableAttribute()]
    public partial class Empresa : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int CodigoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DirFiscalField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NombreField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private long RutField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private long TelField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Codigo {
            get {
                return this.CodigoField;
            }
            set {
                if ((this.CodigoField.Equals(value) != true)) {
                    this.CodigoField = value;
                    this.RaisePropertyChanged("Codigo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DirFiscal {
            get {
                return this.DirFiscalField;
            }
            set {
                if ((object.ReferenceEquals(this.DirFiscalField, value) != true)) {
                    this.DirFiscalField = value;
                    this.RaisePropertyChanged("DirFiscal");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nombre {
            get {
                return this.NombreField;
            }
            set {
                if ((object.ReferenceEquals(this.NombreField, value) != true)) {
                    this.NombreField = value;
                    this.RaisePropertyChanged("Nombre");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long Rut {
            get {
                return this.RutField;
            }
            set {
                if ((this.RutField.Equals(value) != true)) {
                    this.RutField = value;
                    this.RaisePropertyChanged("Rut");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long Tel {
            get {
                return this.TelField;
            }
            set {
                if ((this.TelField.Equals(value) != true)) {
                    this.TelField = value;
                    this.RaisePropertyChanged("Tel");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Contrato", Namespace="http://schemas.datacontract.org/2004/07/EntidadesCompartidas")]
    [System.SerializableAttribute()]
    public partial class Contrato : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int CodContratoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private BiosMoneyWeb.ServicioWCF.Empresa EmpresaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NomContratoField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int CodContrato {
            get {
                return this.CodContratoField;
            }
            set {
                if ((this.CodContratoField.Equals(value) != true)) {
                    this.CodContratoField = value;
                    this.RaisePropertyChanged("CodContrato");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public BiosMoneyWeb.ServicioWCF.Empresa Empresa {
            get {
                return this.EmpresaField;
            }
            set {
                if ((object.ReferenceEquals(this.EmpresaField, value) != true)) {
                    this.EmpresaField = value;
                    this.RaisePropertyChanged("Empresa");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NomContrato {
            get {
                return this.NomContratoField;
            }
            set {
                if ((object.ReferenceEquals(this.NomContratoField, value) != true)) {
                    this.NomContratoField = value;
                    this.RaisePropertyChanged("NomContrato");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Pago", Namespace="http://schemas.datacontract.org/2004/07/EntidadesCompartidas")]
    [System.SerializableAttribute()]
    public partial class Pago : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private BiosMoneyWeb.ServicioWCF.Usuario CajeroField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime FechaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private BiosMoneyWeb.ServicioWCF.LineaPago[] LineasPagoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int MontoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> NumeroInternoField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public BiosMoneyWeb.ServicioWCF.Usuario Cajero {
            get {
                return this.CajeroField;
            }
            set {
                if ((object.ReferenceEquals(this.CajeroField, value) != true)) {
                    this.CajeroField = value;
                    this.RaisePropertyChanged("Cajero");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Fecha {
            get {
                return this.FechaField;
            }
            set {
                if ((this.FechaField.Equals(value) != true)) {
                    this.FechaField = value;
                    this.RaisePropertyChanged("Fecha");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public BiosMoneyWeb.ServicioWCF.LineaPago[] LineasPago {
            get {
                return this.LineasPagoField;
            }
            set {
                if ((object.ReferenceEquals(this.LineasPagoField, value) != true)) {
                    this.LineasPagoField = value;
                    this.RaisePropertyChanged("LineasPago");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Monto {
            get {
                return this.MontoField;
            }
            set {
                if ((this.MontoField.Equals(value) != true)) {
                    this.MontoField = value;
                    this.RaisePropertyChanged("Monto");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> NumeroInterno {
            get {
                return this.NumeroInternoField;
            }
            set {
                if ((this.NumeroInternoField.Equals(value) != true)) {
                    this.NumeroInternoField = value;
                    this.RaisePropertyChanged("NumeroInterno");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="LineaPago", Namespace="http://schemas.datacontract.org/2004/07/EntidadesCompartidas")]
    [System.SerializableAttribute()]
    public partial class LineaPago : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CodContratoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int CodigoClienteField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private BiosMoneyWeb.ServicioWCF.Contrato ContratoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime FechaVencimientoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int MontoField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CodContrato {
            get {
                return this.CodContratoField;
            }
            set {
                if ((object.ReferenceEquals(this.CodContratoField, value) != true)) {
                    this.CodContratoField = value;
                    this.RaisePropertyChanged("CodContrato");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int CodigoCliente {
            get {
                return this.CodigoClienteField;
            }
            set {
                if ((this.CodigoClienteField.Equals(value) != true)) {
                    this.CodigoClienteField = value;
                    this.RaisePropertyChanged("CodigoCliente");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public BiosMoneyWeb.ServicioWCF.Contrato Contrato {
            get {
                return this.ContratoField;
            }
            set {
                if ((object.ReferenceEquals(this.ContratoField, value) != true)) {
                    this.ContratoField = value;
                    this.RaisePropertyChanged("Contrato");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime FechaVencimiento {
            get {
                return this.FechaVencimientoField;
            }
            set {
                if ((this.FechaVencimientoField.Equals(value) != true)) {
                    this.FechaVencimientoField = value;
                    this.RaisePropertyChanged("FechaVencimiento");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Monto {
            get {
                return this.MontoField;
            }
            set {
                if ((this.MontoField.Equals(value) != true)) {
                    this.MontoField = value;
                    this.RaisePropertyChanged("Monto");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServicioWCF.IMiServicio")]
    public interface IMiServicio {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMiServicio/Logueo", ReplyAction="http://tempuri.org/IMiServicio/LogueoResponse")]
        BiosMoneyWeb.ServicioWCF.Usuario Logueo(string usuario, string clave);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMiServicio/Alta", ReplyAction="http://tempuri.org/IMiServicio/AltaResponse")]
        void Alta(BiosMoneyWeb.ServicioWCF.Usuario usuario, BiosMoneyWeb.ServicioWCF.Usuario logueo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMiServicio/ListarGerentes", ReplyAction="http://tempuri.org/IMiServicio/ListarGerentesResponse")]
        BiosMoneyWeb.ServicioWCF.Gerente[] ListarGerentes(BiosMoneyWeb.ServicioWCF.Usuario logueo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMiServicio/ModificarClave", ReplyAction="http://tempuri.org/IMiServicio/ModificarClaveResponse")]
        void ModificarClave(BiosMoneyWeb.ServicioWCF.Usuario usuario, string clave1, string clave2);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMiServicio/BajaCajero", ReplyAction="http://tempuri.org/IMiServicio/BajaCajeroResponse")]
        void BajaCajero(BiosMoneyWeb.ServicioWCF.Usuario cajero, BiosMoneyWeb.ServicioWCF.Usuario logueo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMiServicio/BuscarCajero", ReplyAction="http://tempuri.org/IMiServicio/BuscarCajeroResponse")]
        BiosMoneyWeb.ServicioWCF.Cajero BuscarCajero(int Ci, BiosMoneyWeb.ServicioWCF.Usuario logueo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMiServicio/Modificar", ReplyAction="http://tempuri.org/IMiServicio/ModificarResponse")]
        void Modificar(BiosMoneyWeb.ServicioWCF.Usuario cajero, BiosMoneyWeb.ServicioWCF.Usuario logueo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMiServicio/ListarCajeros", ReplyAction="http://tempuri.org/IMiServicio/ListarCajerosResponse")]
        BiosMoneyWeb.ServicioWCF.Cajero[] ListarCajeros(BiosMoneyWeb.ServicioWCF.Usuario logueo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMiServicio/ActualizarHorasExtra", ReplyAction="http://tempuri.org/IMiServicio/ActualizarHorasExtraResponse")]
        void ActualizarHorasExtra(BiosMoneyWeb.ServicioWCF.Cajero _cajero, System.DateTime _fecha, int _minutosExtra);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMiServicio/AltaEmpresa", ReplyAction="http://tempuri.org/IMiServicio/AltaEmpresaResponse")]
        void AltaEmpresa(BiosMoneyWeb.ServicioWCF.Empresa empresa, BiosMoneyWeb.ServicioWCF.Usuario logueo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMiServicio/ModEmpresa", ReplyAction="http://tempuri.org/IMiServicio/ModEmpresaResponse")]
        void ModEmpresa(BiosMoneyWeb.ServicioWCF.Empresa empresa, BiosMoneyWeb.ServicioWCF.Usuario logueo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMiServicio/BajaEmpresa", ReplyAction="http://tempuri.org/IMiServicio/BajaEmpresaResponse")]
        void BajaEmpresa(BiosMoneyWeb.ServicioWCF.Empresa empresa, BiosMoneyWeb.ServicioWCF.Usuario logueo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMiServicio/BuscarEmpresa", ReplyAction="http://tempuri.org/IMiServicio/BuscarEmpresaResponse")]
        BiosMoneyWeb.ServicioWCF.Empresa BuscarEmpresa(int codigo, BiosMoneyWeb.ServicioWCF.Usuario logueo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMiServicio/ListarEmpresa", ReplyAction="http://tempuri.org/IMiServicio/ListarEmpresaResponse")]
        BiosMoneyWeb.ServicioWCF.Empresa[] ListarEmpresa(BiosMoneyWeb.ServicioWCF.Usuario logueo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMiServicio/AltaContrato", ReplyAction="http://tempuri.org/IMiServicio/AltaContratoResponse")]
        void AltaContrato(BiosMoneyWeb.ServicioWCF.Contrato contrato, BiosMoneyWeb.ServicioWCF.Usuario logueo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMiServicio/BajaContrato", ReplyAction="http://tempuri.org/IMiServicio/BajaContratoResponse")]
        void BajaContrato(BiosMoneyWeb.ServicioWCF.Contrato contrato, BiosMoneyWeb.ServicioWCF.Usuario logueo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMiServicio/ModContrato", ReplyAction="http://tempuri.org/IMiServicio/ModContratoResponse")]
        void ModContrato(BiosMoneyWeb.ServicioWCF.Contrato contrato, BiosMoneyWeb.ServicioWCF.Usuario logueo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMiServicio/BuscarContrato", ReplyAction="http://tempuri.org/IMiServicio/BuscarContratoResponse")]
        BiosMoneyWeb.ServicioWCF.Contrato BuscarContrato(int codigoEmpresa, int codTipo, BiosMoneyWeb.ServicioWCF.Usuario logueo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMiServicio/ListarContrato", ReplyAction="http://tempuri.org/IMiServicio/ListarContratoResponse")]
        BiosMoneyWeb.ServicioWCF.Contrato[] ListarContrato(BiosMoneyWeb.ServicioWCF.Empresa empresa, BiosMoneyWeb.ServicioWCF.Usuario logueo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMiServicio/GenerarXMLContratos", ReplyAction="http://tempuri.org/IMiServicio/GenerarXMLContratosResponse")]
        string GenerarXMLContratos();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMiServicio/ChequearFacturaPaga", ReplyAction="http://tempuri.org/IMiServicio/ChequearFacturaPagaResponse")]
        System.DateTime ChequearFacturaPaga(string factura);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMiServicio/AltaPago", ReplyAction="http://tempuri.org/IMiServicio/AltaPagoResponse")]
        void AltaPago(BiosMoneyWeb.ServicioWCF.Pago pago, BiosMoneyWeb.ServicioWCF.Usuario logueo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMiServicio/ListarPagos", ReplyAction="http://tempuri.org/IMiServicio/ListarPagosResponse")]
        BiosMoneyWeb.ServicioWCF.Pago[] ListarPagos(BiosMoneyWeb.ServicioWCF.Usuario logueo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMiServicio/ListarFacturas", ReplyAction="http://tempuri.org/IMiServicio/ListarFacturasResponse")]
        BiosMoneyWeb.ServicioWCF.LineaPago[] ListarFacturas(int _NumeroInterno, BiosMoneyWeb.ServicioWCF.Usuario logueo);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMiServicioChannel : BiosMoneyWeb.ServicioWCF.IMiServicio, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MiServicioClient : System.ServiceModel.ClientBase<BiosMoneyWeb.ServicioWCF.IMiServicio>, BiosMoneyWeb.ServicioWCF.IMiServicio {
        
        public MiServicioClient() {
        }
        
        public MiServicioClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public MiServicioClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MiServicioClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MiServicioClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public BiosMoneyWeb.ServicioWCF.Usuario Logueo(string usuario, string clave) {
            return base.Channel.Logueo(usuario, clave);
        }
        
        public void Alta(BiosMoneyWeb.ServicioWCF.Usuario usuario, BiosMoneyWeb.ServicioWCF.Usuario logueo) {
            base.Channel.Alta(usuario, logueo);
        }
        
        public BiosMoneyWeb.ServicioWCF.Gerente[] ListarGerentes(BiosMoneyWeb.ServicioWCF.Usuario logueo) {
            return base.Channel.ListarGerentes(logueo);
        }
        
        public void ModificarClave(BiosMoneyWeb.ServicioWCF.Usuario usuario, string clave1, string clave2) {
            base.Channel.ModificarClave(usuario, clave1, clave2);
        }
        
        public void BajaCajero(BiosMoneyWeb.ServicioWCF.Usuario cajero, BiosMoneyWeb.ServicioWCF.Usuario logueo) {
            base.Channel.BajaCajero(cajero, logueo);
        }
        
        public BiosMoneyWeb.ServicioWCF.Cajero BuscarCajero(int Ci, BiosMoneyWeb.ServicioWCF.Usuario logueo) {
            return base.Channel.BuscarCajero(Ci, logueo);
        }
        
        public void Modificar(BiosMoneyWeb.ServicioWCF.Usuario cajero, BiosMoneyWeb.ServicioWCF.Usuario logueo) {
            base.Channel.Modificar(cajero, logueo);
        }
        
        public BiosMoneyWeb.ServicioWCF.Cajero[] ListarCajeros(BiosMoneyWeb.ServicioWCF.Usuario logueo) {
            return base.Channel.ListarCajeros(logueo);
        }
        
        public void ActualizarHorasExtra(BiosMoneyWeb.ServicioWCF.Cajero _cajero, System.DateTime _fecha, int _minutosExtra) {
            base.Channel.ActualizarHorasExtra(_cajero, _fecha, _minutosExtra);
        }
        
        public void AltaEmpresa(BiosMoneyWeb.ServicioWCF.Empresa empresa, BiosMoneyWeb.ServicioWCF.Usuario logueo) {
            base.Channel.AltaEmpresa(empresa, logueo);
        }
        
        public void ModEmpresa(BiosMoneyWeb.ServicioWCF.Empresa empresa, BiosMoneyWeb.ServicioWCF.Usuario logueo) {
            base.Channel.ModEmpresa(empresa, logueo);
        }
        
        public void BajaEmpresa(BiosMoneyWeb.ServicioWCF.Empresa empresa, BiosMoneyWeb.ServicioWCF.Usuario logueo) {
            base.Channel.BajaEmpresa(empresa, logueo);
        }
        
        public BiosMoneyWeb.ServicioWCF.Empresa BuscarEmpresa(int codigo, BiosMoneyWeb.ServicioWCF.Usuario logueo) {
            return base.Channel.BuscarEmpresa(codigo, logueo);
        }
        
        public BiosMoneyWeb.ServicioWCF.Empresa[] ListarEmpresa(BiosMoneyWeb.ServicioWCF.Usuario logueo) {
            return base.Channel.ListarEmpresa(logueo);
        }
        
        public void AltaContrato(BiosMoneyWeb.ServicioWCF.Contrato contrato, BiosMoneyWeb.ServicioWCF.Usuario logueo) {
            base.Channel.AltaContrato(contrato, logueo);
        }
        
        public void BajaContrato(BiosMoneyWeb.ServicioWCF.Contrato contrato, BiosMoneyWeb.ServicioWCF.Usuario logueo) {
            base.Channel.BajaContrato(contrato, logueo);
        }
        
        public void ModContrato(BiosMoneyWeb.ServicioWCF.Contrato contrato, BiosMoneyWeb.ServicioWCF.Usuario logueo) {
            base.Channel.ModContrato(contrato, logueo);
        }
        
        public BiosMoneyWeb.ServicioWCF.Contrato BuscarContrato(int codigoEmpresa, int codTipo, BiosMoneyWeb.ServicioWCF.Usuario logueo) {
            return base.Channel.BuscarContrato(codigoEmpresa, codTipo, logueo);
        }
        
        public BiosMoneyWeb.ServicioWCF.Contrato[] ListarContrato(BiosMoneyWeb.ServicioWCF.Empresa empresa, BiosMoneyWeb.ServicioWCF.Usuario logueo) {
            return base.Channel.ListarContrato(empresa, logueo);
        }
        
        public string GenerarXMLContratos() {
            return base.Channel.GenerarXMLContratos();
        }
        
        public System.DateTime ChequearFacturaPaga(string factura) {
            return base.Channel.ChequearFacturaPaga(factura);
        }
        
        public void AltaPago(BiosMoneyWeb.ServicioWCF.Pago pago, BiosMoneyWeb.ServicioWCF.Usuario logueo) {
            base.Channel.AltaPago(pago, logueo);
        }
        
        public BiosMoneyWeb.ServicioWCF.Pago[] ListarPagos(BiosMoneyWeb.ServicioWCF.Usuario logueo) {
            return base.Channel.ListarPagos(logueo);
        }
        
        public BiosMoneyWeb.ServicioWCF.LineaPago[] ListarFacturas(int _NumeroInterno, BiosMoneyWeb.ServicioWCF.Usuario logueo) {
            return base.Channel.ListarFacturas(_NumeroInterno, logueo);
        }
    }
}
