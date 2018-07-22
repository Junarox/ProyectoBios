using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BiosMoneyApp.Servicio;

namespace BiosMoneyApp.GerenteApp
{
    public partial class ListadoPagos : Form
    {
        IMiServicio SServicio = new MiServicioClient();
        Usuario usuario;
        List<Pago> pagos = new List<Pago>();
        static List<Pago> listaPagos;

        public ListadoPagos(Usuario usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                switch (cmbFiltros.SelectedIndex)
                {
                    //Buscar pagos por cajero
                    case 2:
                        //Actualizo lista de pagos
                        listaPagos = SServicio.ListarPagos(usuario).ToList();

                        int aux;
                        //Chequeo que la CI sea numerica y tenga 8 caracteres
                        if (int.TryParse(txtFiltros.Text, out aux) && txtFiltros.Text.Length == 8)
                        {
                            //LinQ to Objects
                            IEnumerable<Pago> pagosPorCajero =
                      from pagos in listaPagos
                      where pagos.Cajero.Ci.ToString() == txtFiltros.Text
                      orderby pagos.Fecha descending
                      select pagos;

                            //Si encuentro resultados los muestro, sino muestro error.
                            if (pagosPorCajero.Count() > 0)
                            {
                                DataTable dt = new DataTable();
                                DataColumn dc = new DataColumn("Fecha");
                                DataColumn dc1 = new DataColumn("Monto Total");
                                dt.Columns.Add(dc);
                                dt.Columns.Add(dc1);

                                foreach (var p in pagosPorCajero)
                                {
                                    DataRow dr = dt.NewRow();
                                    dr["Fecha"] = p.Fecha;
                                    dr["Monto Total"] = p.Monto;
                                    dt.Rows.Add(dr);
                                }
                                gvPagos.DataSource = dt;
                            }
                            else
                            {
                                txtFiltros.Text = "";
                                throw new Exception("No se encontraron resultados.");
                            }
                        }
                        else
                        {
                            txtFiltros.Text = "";
                            throw new Exception("La cédula debe contener 8 caracteres numéricos.");
                        }
                        break;
                    //Buscar pagos por Empresa
                    case 3:
                        //Actualizo lista de pagos
                        listaPagos = SServicio.ListarPagos(usuario).ToList();


                        //Chequeo que el còdigo sea numerica y tenga 8 caracteres
                        if (int.TryParse(txtFiltros.Text, out aux) && txtFiltros.Text.Length == 4)
                        {

                            /*      //LinQ to Objects
                                  IEnumerable<LineaPago> pagosPorEmpresa =
                            from pagos in listaPagos
                            where pagos.LineasPago.Contains == txtFiltros.Text
                            orderby pagos.Fecha descending
                            select pagos;


                                  //Si encuentro resultados los muestro, sino muestro error.
                                  if (pagosPorEmpresa.Count() > 0)
                                  {
                                      DataTable dt = new DataTable();
                                      DataColumn dc = new DataColumn("Fecha Vencimiento");
                                      DataColumn dc1 = new DataColumn("Monto");
                                      DataColumn dc2 = new DataColumn("Tipo de Contrato");
                                      dt.Columns.Add(dc);
                                      dt.Columns.Add(dc1);
                                      dt.Columns.Add(dc2);

                                      foreach (var f in pagosPorEmpresa)
                                      {
                                          DataRow dr = dt.NewRow();
                                          dr["Fecha Vencimiento"] = f.FechaVencimiento;
                                          dr["Monto"] = f.Monto;
                                          dr["Tipo de Contrato"] = f.Contrato.CodContrato;
                                          dt.Rows.Add(dr);
                                      }
                                      gvPagos.DataSource = dt;
                                  }
                                  else
                                  {
                                      txtFiltros.Text = "";
                                      throw new Exception("No se encontraron resultados.");
                                  }*/
                        }
                        else
                        {
                            txtFiltros.Text = "";
                            throw new Exception("El código de Empresa debe contener 4 caracteres numéricos.");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ListadoDePagos_Load(object sender, EventArgs e)
        {
            //Preparo el combo
            cmbFiltros.Items.Add("- Seleccione -");
            cmbFiltros.Items.Add("Resumen Cajeros");
            cmbFiltros.Items.Add("Usuario Cajero");
            cmbFiltros.Items.Add("Empresa");


            try
            {
                //Si se encuentran pagos los muestro, sino muestro error.
                cmbFiltros.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void MostrarTodosLosPagos()
        {
            try
            {
                //Actualizo lista de pagos
                listaPagos = SServicio.ListarPagos(usuario).ToList();

                if (listaPagos != null)
                {
                    //Genero DataTable y sus columnas
                    DataTable dt = new DataTable();
                    DataColumn dc = new DataColumn("Número Interno");
                    DataColumn dc1 = new DataColumn("Fecha");
                    DataColumn dc2 = new DataColumn("Monto");
                    DataColumn dc3 = new DataColumn("Cajero");

                    dt.Columns.Add(dc);
                    dt.Columns.Add(dc1);
                    dt.Columns.Add(dc2);
                    dt.Columns.Add(dc3);

                    //Creo una row por cada registro encontrado y la agrego a la tabla
                    foreach (Pago p in listaPagos)
                    {
                        DataRow dr = dt.NewRow();
                        dr["Número Interno"] = p.NumeroInterno;
                        dr["Fecha"] = p.Fecha;
                        dr["Monto"] = p.Monto;
                        dr["Cajero"] = p.Cajero.Ci + " (" + p.Cajero.NomCompleto + ")";
                        dt.Rows.Add(dr);
                    }

                    gvPagos.DataSource = dt;
                    lblFiltros.Visible = false;
                    txtFiltros.Visible = false;
                    btnBuscar.Visible = false;
                    txtFiltros.Text = "";
                }
                else
                {
                    this.Dispose();
                    throw new Exception("No hay pagos para mostrar.");
                }


            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                if (ex.Detail.InnerText.Length > 40)
                    MessageBox.Show(ex.Detail.InnerText.Substring(0, 40));
                else
                    MessageBox.Show(ex.Detail.InnerText);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void cmbContratos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (cmbFiltros.SelectedIndex)
                {
                    case 0:
                        {
                            //Actùa como limpiador de filtros
                            MostrarTodosLosPagos();
                        }
                        break;
                    case 1:
                        {
                            //Actualizo lista de pagos
                            listaPagos = SServicio.ListarPagos(usuario).ToList();
                            gvPagos.DataSource = null;

                            //LinQ to Objects, seleccionando como clave la CI de los cajeros
                            //y agrupando por la cantidad de veces que se repite la misma.
                            var pagos = listaPagos.GroupBy(x => x.Cajero.Ci)
                      .Select(g => new { Value = g.Key, Count = g.Count() })
                      .OrderByDescending(x => x.Count);

                            //Creo DataTable para adaptar resultados en el GridView
                            DataTable dt = new DataTable();
                            DataColumn dc = new DataColumn("Cajero");
                            DataColumn dc1 = new DataColumn("Cantidad de Pagos");
                            dt.Columns.Add(dc);
                            dt.Columns.Add(dc1);

                            //Si se encontraron resultados se generan las rows y se agregan al DataTable
                            //para luego ser asignado como DataSource del GridView
                            if (pagos.Count() > 0)
                            {
                                foreach (var x in pagos)
                                {
                                    DataRow dr = dt.NewRow();
                                    dr["Cajero"] = x.Value.ToString();
                                    dr["Cantidad de Pagos"] = x.Count.ToString();
                                    dt.Rows.Add(dr);
                                }
                                gvPagos.DataSource = dt;
                                lblFiltros.Visible = false;
                                txtFiltros.Visible = false;
                                btnBuscar.Visible = false;
                                txtFiltros.Text = "";
                            }
                            else
                            {
                                throw new Exception("No hay datos para mostrar");
                            }
                        }
                        break;
                    case 2:
                        {
                            lblFiltros.Text = "Ingrese CI:";
                            lblFiltros.Visible = true;
                            txtFiltros.Visible = true;
                            btnBuscar.Visible = true;
                            txtFiltros.MaxLength = 20;
                            txtFiltros.Text = "";

                        }
                        break;
                    case 3:
                        {
                            lblFiltros.Text = "Ingrese Código:";
                            lblFiltros.Visible = true;
                            txtFiltros.Visible = true;
                            btnBuscar.Visible = true;
                            txtFiltros.MaxLength = 4;
                            txtFiltros.Text = "";

                        }
                        break;
                    default:

                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
