using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntidadesCompartidas;

namespace Logica
{
    public interface ILEmpresa
    {
        void AltaEmpresa(Empresa _empresa);
        void ModEmpresa(Empresa _empresa);
        void BajaEmpresa(Empresa _empresa);
        Empresa BuscarEmpresa(int _codigo);
        List<Empresa> ListarEmpresa();
    }
}
