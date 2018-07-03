using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EntidadesCompartidas;
using Logica;

namespace BiosMoneyApp.GerenteApp
{
    public partial class ABMCajero : Form
    {
        Usuario usuario;

        //Esto determina como se va a ordenar la lista del DataGrid
        private bool cambio;

        //Instancio la lista
        private List<Cajero> cs = new List<Cajero>();

        /// <summary>
        /// Ya sea un alta, modificar o eliminar, traigo la lista con los nuevos cambios de la DB
        /// </summary>
        public override void Refresh()
        {
            DGVCajeros.DataSource = null;
            DGVCajeros.DataSource = cs = FabricaL.GetLUsuario().ListarCajeros(usuario);
        }

        /// <summary>
        /// Esto sucede la primera vez que se abre el Form.
        /// </summary>
        public ABMCajero(Usuario usuario)
        {
            this.usuario = usuario;

            //Se inicializa la UI.
            InitializeComponent();
            //Si no hago esto, el DataGrid me muestra todos los datos del Usuario.
            DGVCajeros.AutoGenerateColumns = false;

            //Seteo los ComboBox con el primer valor posible para que no se inicializen en null.
            CBHIHH.SelectedIndex = 0;
            CBHIMM.SelectedIndex = 0;
            CBHFHH.SelectedIndex = 0;
            CBHFMM.SelectedIndex = 0;

            //Traigo la lista de cajeros y la guardo en el GridView
            Refresh();
        }

        /// <summary>
        /// Capturo la fila a la que se le dio click, y muestro los datos del Usuario en esa fila en los TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGVCajeros_SelectionChanged(object sender, EventArgs e)
        {
            //Obtengo el usuario de la fila seleccionada.
            var usu = DGVCajeros.CurrentRow.DataBoundItem as Cajero;

            //Seteo los datos del usuario en los TextBox
            txtUsuario.Text = usu.Usu;
            txtCedula.Text = usu.Ci.ToString();
            txtNombreC.Text = usu.NomCompleto;

            //Seteo la hora de inicio y fin
            CBHIHH.SelectedItem = usu.HoraInicio.ToString().Substring(0, 2);
            CBHFHH.SelectedItem = usu.HoraFin.ToString().Substring(0, 2);

            //Seteo los minutos de inicio fin
            string c = usu.HoraInicio.ToString().Substring(2, 2);
            CBHIMM.SelectedItem = usu.HoraInicio.ToString().Substring(2, 2);
            CBHFMM.SelectedItem = usu.HoraFin.ToString().Substring(2, 2);

        }

        /// <summary>
        /// Agrega un Usuario Cajero a la DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            //Creo el Cajero.
            Cajero c = new Cajero();
            try
            {
                //Seteo sus datos.
                try
                {
                    c.Ci = Convert.ToInt32(txtCedula.Text);
                    
                }
                catch (FormatException)
                {
                    MessageBox.Show("La Cédula debe contener 8 dígitos.", "Campo Inválido");
                    return;
                }
                catch (OverflowException) { MessageBox.Show("La Cédula debe contener 8 dígitos.", "Campo Inválido"); }
                c.Usu = txtUsuario.Text;
                c.NomCompleto = txtNombreC.Text;
                c.HoraInicio = CBHIHH.SelectedItem.ToString() + CBHIMM.SelectedItem.ToString();
                c.HoraFin = CBHFHH.SelectedItem.ToString() + CBHFMM.SelectedItem.ToString();

                //Llamo a la fabrica para darlo de alta.
                FabricaL.GetLUsuario().Alta(c, usuario);

                //Refresco el DataGrid con los nuevos datos.
                Refresh();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message,"Error"); }
        }

        /// <summary>
        /// Dado un cajero seleccionado, lo modifico.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModificar_Click(object sender, EventArgs e)
        {
            Cajero c = new Cajero();

            //Instancio un Cajero y lo seteo con el objeto Cajero de la fila seleccionada del DataGrid.
            try
            {
                try
                {
                    c = (Cajero)DGVCajeros.CurrentRow.DataBoundItem;
                }
                catch (NullReferenceException) { MessageBox.Show("No se ha seleccionado ningún Cajero.", "Error"); }

                //Modifico sus atributos.
                c.NomCompleto = txtNombreC.Text;
                c.Usu = txtUsuario.Text;
                c.HoraInicio = CBHIHH.SelectedItem.ToString() + CBHIMM.SelectedItem.ToString();
                c.HoraFin = CBHFHH.SelectedItem.ToString() + CBHFMM.SelectedItem.ToString();

                //LLamo a la fabrica para modificarlo en la DB.
                FabricaL.GetLUsuario().Modificar(c, usuario);

                //Refresco el DataGrid con los nuevos datos.
                Refresh();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error"); }
        }

        /// <summary>
        /// Filtro la lista en tiempo de ejecución por los distintos valores ingresados en el TextBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            //Refresco el DataGrid con los nuevos datos.
            Refresh();

            //Creo la nueva lista de Usuarios resultante del filtrado.
            var resultado = DGVCajeros.DataSource as List<Cajero>;

            //Este if es porque la primera vez que se inicia la aplicación, cuando se cargan los controles se llama al TextChanged 
            //y como el DataGrid está aún en null, se lanza la exception NullReferenceException
            if(DGVCajeros.DataSource != null)
            {
                //Variable para ver si un txt se puede transformar a int.
                int i = 0;

                //Si lo que ingreso en el texto se puede transformar en un int, es porque estoy intentando filtrar por una cedula.
                if (int.TryParse(txtBuscar.Text, out i))
                {
                    //Seteo el DataGrid con los nuevos valores.
                    DGVCajeros.DataSource = null;
                    DGVCajeros.DataSource = resultado.Find(x => x.Ci.ToString().StartsWith(txtBuscar.Text));
                }

                //Si el texto del txtBuscar es distinto de "" y no se puede convertir en int por el caso anterior, es porque estoy filtrando
                //por Nombre o Usuario.
                else if (txtBuscar.Text.Trim() != "")
                {
                    //Realizo el filtro mediante LinQ
                    //Aclaración: en el where, comparo el texto con los dos atributos y traigo los valores que coincidan, independientemente
                    DGVCajeros.DataSource = null;
                    DGVCajeros.DataSource = resultado.Find(c => c.NomCompleto.ToLower().StartsWith(txtBuscar.Text.ToLower()) || c.Usu.ToLower().StartsWith(txtBuscar.Text.ToLower()));
                }

                else
                    //En este caso el txtBuscar está en "", quiere decir que no se está realizando ningun filtrado, por lo que cargo el DataGrid
                    //con todos los datos.
                    //Aclaración: No uso la misma lista para cargarlo porque si en el filtrado modifiqué un Usuario, si no llamo al Refresh()
                    //no me va a traer las modificaciones.
                    Refresh();
            }
        }

        /// <summary>
        /// Dada una fila seleccionada del DataGrid, borro el Usuario de esa fila
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                //Llamo a la fabrica y doy de baja al Usuario de la fila seleccionada.
                try
                {
                    FabricaL.GetLUsuario().BajaCajero((Cajero)DGVCajeros.CurrentRow.DataBoundItem, usuario);
                }
                catch (NullReferenceException) { MessageBox.Show("No se ha selecionado ningún Cajero.", "Error"); }

                //Vacío y seteo el DataGrid con los nuevos valores.
                Refresh();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Error");
            }
        }

        /// <summary>
        /// Ordeno los datos del DataGrid dependiendo de la columna seleccionada.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGVCajeros_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                //Traigo la lista del DataGrid (Puede usarse con el filtrado en conjunto).
                try
                {
                    if (DGVCajeros.DataSource == null)
                        throw new NullReferenceException();
                    List<Cajero> cs = (List<Cajero>)DGVCajeros.DataSource;
                }
                catch (NullReferenceException) { throw new NullReferenceException("No existe ningún Cajero."); }

                //Obtengo la columna a la que se le hizo click.
                //DataPropertyName: devuelve el valor en string que utiliza el DataGrid para referenciarse con la variable del Objeto.
                string headerText = DGVCajeros.Columns[e.ColumnIndex].DataPropertyName;

                //Este es el switch que utiliza el bool cambio del principio de la clase.
                //El bool se encuentra fuera del evento porque sino cada vez que este se llama, va a venir en su valor por defecto.
                switch (cambio)
                {
                    //Por defecto es falso, entonces ordeno la lista de A-Z
                    case false:
                        //Dependiendo del valor de la columna a la que se le hizo click, ordeno en base a eso.
                        switch (headerText)
                        {
                            case "NomCompleto":
                                //Realizo el ordenamiento en base al Nombre Completo
                                cs = cs.OrderBy(o => o.NomCompleto).ToList();
                                break;

                            case "Usu":
                                //Realizo el ordenamiento en base al Usuario
                                cs = cs.OrderBy(o => o.Usu).ToList();
                                break;

                            case "Ci":
                                //Realizo el ordenamiento en base a la Cédula
                                cs = cs.OrderBy(o => o.Ci).ToList();
                                break;
                        }

                        //Vacío el DataGrid y lo seteo con la nueva lsita ordenada.
                        DGVCajeros.DataSource = null;
                        DGVCajeros.DataSource = cs;

                        //Seteo el ordenamiento en el valor contrario, asi la proxima vez que se llame a este evento, en vez de ordenar de A-Z
                        //ordenara la lista de Z-A
                        cambio = true;
                        break;

                    //Lo mismo que arriba pero de Z-A
                    case true:
                        switch (headerText)
                        {
                            case "NomCompleto":
                                cs = cs.OrderByDescending(o => o.NomCompleto).ToList();
                                break;

                            case "Usu":
                                cs = cs.OrderByDescending(o => o.Usu).ToList();
                                break;

                            case "Ci":
                                cs = cs.OrderByDescending(o => o.Ci).ToList();
                                break;
                        }
                        DGVCajeros.DataSource = null;
                        DGVCajeros.DataSource = cs;
                        cambio = false;
                        break;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error"); }
        }
    }
}
