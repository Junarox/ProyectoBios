using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EntidadesCompartidas;

namespace Persistencia
{
    public interface IPGerente
    {
        int Logueo(string usu, string Clave);
        Gerente BuscarGerenteLogueo(string usu);
        void AltaGerente(Gerente Gerente);
        void ModificarClave(Gerente _gerente, string _clave);

    }
}
