<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="ConsultaEmpresas.aspx.cs" Inherits="BiosMoneyWeb.ConsultaEmpresas"
    ValidateRequest="false" EnableEventValidation="false" %>

<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="System.Xml.Xsl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="css/main.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contenedorGeneral">
        
       
        <div id="contenedorRepeater">
            <asp:Label ID="lblTitulo" runat="server" Text="Label"></asp:Label>
            <asp:Repeater ID="repeaterEmpresas" runat="server" ViewStateMode="Enabled">
                <HeaderTemplate>
                    <table>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <div id="contenedorEmpresa">
                                <asp:Label ID="lblNombre" class="labelNombreEmpresa" runat="server" Text='<%# ((XElement)Container.DataItem).Element("NombreEmpresa").Value %>'></asp:Label>
                                <asp:Button ID="Button1" class="botonSeleccionarEmpresa" runat="server" Text="Seleccionar"
                                    OnClick="btnVerEmpresa_Click" CommandArgument='<%# ((XElement)Container.DataItem).Element("CodigoEmpresa").Value %>' />
                            </div>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <asp:Panel ID="panel_contratos" runat="server">
            <table id="tabla_contratos" width="100%" border="1">
                <tr id="row_titulos">
                    <td>
                        Código
                    </td>
                    <td>
                        Nombre Contrato
                    </td>
                </tr>
                <asp:Xml ID="xml_contratos" runat="server">        
                </asp:Xml>
                <tr><td><asp:Button ID="btnVolver" class="botonSeleccionarEmpresa" runat="server" Text="Volver" onclick="BtnVolver_Click" /></td></tr>
               
            </table>
            
        </asp:Panel>
    </div>
</asp:Content>
