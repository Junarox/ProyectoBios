using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;

using EntidadesCompartidas;
using Persistencia;

namespace Logica
{
    internal class LEmpresa : ILEmpresa
    {
        

        private static LEmpresa _instancia = null;
        private LEmpresa() { }
        public static LEmpresa GetInstancia()
        {
            if (_instancia == null)
                _instancia = new LEmpresa();
            return _instancia;
        }

        public void AltaEmpresa(Empresa _empresa)
        {
            FabricaP.GetPEmpresa().AltaEmpresa(_empresa);
        }

        public void BajaEmpresa(Empresa empresa)
        {
            FabricaP.GetPEmpresa().BajaEmpresa(empresa);
        }

        public Empresa BuscarEmpresa(int _codigo)
        {
            return (FabricaP.GetPEmpresa().BuscarEmpresa(_codigo));
        }

        public List<Empresa> ListarEmpresa()
        {
            return (FabricaP.GetPEmpresa().ListarEmpresa());
        }

        public void ModEmpresa(Empresa _empresa)
        {
            FabricaP.GetPEmpresa().ModEmpresa(_empresa);
        }
                
        

        
    }
}
