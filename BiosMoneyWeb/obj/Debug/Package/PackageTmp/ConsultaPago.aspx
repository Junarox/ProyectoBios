<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ConsultaPago.aspx.cs" Inherits="BiosMoneyWeb.ConsultaPago" %>
    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div id="contenedorConsulta">
        <asp:Label ID="Label1" runat="server" Text="Ingrese el código de barras de su factura..."></asp:Label><br /><br />
        <asp:TextBox ID="txtCodigoBarras" runat="server" Font-Size="X-Large" MaxLength="25"></asp:TextBox>
        <asp:Button ID="btnConsultar" runat="server" Text="Consultar" Font-Size="X-Large" 
                onclick="btnConsultar_Click" /><br /><br />

            <asp:Label ID="lblResultado" runat="server" Text=""></asp:Label>
</div>
    </asp:Content>
