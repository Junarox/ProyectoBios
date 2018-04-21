using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntidadesCompartidas;

namespace BiosMoneyApp.GerenteApp
{
    public partial class MenuGerente : Form
    {
        private Usuario usu;

        public MenuGerente(Usuario usu)
        {
            InitializeComponent();
            this.usu = usu;
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

        private void aBMContratoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            ABMTipoContrato myForm = new ABMTipoContrato();
            myForm.FormBorderStyle = FormBorderStyle.None;
            myForm.TopLevel = false;
            myForm.AutoScroll = true;
            flowLayoutPanel1.Controls.Add(myForm);
            myForm.Show();
        }
    }
}
