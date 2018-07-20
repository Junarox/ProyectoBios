using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BiosMoneyWeb.ServicioWCF;

namespace BiosMoneyWeb
{
    public partial class ConsultaPago : System.Web.UI.Page
    {
        IMiServicio servicio = new MiServicioClient();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                string codigoIngresado = txtCodigoBarras.Text;
                lblResultado.Text = "";

                //Controlo que tenga 25 caracteres
                if (txtCodigoBarras.Text.Length != 25)
                {
                    throw new Exception("El código debe tener 25 dígitos.");
                }


                //Controlo que sean sólo números y guardo los diferentes valores.                
                for (int i = 0; i <= codigoIngresado.Length - 1; i++)
                {
                    if (!char.IsDigit(codigoIngresado[i]))
                    {
                        throw new Exception("El código debe contener sólo números.");
                    }
                }

                //Buscar los datos del código de barras en la base de datos.
                DateTime fecha = servicio.ChequearFacturaPaga(txtCodigoBarras.Text.Trim());

                //Si esta paga muestra fecha de pago, si no muestra error.
                if (fecha.Year != 9999)
                {
                    lblResultado.Text = "La factura ingresada fué paga en la siguiente fecha:</br>" + fecha.Day.ToString() + "/" + fecha.Month.ToString() + "/" + fecha.Year.ToString() + " " + fecha.Hour.ToString() + ":" + fecha.Minute.ToString();
                }
                else
                {
                    lblResultado.Text = "La factura ingresada no existe o no ha sido paga aún.";
                }
            }
            catch (Exception ex)
            {
                lblResultado.Text = ex.Message;
            }

        }
    }
}