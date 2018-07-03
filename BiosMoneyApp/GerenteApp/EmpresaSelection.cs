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
using EntidadesCompartidas;

namespace BiosMoneyApp.GerenteApp
{
    public partial class EmpresaSelection : Form
    {
        private Usuario usuario;
        FlowLayoutPanel panel;

        public EmpresaSelection(FlowLayoutPanel flowLayoutPanel1, Usuario usuario)
        {
            this.usuario = usuario;
            InitializeComponent();
            List<Empresa> empresas = FabricaL.GetEmpresa().ListarEmpresa(usuario);
            DGVEmpresas.DataSource = empresas;
            panel = flowLayoutPanel1;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            Empresa emp = (Empresa)DGVEmpresas.CurrentRow.DataBoundItem;

            panel.Controls.Clear();
            ABMContrato myForm = new ABMContrato(panel, emp);
            myForm.FormBorderStyle = FormBorderStyle.None;
            myForm.TopLevel = false;
            myForm.AutoScroll = true;
            panel.Controls.Add(myForm);
            myForm.Show();
            this.Close();
        }
    }
}
