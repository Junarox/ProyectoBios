using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logica
{
    public class FabricaL
    {
        public static ILUsuario GetLUsuario()
        {
            return (LUsuario.GetInstancia());
        }

        public static ILEmpresa GetEmpresa()
        {
            return (LEmpresa.GetInstancia());
        }

        public static ILContrato GetContrato()
        {
            return (LContrato.GetInstancia());
        }

        public static ILPago GetPago()
        {
            return (LPago.GetInstancia());
        }
    }
}
