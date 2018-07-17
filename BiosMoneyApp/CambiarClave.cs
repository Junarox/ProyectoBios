using BiosMoneyApp.ServicioWCF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BiosMoneyApp
{
    public partial class CambiarClave : Form
    {
        IMiServicio SServicio = new MiServicioClient();
        private Usuario usu;

        public CambiarClave(Usuario usu)
        {
            this.usu = usu;
            InitializeComponent();
            
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (usu.Clave.Equals(txtClave.Text))
            {
                if (txtNuevaClave.Text.Equals(txtReClave.Text))
                {
                    try
                    {
                        SServicio.ModificarClave(usu, txtNuevaClave.Text, txtReClave.Text);
                    }
                    catch(Exception ex) { MessageBox.Show(ex.Message, "Error"); }

                    MessageBox.Show("Se ha modificado la clave con exito.");
                    usu.Clave = txtNuevaClave.Text;
                    this.Close();
                }
                else
                    MessageBox.Show("Las claves no coinciden.","Error");
            }
            else
                MessageBox.Show("Clave incorrecta.","Error");
        }
    }
}
