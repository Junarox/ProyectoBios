using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EntidadesCompartidas;

namespace Persistencia
{
    public interface IPEmpresa
    {
        void AltaEmpresa(Empresa _empresa, string usuario, string clave);
        void ModEmpresa(Empresa _empresa, string usuario, string clave);
        void BajaEmpresa(Empresa _empresa, string usuario, string clave);
        Empresa BuscarEmpresa(int _codigo, string usuario, string clave);
        List<Empresa> ListarEmpresa(string usuario, string clave);
    }
}
