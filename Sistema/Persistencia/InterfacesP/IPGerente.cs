using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EntidadesCompartidas;

namespace Persistencia
{
    public interface IPGerente
    {
        int Logueo(string usuario, string clave);
        Gerente BuscarGerenteLogueo(string usuario);
        void AltaGerente(Gerente gerente, string usuario, string clave);
        void ModificarClave(Gerente gerente, string _clave);
        List<Gerente> ListarGerentes(string usuario, string clave);

    }
}
