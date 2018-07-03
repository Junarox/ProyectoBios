using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistencia
{
    internal class Conexion
    {
        public static string Cnn(string usuario, string clave)
        {
            return "Data Source= localhost; Initial Catalog = BiosMoney; User = " + usuario+ "; Password = " + clave + ";";
        }

        public static string CnnLogueo()
        {
            return "Data Source= localhost; Initial Catalog = BiosMoney; Integrated Security = true;";
        }
        

    }
}
