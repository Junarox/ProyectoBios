using BiosMoneyApp.CajeroApp;
using BiosMoneyApp.GerenteApp;
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
namespace BiosMoneyApp
{
    public partial class Menu : Form
    {
        private Usuario usuario;

        public Menu(Usuario usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
            CheckUsu(usuario);
        }

        private void ABMCajero_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            ABMCajero myForm = new ABMCajero(usuario);
            myForm.FormBorderStyle = FormBorderStyle.None;
            myForm.TopLevel = false;
            myForm.AutoScroll = true;
            flowLayoutPanel1.Controls.Add(myForm);
            myForm.Show();
        }

        private void ABMEmpresa_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            ABMEmpresa myForm = new ABMEmpresa(usuario);
            myForm.FormBorderStyle = FormBorderStyle.None;
            myForm.TopLevel = false;
            myForm.AutoScroll = true;
            flowLayoutPanel1.Controls.Add(myForm);
            myForm.Show();
        }

        private void TSMICambiarUsuario_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login form = new Login();
            form.ShowDialog();
            usuario = null;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (PreClosingConfirmation() == System.Windows.Forms.DialogResult.Yes)
            {
                Dispose(true);
                Application.Exit();
            }
            else
            {
                e.Cancel = true;
            }
        }
        private DialogResult PreClosingConfirmation()
        {
            DialogResult res = System.Windows.Forms.MessageBox.Show("Está seguro que desea salir?", "Salir", MessageBoxButtons.YesNo);
            return res;
        }

        private void TSMISalir_Click(object sender, EventArgs e)
        {
            if (PreClosingConfirmation() == System.Windows.Forms.DialogResult.Yes)
            {
                Dispose(true);
                Application.Exit();
            }
            else
            {
                return;
            }
        }

        private void TSMICambiarClave_Click(object sender, EventArgs e)
        {
            CambiarClave form = new CambiarClave(usuario);
            form.ShowDialog();
        }

        private void TSMIAltaGerente_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            AltaGerente myForm = new AltaGerente(usuario);
            myForm.FormBorderStyle = FormBorderStyle.None;
            myForm.TopLevel = false;
            myForm.AutoScroll = true;
            flowLayoutPanel1.Controls.Add(myForm);
            myForm.Show();
        }

        private void TSMIABMContrato_Click(object sender, EventArgs e)
        {
            Form f = new EmpresaSelection(flowLayoutPanel1, usuario);
            f.ShowDialog();
        }

        private void TSMIAltaPago_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            AltaDePago myForm = new AltaDePago(usuario);
            myForm.FormBorderStyle = FormBorderStyle.None;
            myForm.TopLevel = false;
            myForm.AutoScroll = true;
            flowLayoutPanel1.Controls.Add(myForm);
            myForm.Show();
        }

        private void CheckUsu(Usuario usu)
        {
            if (usu is Gerente)
            {
                TSMIABMCajero.Visible = true;
                TSMIABMCajero.Enabled = true;

                TSMIABMEmpresa.Visible = true;
                TSMIABMEmpresa.Enabled = true;

                TSMIABMContrato.Visible = true;
                TSMIABMContrato.Enabled = true;

                TSMIAltaGerente.Visible = true;
                TSMIAltaGerente.Enabled = true;

                TSMIAltaPago.Visible = false;
                TSMIAltaPago.Enabled = false;
            }

            else
            {
                TSMIABMCajero.Visible = false;
                TSMIABMCajero.Enabled = false;

                TSMIABMEmpresa.Visible = false;
                TSMIABMEmpresa.Enabled = false;

                TSMIABMContrato.Visible = false;
                TSMIABMContrato.Enabled = false;

                TSMIAltaGerente.Visible = false;
                TSMIAltaGerente.Enabled = false;

                TSMIAltaPago.Visible = true;
                TSMIAltaPago.Enabled = true;
            }
        }
    }
}
