using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EntidadesCompartidas;

namespace Logica
{
    public interface ILUsuario
    {
        Usuario Logueo(string usuario, string clave);
        void Alta(Usuario usuario, Usuario logueo);
        void BajaCajero(Usuario cajero, Usuario logueo);
        Cajero BuscarCajero(int Ci, Usuario logueo);
        List<Cajero> ListarCajeros(Usuario logueo);
        List<Gerente> ListarGerentes(Usuario logueo);
        void Modificar(Usuario cajero, Usuario logueo);
        void ModificarClave(Usuario usuario, string clave1, string clave2);
    }
}
