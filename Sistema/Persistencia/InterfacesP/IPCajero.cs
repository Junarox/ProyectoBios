using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EntidadesCompartidas;

namespace Persistencia
{
    public interface IPCajero
    {
        int Logueo(string usuario, string clave);
        void AltaCajero(Cajero cajero, string usuario, string clave);
        Cajero BuscarCajero(int cedula, string usuario, string clave);
        Cajero BuscarCajeroLogueo(string usuario, string clave);
        void ModificarCajero(Cajero cajero, string usuario, string clave);
        void ModificarClave(Cajero cajero, string clave);
        void BajaCajero(Cajero cajero, string usuario, string clave);
        List<Cajero> ListarCajeros(string usuario, string clave);
        void ActualizarHorasExtra(Cajero cajero, DateTime fecha, int minutosExtra);
    }
}
