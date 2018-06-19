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
using Logica;

namespace BiosMoneyApp.GerenteApp
{
    public partial class ABMContrato : Form
    {
        FlowLayoutPanel panel;

        public ABMContrato()
        {
            InitializeComponent();
        }

        public ABMContrato(FlowLayoutPanel panel , Empresa empresa)
        {
            InitializeComponent();

            this.panel = panel;
            lblEmpresa.Text += empresa.Nombre;
        }

        private void BtnCambiarEmp_Click(object sender, EventArgs e)
        {
            Form cambiarEmp = new EmpresaSelection(panel);
            cambiarEmp.ShowDialog();
        }
    }
}
