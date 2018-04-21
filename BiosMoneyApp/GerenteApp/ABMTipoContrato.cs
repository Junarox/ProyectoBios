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
            DGVEmpresas.DataSource = null;
            DGVEmpresas.DataSource = emps = FabricaL.GetEmpresa().ListarEmpresa();

            Empresa emp = (Empresa)DGVEmpresas.CurrentRow.DataBoundItem;
            DGVContratos.DataSource = null;
            DGVContratos.DataSource = conts = FabricaL.GetContrato().ListarContrato(emp);
        }

        public ABMTipoContrato()
        {
            InitializeComponent();
        }
    }
}
