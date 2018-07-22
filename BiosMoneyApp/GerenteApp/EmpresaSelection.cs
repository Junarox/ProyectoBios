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
    public partial class EmpresaSelection : Form
    {
        IMiServicio SServicio = new MiServicioClient();
        private Usuario usuario;
        FlowLayoutPanel panel;

        public EmpresaSelection(FlowLayoutPanel flowLayoutPanel1, Usuario usuario)
        {
            this.usuario = usuario;
            InitializeComponent();
            DGVEmpresas.AutoGenerateColumns = false;
            List<Empresa> empresas = SServicio.ListarEmpresa(usuario).ToList();
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
            ABMContrato myForm = new ABMContrato(panel, emp, usuario);
            myForm.FormBorderStyle = FormBorderStyle.None;
            myForm.TopLevel = false;
            myForm.AutoScroll = true;
            panel.Controls.Add(myForm);
            myForm.Show();
            this.Close();
        }

        private void DGVEmpresas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            BtnOK_Click(sender, e);
        }
    }
}
