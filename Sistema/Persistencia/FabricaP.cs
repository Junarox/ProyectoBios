using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistencia
{
    public class FabricaP
    {
        public static IPGerente GetPGerente()
        {
            return (PGerente.GetInstancia());
        }

        public static IPCajero GetPCajero()
        {
            return (PCajero.GetInstancia());
        }

        public static IPEmpresa GetPEmpresa()
        {
            return (PEmpresa.GetInstancia());
        }

        public static IPContrato GetPContrato()
        {
            return (PContrato.GetInstancia());
        }

        public static IPPago GetPPago()
        {
            return (PPago.GetInstancia());
        }
    }
}
