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

        public void AltaEmpresa(Empresa empresa, Usuario logueo)
        {
            FabricaP.GetPEmpresa().AltaEmpresa(empresa, logueo.Usu, logueo.Clave);
        }

        public void BajaEmpresa(Empresa empresa, Usuario logueo)
        {
            FabricaP.GetPEmpresa().BajaEmpresa(empresa, logueo.Usu, logueo.Clave);
        }

        public Empresa BuscarEmpresa(int codigo, Usuario logueo)
        {
            return (FabricaP.GetPEmpresa().BuscarEmpresa(codigo, logueo.Usu, logueo.Clave));
        }

        public List<Empresa> ListarEmpresa(Usuario logueo)
        {
            return (FabricaP.GetPEmpresa().ListarEmpresa(logueo.Usu, logueo.Clave));
        }

        public void ModEmpresa(Empresa empresa, Usuario logueo)
        {
            FabricaP.GetPEmpresa().ModEmpresa(empresa, logueo.Usu, logueo.Clave);
        }
                
        

        
    }
}
