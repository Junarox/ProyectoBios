using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BiosMoneyApp.ServicioWCF;

namespace BiosMoneyApp
{
    public partial class Login : Form
    {
        IMiServicio SServicio = new MiServicioClient();
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogueo_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario usu = SServicio.Logueo(lblUsu.Text, lblClave.Text);
                if(usu is Cajero)
                {
                    //Abrir el form Menu Cajeros.
                    this.Hide();
                    Form f = new Menu(usu);
                    f.ShowDialog();
                    
                }
                else if(usu is Gerente)
                {
                    //Abrir el form Menu Gerente
                    this.Hide();
                    Form f = new Menu(usu);
                    f.ShowDialog();
                }
                else
                {
                    throw new Exception("Error de logueo.");
                }
                
            }catch(Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }
}
