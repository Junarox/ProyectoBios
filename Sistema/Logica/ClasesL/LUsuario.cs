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

        public Usuario Logueo(string usu, string clave)
        {
            Usuario _usu = null;

            int i = FabricaP.GetPCajero().Logueo(usu, clave);
            switch (i)
            {
                case 1:
                    _usu = FabricaP.GetPCajero().BuscarCajeroLogueo(usu);
                    break;
                case 2:
                    _usu = FabricaP.GetPGerente().BuscarGerenteLogueo(usu);
                    break;
                default:
                    throw new Exception("Usuario o Contraseña incorrectos.");
            }

            return _usu;
        }

        public void Alta(Usuario _usu)
        {
            if (_usu is Cajero)
                FabricaP.GetPCajero().AltaCajero((Cajero)_usu);
            else if (_usu is Gerente)
                FabricaP.GetPGerente().AltaGerente((Gerente)_usu);
            else
                throw new Exception("Usuario no reconocido.");
        }

        public void BajaCajero(Usuario Usu)
        {
            FabricaP.GetPCajero().BajaCajero((Cajero)Usu);
        }

        public Cajero BuscarCajero(int Ci)
        {
            return (FabricaP.GetPCajero().BuscarCajero(Ci));
        }

        public List<Cajero> ListarCajero()
        {
            return (FabricaP.GetPCajero().ListarCajero());
        }

        public void Modificar(Usuario _usu)
        {
            if (_usu is Cajero)
                FabricaP.GetPCajero().ModificarCajero((Cajero)_usu);
            else
                throw new Exception("Usuario no reconocido.");
        }

        public void ModificarClave(Usuario _usu, string clave, string reclave)
        {
            if (clave.Equals(reclave))
            {
                if (clave.Length <= 7)
                {
                    if (_usu is Cajero)
                        FabricaP.GetPCajero().ModificarClave((Cajero)_usu, clave);
                    else if (_usu is Gerente)
                        FabricaP.GetPGerente().ModificarClave((Gerente)_usu, clave);

                }
                else
                    throw new Exception("La clave puede contener hasta 7 caracteres.");
            }
            else
                throw new Exception("Las claves no coinciden.");
        }
    }
}
