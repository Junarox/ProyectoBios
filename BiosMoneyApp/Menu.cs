using BiosMoneyApp.CajeroApp;
using BiosMoneyApp.GerenteApp;
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

namespace BiosMoneyApp
{
    public partial class Menu : Form
    {
        private Usuario usu;

        public Menu(Usuario usu)
        {
            InitializeComponent();
            this.usu = usu;
            CheckUsu(usu);
        }

        private void ABMCajero_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            ABMCajero myForm = new ABMCajero();
            myForm.FormBorderStyle = FormBorderStyle.None;
            myForm.TopLevel = false;
            myForm.AutoScroll = true;
            flowLayoutPanel1.Controls.Add(myForm);
            myForm.Show();
        }

        private void ABMEmpresa_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            ABMEmpresa myForm = new ABMEmpresa();
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
            usu = null;
        }

        private void MenuGerente_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void TSMISalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void TSMICambiarClave_Click(object sender, EventArgs e)
        {
            CambiarClave form = new CambiarClave(usu);
            form.ShowDialog();
        }

        private void TSMIAltaGerente_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            AltaGerente myForm = new AltaGerente();
            myForm.FormBorderStyle = FormBorderStyle.None;
            myForm.TopLevel = false;
            myForm.AutoScroll = true;
            flowLayoutPanel1.Controls.Add(myForm);
            myForm.Show();
        }

        private void TSMIABMContrato_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            ABMTipoContrato myForm = new ABMTipoContrato();
            myForm.FormBorderStyle = FormBorderStyle.None;
            myForm.TopLevel = false;
            myForm.AutoScroll = true;
            flowLayoutPanel1.Controls.Add(myForm);
            myForm.Show();
        }

        private void TSMIAltaPago_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            AltaDePago myForm = new AltaDePago();
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
