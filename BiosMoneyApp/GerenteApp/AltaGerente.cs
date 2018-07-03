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
    public partial class AltaGerente : Form
    {
        Usuario usuario;

        //Esto determina como se va a ordenar la lista del DataGrid
        private bool cambio;

        //Instancio la lista
        private List<Gerente> gs = new List<Gerente>();

        /// <summary>
        /// Ya sea un alta, modificar o eliminar, traigo la lista con los nuevos cambios de la DB
        /// </summary>
        public override void Refresh()
        {
            DGVGerentes.DataSource = null;
            DGVGerentes.DataSource = gs = FabricaL.GetLUsuario().ListarGerentes(usuario);
        }

        public AltaGerente(Usuario usuario)
        {
            this.usuario = usuario;
            InitializeComponent();
            DGVGerentes.AutoGenerateColumns = false;
            Refresh();
        }

        /// <summary>
        /// Agrega un Usuario Cajero a la DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            //Creo el Cajero.
            Gerente g = new Gerente();
            try
            {
                //Seteo sus datos.
                try
                {
                    g.Ci = Convert.ToInt32(txtCedula.Text);
                }
                catch (FormatException) { throw new FormatException("El Documento debe contener 8 digitos."); }
                catch (OverflowException) { throw new OverflowException("El Documento debe contener 8 digitos."); }

                g.Usu = txtUsuario.Text;
                g.NomCompleto = txtNombreC.Text;
                g.Email = txtEmail.Text;

                //Llamo a la fabrica para darlo de alta.
                FabricaL.GetLUsuario().Alta(g,usuario);

                //Refresco el DataGrid con los nuevos datos.
                Refresh();
            }
            catch (Exception ex) { lblError.Text = ex.Message; }
        }

        /// <summary>
        /// Ordeno los datos del DataGrid dependiendo de la columna seleccionada.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGVGerentes_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                //Traigo la lista del DataGrid (Puede usarse con el filtrado en conjunto).
                try
                {
                    if (DGVGerentes.DataSource == null)
                        throw new NullReferenceException();
                    List<Gerente> cs = (List<Gerente>)DGVGerentes.DataSource;
                }
                catch (NullReferenceException) { throw new NullReferenceException("No existe ningún Cajero."); }

                //Obtengo la columna a la que se le hizo click.
                //DataPropertyName: devuelve el valor en string que utiliza el DataGrid para referenciarse con la variable del Objeto.
                string headerText = DGVGerentes.Columns[e.ColumnIndex].DataPropertyName;

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
                                gs = gs.OrderBy(o => o.NomCompleto).ToList();
                                break;

                            case "Usu":
                                //Realizo el ordenamiento en base al Usuario
                                gs = gs.OrderBy(o => o.Usu).ToList();
                                break;

                            case "Ci":
                                //Realizo el ordenamiento en base a la Cédula
                                gs = gs.OrderBy(o => o.Ci).ToList();
                                break;
                        }

                        //Vacío el DataGrid y lo seteo con la nueva lsita ordenada.
                        DGVGerentes.DataSource = null;
                        DGVGerentes.DataSource = gs;

                        //Seteo el ordenamiento en el valor contrario, asi la proxima vez que se llame a este evento, en vez de ordenar de A-Z
                        //ordenara la lista de Z-A
                        cambio = true;
                        break;

                    //Lo mismo que arriba pero de Z-A
                    case true:
                        switch (headerText)
                        {
                            case "NomCompleto":
                                gs = gs.OrderByDescending(o => o.NomCompleto).ToList();
                                break;

                            case "Usu":
                                gs = gs.OrderByDescending(o => o.Usu).ToList();
                                break;

                            case "Ci":
                                gs = gs.OrderByDescending(o => o.Ci).ToList();
                                break;
                        }
                        DGVGerentes.DataSource = null;
                        DGVGerentes.DataSource = gs;
                        cambio = false;
                        break;
                }
            }

            catch (Exception ex) { lblError.Text = ex.Message; }
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
            List<Gerente> resultado = new List<Gerente>();

            //Este if es porque la primera vez que se inicia la aplicación, cuando se cargan los controles se llama al TextChanged 
            //y como el DataGrid está aún en null, se lanza la exception NullReferenceException
            if (DGVGerentes.DataSource != null)
            {
                //Variable para ver si un txt se puede transformar a int.
                int i = 0;

                //Si lo que ingreso en el texto se puede transformar en un int, es porque estoy intentando filtrar por una cedula.
                if (int.TryParse(txtBuscar.Text, out i))
                {
                    //Realizo el filtro mediante LinQ
                    resultado = (from Gerente g in (List<Gerente>)DGVGerentes.DataSource
                                 where g.Ci.ToString().StartsWith(txtBuscar.Text)
                                 select g).ToList();

                    //Vacío y seteo el DataGrid con los nuevos valores.
                    DGVGerentes.DataSource = null;
                    DGVGerentes.DataSource = resultado;
                }

                //Si el texto del txtBuscar es distinto de "" y no se puede convertir en int por el caso anterior, es porque estoy filtrando
                //por Nombre o Usuario.
                else if (txtBuscar.Text.Trim() != "")
                {
                    //Realizo el filtro mediante LinQ
                    //Aclaración: en el where, comparo el texto con los dos atributos y traigo los valores que coincidan, independientemente
                    //de si uno coincide con el otro o no
                    resultado = (from Gerente g in (List<Gerente>)DGVGerentes.DataSource
                                 where (g.NomCompleto.ToLower().StartsWith(txtBuscar.Text.ToLower()) || g.Usu.ToLower().StartsWith(txtBuscar.Text.ToLower()))
                                 select g).ToList();

                    //Vacío y seteo el DataGrid con los nuevos valores.
                    DGVGerentes.DataSource = null;
                    DGVGerentes.DataSource = resultado;
                }

                else
                    //En este caso el txtBuscar está en "", quiere decir que no se está realizando ningun filtrado, por lo que cargo el DataGrid
                    //con todos los datos.
                    //Aclaración: No uso la misma lista para cargarlo porque si en el filtrado modifiqué un Usuario, si no llamo al Refresh()
                    //no me va a traer las modificaciones.
                    Refresh();
            }
        }
    }
}
