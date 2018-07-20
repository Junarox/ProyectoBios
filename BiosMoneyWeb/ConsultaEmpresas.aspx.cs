using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.Web.Services;
using System.Web.Script.Services;
using System.Xml.Xsl;
using BiosMoneyWeb.ServicioWCF;

namespace BiosMoneyWeb
{
    public partial class ConsultaEmpresas : System.Web.UI.Page
    {
        IMiServicio servicio = new MiServicioClient();
        //Instancio documento XML para trabajar en memoria
        static XDocument doc = new XDocument();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    //Cargo documento XML
                    string docAux = servicio.GenerarXMLContratos();
                    doc = System.Xml.Linq.XDocument.Parse(docAux);
                    panel_contratos.Visible = false;
                }

                if (doc != null)
                {
                    //Linq to XML para obtener datos de las Empresas
                    IEnumerable<XElement> empresas =
                (from empresa in doc.Elements("Contratos").Elements("Contrato")
                 group empresa by (string)empresa.Element("CodigoEmpresa") into g
                 select g.First()).ToList();

                    if (empresas.Count() > 0)
                    {
                        lblTitulo.Text = "Lista de Empresas Adheridas";
                        //Cargo datos en repeater
                        repeaterEmpresas.DataSource = empresas;
                        repeaterEmpresas.DataBind();
                    }
                }
                else
                {
                    lblTitulo.Text = "No hay empresas adheridas hasta el momento";
                }
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                if (ex.Detail.InnerText.Length > 40)
                    throw new Exception(ex.Detail.InnerText.Substring(0, 40));
                else
                    throw new Exception(ex.Detail.InnerText);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void btnVerEmpresa_Click(object sender, EventArgs e)
        {
            try
            {
                //Obtengo parámetro con código de empresa
                Button btn = (sender as Button);
                string codigoEmpresa = btn.CommandArgument;


                //Linq to XML al documento de contratos
                IEnumerable<XElement> contratosXML =
                  from contratos in doc.Elements("Contratos").Elements("Contrato")
                  where (string)contratos.Element("CodigoEmpresa") == codigoEmpresa
                  select contratos;


                //Genero nuevo XML y cargo los datos obtenidos
                XDocument doc2 = new XDocument(new XElement("Contratos"));
                doc2.Element("Contratos").Add(contratosXML);

                //Cargo datos del XML en el control y aplico XSLT
                xml_contratos.DocumentContent = doc2.ToString();
                xml_contratos.TransformSource = Server.MapPath("xslt\\xslt.xslt");


                panel_contratos.Visible = true;
                repeaterEmpresas.Visible = false;



                string xx = contratosXML.ToString();
                lblTitulo.Text = "Tipos de Contrato de la Empresa </br> " + contratosXML.First().Element("NombreEmpresa").Value;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void BtnVolver_Click(object sender, EventArgs e)
        {
            string docAux = servicio.GenerarXMLContratos();
            doc = System.Xml.Linq.XDocument.Parse(docAux);
            if (doc != null)
            {
                panel_contratos.Visible = false;
                repeaterEmpresas.Visible = true;
                lblTitulo.Text = "Lista de Empresas Adheridas";
            }
            else
            {
                panel_contratos.Visible = false;
                repeaterEmpresas.Visible = false;
                lblTitulo.Text = "No hay empresas adheridas hasta el momento";
            }

        }
    }
}