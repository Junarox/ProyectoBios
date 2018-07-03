using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EntidadesCompartidas;
using Persistencia;

namespace Logica
{
    internal class LUsuario : ILUsuario
    {
        private static LUsuario _instancia = null;
        private LUsuario() { }
        public static LUsuario GetInstancia()
        {
            if (_instancia == null)
                _instancia = new LUsuario();
            return _instancia;
        }

        public Usuario Logueo(string usuario, string clave)
        {
            Usuario _usu = null;

            int i = FabricaP.GetPCajero().Logueo(usuario, clave);
            switch (i)
            {
                case 1:
                    _usu = FabricaP.GetPCajero().BuscarCajeroLogueo(usuario, clave);
                    break;
                case 2:
                    _usu = FabricaP.GetPGerente().BuscarGerenteLogueo(usuario);
                    break;
                default:
                    throw new Exception("Usuario o Contraseña incorrectos.");
            }

            return _usu;
        }

        public void Alta(Usuario usuario, Usuario logueo)
        {
            if (usuario is Cajero)
                FabricaP.GetPCajero().AltaCajero((Cajero)usuario, logueo.Usu, logueo.Clave);
            else if (usuario is Gerente)
                FabricaP.GetPGerente().AltaGerente((Gerente)usuario, logueo.Usu, logueo.Clave);
            else
                throw new Exception("Usuario no reconocido.");
        }

        public void BajaCajero(Usuario usuario, Usuario logueo)
        {
            FabricaP.GetPCajero().BajaCajero((Cajero)usuario, logueo.Usu, logueo.Clave);
        }

        public Cajero BuscarCajero(int cedula, Usuario logueo)
        {
            return (FabricaP.GetPCajero().BuscarCajero(cedula, logueo.Usu,logueo.Clave));
        }

        public List<Cajero> ListarCajeros(Usuario logueo)
        {
            return (FabricaP.GetPCajero().ListarCajeros(logueo.Usu,logueo.Clave));
        }

        public List<Gerente> ListarGerentes(Usuario logueo)
        {
            return (FabricaP.GetPGerente().ListarGerentes(logueo.Usu,logueo.Clave));
        }

        public void Modificar(Usuario usuario, Usuario logueo)
        {
            if (usuario is Cajero)
                FabricaP.GetPCajero().ModificarCajero((Cajero)usuario, logueo.Usu, logueo.Clave);
            else
                throw new Exception("Usuario no reconocido.");
        }

        public void ModificarClave(Usuario usuario, string clave, string reclave)
        {
            if (clave.Equals(reclave))
            {
                if (clave.Length <= 7)
                {
                    if (usuario is Cajero)
                        FabricaP.GetPCajero().ModificarClave((Cajero)usuario, clave);
                    else if (usuario is Gerente)
                        FabricaP.GetPGerente().ModificarClave((Gerente)usuario, clave);

                }
                else
                    throw new Exception("La clave puede contener hasta 7 caracteres.");
            }
            else
                throw new Exception("Las claves no coinciden.");
        }
    }
}
