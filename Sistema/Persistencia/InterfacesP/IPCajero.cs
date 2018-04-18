using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EntidadesCompartidas;

namespace Persistencia
{
    public interface IPCajero
    {
        int Logueo(string usu, string clave);
        void AltaCajero(Cajero _cajero);
        Cajero BuscarCajero(int Ci);
        Cajero BuscarCajeroLogueo(string usu);
        void ModificarCajero(Cajero _cajero);
        void ModificarClave(Cajero _cajero, string clave);
        void BajaCajero(Cajero _cajero);
        List<Cajero> ListarCajero();
    }
}
