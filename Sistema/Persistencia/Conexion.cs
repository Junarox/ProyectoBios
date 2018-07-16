using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

        public static void DatosPrueba()
        {
            using (SqlConnection cnn = new SqlConnection("Data Source= localhost; Initial Catalog = BiosMoney; Integrated Security = true;"))
            {
                using (SqlCommand cmd = new SqlCommand("DatosPrueba", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        cnn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch(Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
            
        }

    }
}
