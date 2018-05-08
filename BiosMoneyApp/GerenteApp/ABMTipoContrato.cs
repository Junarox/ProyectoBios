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
    public partial class ABMTipoContrato : Form
    {
        private bool cambio;
        private List<Contrato> conts = new List<Contrato>();
        private List<Empresa> emps = new List<Empresa>();

        public override void Refresh()
        {
            try
            {
                DGVEmpresas.DataSource = null;
                DGVEmpresas.DataSource = emps = FabricaL.GetEmpresa().ListarEmpresa();

                DGVContratos.DataSource = null;
                if(emps.Count() != 0)
                {
                    //Por defecto el DataGrid setea DGV.Rows[0].Selected en true, que es lo mismo
                    //que el primer elemento de la lista.
                    DGVContratos.DataSource = conts = FabricaL.GetContrato().ListarContrato(emps[0]);
                }
            }
            catch(Exception ex)
            { lblError.Text = ex.Message; }
      }

        public ABMTipoContrato()
        {
            InitializeComponent();

            DGVEmpresas.AutoGenerateColumns = false;
            DGVContratos.AutoGenerateColumns = false;

            Refresh();
        }

        private void DGVEmpresas_SelectionChanged(object sender, EventArgs e)
        {
            Empresa emp = (Empresa)DGVEmpresas.CurrentRow.DataBoundItem;

            DGVContratos.DataSource = null;
            DGVContratos.DataSource = conts = FabricaL.GetContrato().ListarContrato(emp);
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            Contrato c = new Contrato();
            try
            {
                //Seteo sus datos.
                try
                {
                    c.CodContrato = Convert.ToInt32(txtCodigoC.Text);
                }
                catch (FormatException) { throw new FormatException("El Código debe contener 4 digitos."); }
                catch (OverflowException) { throw new OverflowException("El Código debe contener 4 digitos."); }

                c.Empresa = (Empresa)DGVEmpresas.CurrentRow.DataBoundItem;
                c.NomContrato = txtNombreC.Text;

                //Llamo a la fabrica para darlo de alta.
                FabricaL.GetContrato().AltaContrato(c);

                //Refresco el DataGrid con los nuevos datos.
                Refresh();
            }
            catch (Exception ex) { lblError.Text = ex.Message; }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Contrato c = new Contrato();

            //Instancio un Cajero y lo seteo con el objeto Cajero de la fila seleccionada del DataGrid.
            try
            {
                try
                {
                    c = (Contrato)DGVContratos.CurrentRow.DataBoundItem;
                }
                catch (NullReferenceException) { throw new NullReferenceException("No se ha seleccionado ningún Contrato."); }

                //Modifico sus atributos.
                c.NomContrato = txtNombreC.Text;

                //LLamo a la fabrica para modificarlo en la DB.
                FabricaL.GetContrato().ModContrato(c);

                //Refresco el DataGrid con los nuevos datos.
                Refresh();
            }
            catch (Exception ex) { lblError.Text = ex.Message; }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                //Llamo a la fabrica y doy de baja al Contrato de la fila seleccionada.
                try
                {
                    FabricaL.GetContrato().BajaContrato((Contrato)DGVContratos.CurrentRow.DataBoundItem);
                }
                catch (NullReferenceException) { throw new NullReferenceException("No se ha selecionado ningún Contrato."); }

                //Vacío y seteo el DataGrid con los nuevos valores.
                Refresh();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        private void txtBuscarEmp_TextChanged(object sender, EventArgs e)
        {
            Refresh();

            List<Empresa> resultado = new List<Empresa>();

            if (DGVEmpresas.DataSource != null)
            {
                long i = 0;

                if (Int64.TryParse(txtBuscarEmp.Text, out i))
                {

                    resultado = (from Empresa emp in (List<Empresa>)DGVEmpresas.DataSource
                                 where emp.Codigo.ToString().StartsWith(txtBuscarEmp.Text) || emp.Rut.ToString().StartsWith(txtBuscarEmp.Text)
                                 select emp).ToList();

                    DGVEmpresas.DataSource = null;
                    DGVEmpresas.DataSource = resultado;
                }
                else if (txtBuscarEmp.Text.Trim() != "")
                {
                    resultado = (from Empresa emp in (List<Empresa>)DGVEmpresas.DataSource
                                 where (emp.Nombre.ToLower().StartsWith(txtBuscarEmp.Text.ToLower()))
                                 select emp).ToList();

                    DGVEmpresas.DataSource = null;
                    DGVEmpresas.DataSource = resultado;
                }

                else
                    Refresh();
            }
        }

        private void txtBuscarCon_TextChanged(object sender, EventArgs e)
        {
            Refresh();
            List<Contrato> resultado = new List<Contrato>();

            if (DGVContratos.DataSource != null)
            {
                int i = 0;

                if (int.TryParse(txtBuscarCon.Text, out i))
                {
                    resultado = (from Contrato con in (List<Contrato>)DGVContratos.DataSource
                                 where con.CodContrato.ToString().StartsWith(txtBuscarCon.Text)
                                 select con).ToList();

                    DGVContratos.DataSource = null;
                    DGVContratos.DataSource = resultado;
                }
                else if (txtBuscarCon.Text.Trim() != "")
                {
                    resultado = (from Contrato con in (List<Contrato>)DGVContratos.DataSource
                                 where (con.NomContrato.ToLower().StartsWith(txtBuscarCon.Text.ToLower()))
                                 select con).ToList();

                    DGVContratos.DataSource = null;
                    DGVContratos.DataSource = resultado;
                }

                else
                    Refresh();
            }
        }

        private void DGVGerentes_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                List<Contrato> cs = new List<Contrato>();
                //Traigo la lista del DataGrid (Puede usarse con el filtrado en conjunto).
                try
                {
                    if (DGVContratos.DataSource == null)
                        throw new NullReferenceException();
                    cs = (List<Contrato>)DGVContratos.DataSource;
                }
                catch (NullReferenceException) { throw new NullReferenceException("No existe ningún Contrato."); }

                //Obtengo la columna a la que se le hizo click.
                //DataPropertyName: devuelve el valor en string que utiliza el DataGrid para referenciarse con la variable del Objeto.
                string headerText = DGVContratos.Columns[e.ColumnIndex].DataPropertyName;

                //Este es el switch que utiliza el bool cambio del principio de la clase.
                //El bool se encuentra fuera del evento porque sino cada vez que este se llama, va a venir en su valor por defecto.
                switch (cambio)
                {
                    //Por defecto es falso, entonces ordeno la lista de A-Z
                    case false:
                        //Dependiendo del valor de la columna a la que se le hizo click, ordeno en base a eso.
                        switch (headerText)
                        {
                            case "CodContrato":
                                //Realizo el ordenamiento en base al Nombre Completo
                                cs = cs.OrderBy(o => o.CodContrato).ToList();
                                break;

                            case "NomContrato":
                                //Realizo el ordenamiento en base al Usuario
                                cs = cs.OrderBy(o => o.NomContrato).ToList();
                                break;
                        }

                        //Vacío el DataGrid y lo seteo con la nueva lsita ordenada.
                        DGVContratos.DataSource = null;
                        DGVContratos.DataSource = cs;

                        //Seteo el ordenamiento en el valor contrario, asi la proxima vez que se llame a este evento, en vez de ordenar de A-Z
                        //ordenara la lista de Z-A
                        cambio = true;
                        break;

                    //Lo mismo que arriba pero de Z-A
                    case true:
                        switch (headerText)
                        {
                            case "CodContrato":
                                cs = cs.OrderByDescending(o => o.CodContrato).ToList();
                                break;

                            case "NomContrato":
                                cs = cs.OrderByDescending(o => o.NomContrato).ToList();
                                break;
                        }
                        DGVContratos.DataSource = null;
                        DGVContratos.DataSource = cs;
                        cambio = false;
                        break;
                }
            }

            catch (Exception ex) { lblError.Text = ex.Message; }
        }

        private void DGVEmpresa_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                List<Empresa> emps = new List<Empresa>();
                //Traigo la lista del DataGrid (Puede usarse con el filtrado en conjunto).
                try
                {
                    if (DGVEmpresas.DataSource == null)
                        throw new NullReferenceException();

                    emps = (List<Empresa>)DGVEmpresas.DataSource;
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
                                emps = emps.OrderBy(o => o.Rut).ToList();
                                break;

                            case "Codigo":
                                //Realizo el ordenamiento en base al Usuario
                                emps = emps.OrderBy(o => o.Codigo).ToList();
                                break;

                            case "Nombre":
                                //Realizo el ordenamiento en base a la Cédula
                                emps = emps.OrderBy(o => o.Nombre).ToList();
                                break;
                        }

                        //Vacío el DataGrid y lo seteo con la nueva lsita ordenada.
                        DGVEmpresas.DataSource = null;
                        DGVEmpresas.DataSource = emps;

                        //Seteo el ordenamiento en el valor contrario, asi la proxima vez que se llame a este evento, en vez de ordenar de A-Z
                        //ordenara la lista de Z-A
                        cambio = true;
                        break;

                    //Lo mismo que arriba pero de Z-A
                    case true:
                        switch (headerText)
                        {
                            case "Rut":
                                emps = emps.OrderByDescending(o => o.Rut).ToList();
                                break;

                            case "Codigo":
                                emps = emps.OrderByDescending(o => o.Codigo).ToList();
                                break;

                            case "Nombre":
                                emps = emps.OrderByDescending(o => o.Nombre).ToList();
                                break;
                        }
                        DGVEmpresas.DataSource = null;
                        DGVEmpresas.DataSource = emps;
                        cambio = false;
                        break;
                }
            }
            catch (Exception ex) { lblError.Text = ex.Message; }
        }
    }
}
