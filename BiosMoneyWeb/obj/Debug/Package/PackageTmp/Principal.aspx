<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Principal.aspx.cs" Inherits="BiosMoneyWeb.Principal" %>
    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link rel="stylesheet" href="css/main.css"/>
        
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div id="contenedor_principal">
            <div id="contenedor_banner">
                <img src="imagenes/Banner-biosmoney.png" alt="" id="imgBanner"/>
                <h2>Consultá tus pagos y <br>nuestras empresas adheridas</h2>
            </div>
            <div id="contenedor_descripcion">
                <img src="imagenes/logo_biosmoney.png" alt="" id="logoDescripcion"/>
                <div>
                    <h2>¡Bienvenido a nuestro Sitio Web!</h2>
                    <h3><i class="fa fa-check-square-o" aria-hidden="true"></i> Información sobre empresas asociadas y tipos de contratos</h3>
                    <h3><i class="fa fa-check-square-o" aria-hidden="true"></i> Consultá la información de tus pagos sólo con el <br />código de barras de tu factura</h3>
                </div>
            </div>
        </div>
    </asp:Content>
