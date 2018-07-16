using EntidadesCompartidas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Logica;
namespace BiosMoneyApp.CajeroApp
{
    public partial class AltaDePago : Form
    {
        Usuario usu;

        private List<LineaPago> lpagos = new List<LineaPago>();

        public AltaDePago(Usuario usuario)
        {
            usu = usuario;
            InitializeComponent();
        }

        private void AddCodigo_Click(object sender, EventArgs e)
        {
            try
            {
                int CodigoEmp = Convert.ToInt32(TxtCodigoBar.Text.Trim().Substring(0, 4));
                int TipoCon = Convert.ToInt32(TxtCodigoBar.Text.Trim().Substring(5, 2));
                DateTime FechaVen = new DateTime(Convert.ToInt32(TxtCodigoBar.Text.Trim().Substring(8, 4)), 
                                                Convert.ToInt32(TxtCodigoBar.Text.Trim().Substring(13, 2)), 
                                                Convert.ToInt32(TxtCodigoBar.Text.Trim().Substring(16, 2)));
                int Cli = Convert.ToInt32(TxtCodigoBar.Text.Trim().Substring(19, 6));
                int Monto = Convert.ToInt32(TxtCodigoBar.Text.Trim().Substring(26, 5));

                Empresa emp = FabricaL.GetEmpresa().BuscarEmpresa(CodigoEmp, usu);
                if (emp == null)
                    throw new Exception("No se ha encontrado la Empresa");

                Contrato contrato = FabricaL.GetContrato().BuscarContrato(emp.Codigo, TipoCon, usu);
                if (contrato == null)
                    throw new Exception("No se ha encontrado el contrato");
                LineaPago lpago = new LineaPago(Monto, FechaVen, Cli, contrato);


                lpagos.Add(lpago);

                DGVPagos.DataSource = lpagos;

                TxtTotal.Text += Monto;
            }catch(Exception ex) { MessageBox.Show(ex.Message, "Error"); }
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
                Pago p = new Pago(null, DateTime.Today, Convert.ToInt32(TxtTotal.Text), usu, pagos);
                FabricaL.GetPago().AltaPago(p, usu);
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message, "Error"); }
            
            
        }
    }
}
