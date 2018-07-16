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
        private Usuario usuario;
        FlowLayoutPanel panel;

        private Empresa emp;
        private List<Contrato> cs = new List<Contrato>();

        public ABMContrato(FlowLayoutPanel panel , Empresa empresa, Usuario usuario)
        {
            this.usuario = usuario;
            InitializeComponent();
            DGVContrato.AutoGenerateColumns = false;
            CargarContratos(empresa);
            this.panel = panel;
            emp = empresa;
            lblEmpresa.Text += emp.Nombre;
        }

        private void CargarContratos(Empresa empresa)
        {
            DGVContrato.DataSource = FabricaL.GetContrato().ListarContrato(empresa, usuario);   
        }

        private void BtnCambiarEmp_Click(object sender, EventArgs e)
        {
            Form cambiarEmp = new EmpresaSelection(panel, usuario);
            cambiarEmp.ShowDialog();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                Contrato contrato = new Contrato(emp, Convert.ToInt32(txtCodigo.Text), txtNombre.Text);
                FabricaL.GetContrato().AltaContrato(contrato, usuario);
                Refresh();
            }
            catch(Exception ex) { MessageBox.Show(ex.Message, "Error"); }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            try
            {


                Refresh();

                List<Contrato> resultado = new List<Contrato>();

                if (DGVContrato.DataSource != null)
                {
                    int i = 0;

                    if (int.TryParse(txtBuscar.Text, out i))
                    {
                        resultado = (from Contrato c in (List<Contrato>)DGVContrato.DataSource
                                     where c.CodContrato.ToString().StartsWith(txtBuscar.Text)
                                     select c).ToList();

                        DGVContrato.DataSource = null;
                        DGVContrato.DataSource = resultado;
                    }

                    else if (txtBuscar.Text.Trim() != "")
                    {
                        resultado = (from Contrato c in (List<Contrato>)DGVContrato.DataSource
                                     where (c.NomContrato.ToLower().StartsWith(txtBuscar.Text.ToLower()))
                                     select c).ToList();

                        DGVContrato.DataSource = null;
                        DGVContrato.DataSource = resultado;
                    }

                    else
                        Refresh();
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message, "Error"); }
        }

        public override void Refresh()
        {
            DGVContrato.DataSource = null;
            DGVContrato.DataSource = cs = FabricaL.GetContrato().ListarContrato(emp, usuario);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                Contrato contrato = (Contrato)DGVContrato.CurrentRow.DataBoundItem;

                FabricaL.GetContrato().BajaContrato(contrato, usuario);
                Refresh();
            }
            catch(Exception ex) { MessageBox.Show(ex.Message, "Error"); }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                Contrato contrato = new Contrato();
                try
                {
                    contrato = (Contrato)DGVContrato.CurrentRow.DataBoundItem;
                }
                catch (NullReferenceException) { throw new NullReferenceException("No se ha seleccionado ninguna Empresa."); }

                try
                {
                    contrato.CodContrato = Convert.ToInt32(txtCodigo.Text);
                }
                catch (FormatException) { throw new FormatException("El Codigo del Contrato debe contener 2 digitos."); }

                contrato.NomContrato = txtNombre.Text;

                FabricaL.GetContrato().ModContrato(contrato, usuario);

                Refresh();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error"); }
        }

        private void DGVContrato_SelectionChanged(object sender, EventArgs e)
        {
            Contrato contrato = (Contrato)DGVContrato.CurrentRow.DataBoundItem;

            txtCodigo.Text = contrato.CodContrato.ToString();
            txtNombre.Text = contrato.NomContrato;
        }
    }
}
