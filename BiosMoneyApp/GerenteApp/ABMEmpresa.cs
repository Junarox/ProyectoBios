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
    public partial class ABMEmpresa : Form
    {
        private Usuario usuario;
        private bool cambio;
        private List<Empresa> emps = new List<Empresa>();

        public override void Refresh()
        {
            DGVEmpresas.DataSource = null;
            DGVEmpresas.DataSource = emps = FabricaL.GetEmpresa().ListarEmpresa(usuario);
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
                
                FabricaL.GetEmpresa().AltaEmpresa(emp, usuario);

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

                FabricaL.GetEmpresa().ModEmpresa(emp, usuario);

                Refresh();
            }
            catch (Exception ex) { lblError.Text = ex.Message; }
            
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    FabricaL.GetEmpresa().BajaEmpresa((Empresa)DGVEmpresas.CurrentRow.DataBoundItem, usuario);
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

        private void DGVEmpresa_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                List<Empresa> cs = new List<Empresa>();
                //Traigo la lista del DataGrid (Puede usarse con el filtrado en conjunto).
                try
                {
                    if (DGVEmpresas.DataSource == null)
                        throw new NullReferenceException();

                    cs = (List<Empresa>)DGVEmpresas.DataSource;
                }
                catch (NullReferenceException) { throw new NullReferenceException("No se ha seleccionado ninguna Empresa."); }

                //Obtengo la columna a la que se le hizo click.
                //DataPropertyName: devuelve el valor en string que utiliza el DataGrid para referenciarse con la variable del Objeto.
                string headerText = DGVEmpresas.Columns[e.ColumnIndex].DataPropertyName;

                //Este es el switch que utiliza el bool cambio del principio de la clase.
                //El bool se encuentra fuera del evento porque sino cada vez que este se llama, va a venir en su valor por defecto.
                switch (cambio)
                {
                    //Por defecto es falso, entonces ordeno la lista de A-Z
                    case false:
                        //Dependiendo del valor de la columna a la que se le hizo click, ordeno en base a eso.
                        switch (headerText)
                        {
                            case "Rut":
                                //Realizo el ordenamiento en base al Nombre Completo
                                cs = cs.OrderBy(o => o.Rut).ToList();
                                break;

                            case "Codigo":
                                //Realizo el ordenamiento en base al Usuario
                                cs = cs.OrderBy(o => o.Codigo).ToList();
                                break;

                            case "Nombre":
                                //Realizo el ordenamiento en base a la Cédula
                                cs = cs.OrderBy(o => o.Nombre).ToList();
                                break;
                        }

                        //Vacío el DataGrid y lo seteo con la nueva lsita ordenada.
                        DGVEmpresas.DataSource = null;
                        DGVEmpresas.DataSource = cs;

                        //Seteo el ordenamiento en el valor contrario, asi la proxima vez que se llame a este evento, en vez de ordenar de A-Z
                        //ordenara la lista de Z-A
                        cambio = true;
                        break;

                    //Lo mismo que arriba pero de Z-A
                    case true:
                        switch (headerText)
                        {
                            case "Rut":
                                cs = cs.OrderByDescending(o => o.Rut).ToList();
                                break;

                            case "Codigo":
                                cs = cs.OrderByDescending(o => o.Codigo).ToList();
                                break;

                            case "Nombre":
                                cs = cs.OrderByDescending(o => o.Nombre).ToList();
                                break;
                        }
                        DGVEmpresas.DataSource = null;
                        DGVEmpresas.DataSource = cs;
                        cambio = false;
                        break;
                }
            }
            catch(Exception ex) { lblError.Text = ex.Message; }
        }
    }
}
