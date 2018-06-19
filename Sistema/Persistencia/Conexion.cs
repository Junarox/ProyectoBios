using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistencia
{
    internal class Conexion
    {   //DIEGO
        //private static string _cnn = "Data Source= localhost; Initial Catalog = BiosMoney; Integrated Security = true";
        
        //MATEO
        private static string _cnn = "Data Source= localhost; Initial Catalog = BiosMoney; Integrated Security = true";
            

        public static string Cnn
        {
            get { return _cnn; }
        }
    }
}
