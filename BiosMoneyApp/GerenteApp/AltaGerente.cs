
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
    public partial class AltaGerente : Form
    {
        IMiServicio SServicio = new MiServicioClient();
        Usuario usuario;

        //Instancio la lista
        private List<Gerente> gs = new List<Gerente>();

        /// <summary>
        /// Ya sea un alta, modificar o eliminar, traigo la lista con los nuevos cambios de la DB
        /// </summary>
        public override void Refresh()
        {
            DGVGerentes.DataSource = null;
            DGVGerentes.DataSource = gs = SServicio.ListarGerentes(usuario).ToList();
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
                SServicio.Alta(g, usuario);

                //Refresco el DataGrid con los nuevos datos.
                Refresh();
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
