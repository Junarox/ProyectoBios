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

namespace BiosMoneyApp
{
    public partial class CambiarClave : Form
    {
        private Usuario usu;

        public CambiarClave(Usuario usu)
        {
            this.usu = usu;
            InitializeComponent();
            
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (usu.Clave.Equals(txtClave.Text))
                {
                    if (txtNuevaClave.Text.Equals(txtReClave.Text))
                    {
                        FabricaL.GetLUsuario().ModificarClave(usu, txtNuevaClave.Text, txtReClave.Text);
                        lblError.Text ="Se ha modificado la clave con exito.";
                    }
                    else
                        throw new Exception("Las claves no coinciden.");
                }
                else
                    throw new Exception("Clave incorrecta.");
            }
            catch(Exception ex) { lblError.Text = ex.Message; }
        }
    }
}
