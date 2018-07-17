
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BiosMoneyApp.ServicioWCF;

namespace BiosMoneyApp.CajeroApp
{
    public partial class AltaDePago : Form
    {
        IMiServicio SServicio = new MiServicioClient();
        Usuario usu;

        private List<LineaPago> lpagos = new List<LineaPago>();

        public AltaDePago(Usuario usuario)
        {
            usu = usuario;
            InitializeComponent();
            DGVPagos.AutoGenerateColumns = false;
        }

        private void AddCodigo_Click(object sender, EventArgs e)
        {
            try
            {
                int largo = TxtCodigoBar.Text.Length;
                if (largo < 21)
                    throw new Exception("El codigo de barra debe contener al menos 21 digitos.");
                else
                {
                    int CodigoEmp = Convert.ToInt32(TxtCodigoBar.Text.Trim().Substring(0, 4));
                    int TipoCon = Convert.ToInt32(TxtCodigoBar.Text.Trim().Substring(4, 2));
                    DateTime FechaVen = new DateTime(Convert.ToInt32(TxtCodigoBar.Text.Trim().Substring(6, 4)),
                                                    Convert.ToInt32(TxtCodigoBar.Text.Trim().Substring(10, 2)),
                                                    Convert.ToInt32(TxtCodigoBar.Text.Trim().Substring(12, 2)));
                    if (FechaVen.CompareTo(DateTime.Today) < 0)
                        throw new Exception("No se pueden cobrar facturas vencidas.");

                    int Cli = Convert.ToInt32(TxtCodigoBar.Text.Trim().Substring(14, 6));
                    int Monto = 0;

                    if (largo == 21)
                        Monto = Convert.ToInt32(TxtCodigoBar.Text.Trim().Substring(20, 1));
                    else if (largo == 22)
                        Monto = Convert.ToInt32(TxtCodigoBar.Text.Trim().Substring(20, 2));
                    else if (largo == 23)
                        Monto = Convert.ToInt32(TxtCodigoBar.Text.Trim().Substring(20, 3));
                    else if (largo == 24)
                        Monto = Convert.ToInt32(TxtCodigoBar.Text.Trim().Substring(20, 4));
                    else if (largo == 25)
                        Monto = Convert.ToInt32(TxtCodigoBar.Text.Trim().Substring(20, 5));

                    Empresa emp = SServicio.BuscarEmpresa(CodigoEmp, usu);
                    if (emp == null)
                        throw new Exception("No se ha encontrado la Empresa");

                    Contrato contrato = SServicio.BuscarContrato(emp.Codigo, TipoCon, usu);
                    if (contrato == null)
                        throw new Exception("No se ha encontrado el contrato");

                    LineaPago lpago = new LineaPago();
                    lpago.Monto = Monto;
                    lpago.FechaVencimiento = FechaVen;
                    lpago.CodigoCliente = Cli;
                    lpago.Contrato = contrato;

                    lpagos.Add(lpago);

                    if (TxtTotal.Text == null || TxtTotal.Text.Equals(""))
                        TxtTotal.Text = Monto.ToString();
                    else
                    {
                        long montoTotal = Convert.ToInt64(TxtTotal.Text) + Monto;
                        TxtTotal.Text = montoTotal.ToString();
                    }
                }

                if(lpagos != null)
                {
                    DGVPagos.DataSource = null;
                    DGVPagos.DataSource = lpagos;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error"); }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                List<LineaPago> pagos = new List<LineaPago>();
                foreach (LineaPago item in DGVPagos.DataSource as List<LineaPago>)
                {
                    pagos.Add(item);
                }
                Pago p = new Pago();
                p.NumeroInterno = null;
                p.Fecha = DateTime.Today;
                p.Monto = Convert.ToInt32(TxtTotal.Text);
                p.Cajero = usu;
                p.LineasPago = pagos.ToArray();

                SServicio.AltaPago(p, usu);
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message, "Error"); }
            
            
        }
    }
}
