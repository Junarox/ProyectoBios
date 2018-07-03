using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntidadesCompartidas;

namespace Logica
{
    public interface ILEmpresa
    {
        void AltaEmpresa(Empresa empresa, Usuario logueo);
        void ModEmpresa(Empresa empresa, Usuario logueo);
        void BajaEmpresa(Empresa empresa, Usuario logueo);
        Empresa BuscarEmpresa(int codigo, Usuario logueo);
        List<Empresa> ListarEmpresa(Usuario logueo);
    }
}
