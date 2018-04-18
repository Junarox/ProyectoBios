using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EntidadesCompartidas;

namespace Logica
{
    public interface ILUsuario
    {
        Usuario Logueo(string usu, string clave);
        void Alta(Usuario _usu);
        void BajaCajero(Usuario _cajero);
        Cajero BuscarCajero(int Ci);
        List<Cajero> ListarCajeros();
        List<Gerente> ListarGerentes();
        void Modificar(Usuario _cajero);
        void ModificarClave(Usuario _usu, string clave1, string clave2);
    }
}
