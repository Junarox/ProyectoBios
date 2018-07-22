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
    public partial class ABMEmpresa : Form
    {
        IMiServicio SServicio = new MiServicioClient();
        private Usuario usuario;
        private List<Empresa> emps = new List<Empresa>();

        public override void Refresh()
        {
            DGVEmpresas.DataSource = null;
            DGVEmpresas.DataSource = emps = SServicio.ListarEmpresa(usuario).ToList();
        }

        public ABMEmpresa(Usuario usuario)
        {
            this.usuario = usuario;
            InitializeComponent();

            DGVEmpresas.AutoGenerateColumns = false;

            Refresh();
        }

        private void DGVEmpresas_SelectionChanged(object sender, EventArgs e)
        {
            Empresa emp = (Empresa)DGVEmpresas.CurrentRow.DataBoundItem;
            txtRut.Text = emp.Rut.ToString();
            txtCodigo.Text = emp.Codigo.ToString();
            txtNombre.Text = emp.Nombre;
            txtDirF.Text = emp.DirFiscal;
            txtTel.Text = emp.Tel.ToString();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            Empresa emp = new Empresa();
            try
            {
                try
                {
                    emp.Codigo = Convert.ToInt32(txtCodigo.Text);
                }
                catch (NullReferenceException) { throw new NullReferenceException("El campo Código no puede estar vacío."); }
                catch (FormatException) { throw new FormatException("El campo Código debe contener 4 dígitos."); }

                try
                {
                    emp.Rut = Convert.ToInt64(txtRut.Text);
                }
                catch (NullReferenceException) { throw new NullReferenceException("El campo Rut no puede estar vacío."); }
                catch (FormatException) { throw new FormatException("El campo Rut debe contener 12 dígitos."); }

                emp.Nombre = txtNombre.Text;
                emp.DirFiscal = txtDirF.Text;
                try
                {
                    emp.Tel = Convert.ToInt64(txtTel.Text);
                }
                catch (NullReferenceException) { throw new NullReferenceException("El campo Teléfono no puede estar vacío."); }
                catch (FormatException) { throw new FormatException("El campo Teléfono debe contener solo dígitos."); }

                SServicio.AltaEmpresa(emp, usuario);

                Refresh();
            }

            catch (Exception ex) { lblError.Text = ex.Message; }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                Empresa emp = new Empresa();
                try
                {
                    emp = (Empresa)DGVEmpresas.CurrentRow.DataBoundItem;
                }
                catch (NullReferenceException) { throw new NullReferenceException("No se ha seleccionado ninguna Empresa."); }

                try
                {
                    emp.Rut = Convert.ToInt64(txtRut.Text);
                }
                catch (FormatException) { throw new FormatException("El Rut debe contener 12 digitos."); }

                emp.Nombre = txtNombre.Text;
                emp.DirFiscal = txtDirF.Text;
                emp.Tel = Convert.ToInt32(txtTel.Text);

                SServicio.ModEmpresa(emp, usuario);

                Refresh();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error"); }

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    SServicio.BajaEmpresa((Empresa)DGVEmpresas.CurrentRow.DataBoundItem, usuario);
                }
                catch (NullReferenceException) { lblError.Text = "No se ha seleccionado ninguna Empresa."; }

                Refresh();
            }
            catch (Exception ex) { lblError.Text = ex.Message; }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            Refresh();

            List<Empresa> resultado = new List<Empresa>();

            if (DGVEmpresas.DataSource != null)
            {
                long i = 0;

                if (Int64.TryParse(txtBuscar.Text, out i))
                {

                    resultado = (from Empresa emp in (List<Empresa>)DGVEmpresas.DataSource
                                 where emp.Codigo.ToString().StartsWith(txtBuscar.Text) || emp.Rut.ToString().StartsWith(txtBuscar.Text)
                                 select emp).ToList();

                    DGVEmpresas.DataSource = null;
                    DGVEmpresas.DataSource = resultado;
                }
                else if (txtBuscar.Text.Trim() != "")
                {
                    resultado = (from Empresa emp in (List<Empresa>)DGVEmpresas.DataSource
                                 where (emp.Nombre.ToLower().StartsWith(txtBuscar.Text.ToLower()))
                                 select emp).ToList();

                    DGVEmpresas.DataSource = null;
                    DGVEmpresas.DataSource = resultado;
                }

                else
                    Refresh();
            }
        }
    }
}
