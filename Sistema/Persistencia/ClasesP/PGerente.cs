using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EntidadesCompartidas;
using System.Data.SqlClient;
using System.Data;

namespace Persistencia
{
    internal class PGerente : IPGerente
    {
        private static PGerente _instancia = null;
        private PGerente() { }
        public static PGerente GetInstancia()
        {
            if (_instancia == null)
                _instancia = new PGerente();
            return _instancia;
        }

        public int Logueo(string usu, string Clave)
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand cmd = new SqlCommand("Logueo", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Usuario", usu);
            cmd.Parameters.AddWithValue("@Clave", Clave);
            SqlParameter retorno = new SqlParameter("@retorno", SqlDbType.Int);
            retorno.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retorno);

            try
            {
                cnn.Open();

                cmd.ExecuteNonQuery();
                if ((int)retorno.Value == 1)
                    return 1;
                else if ((int)retorno.Value == 2)
                    return 2;
                else
                    throw new Exception("Usuario o Contraseña incorrectos.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                cnn.Close();
            }
        }

        public Gerente BuscarGerenteLogueo(string usu)
        {
            Gerente _gerente = null;
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand cmd = new SqlCommand("BuscarGerenteLogueo", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Usuario", usu);

            try
            {
                cnn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    _gerente = new Gerente();
                    _gerente.Usu = (string)reader["Usuario"];
                    _gerente.Clave = (string)reader["Clave"];
                    _gerente.Ci = Convert.ToInt32(reader["Ci"]);
                    _gerente.NomCompleto = (string)reader["NomCompleto"];
                    _gerente.Email = (string)reader["Email"];
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                cnn.Close();
            }
            return _gerente;
        }

        public void AltaGerente(Gerente Gerente)
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand cmd = new SqlCommand("AltaGerente", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            retorno.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retorno);
            cmd.Parameters.AddWithValue("@Usuario", Gerente.Usu);
            cmd.Parameters.AddWithValue("@Clave", Gerente.Clave);
            cmd.Parameters.AddWithValue("@Ci", Gerente.Ci);
            cmd.Parameters.AddWithValue("@NomCompleto", Gerente.NomCompleto);
            cmd.Parameters.AddWithValue("@Email", Gerente.Email);

            try
            {
                cnn.Open();
                cmd.ExecuteNonQuery();
                if ((int)retorno.Value == -1)
                    throw new Exception("Este usuario ya existe.");
                if ((int)retorno.Value == -2)
                    throw new Exception("Error en la base de datos");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                cnn.Close();
            }
        }

        public void ModificarClave(Gerente _gerente, string _clave)
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand cmd = new SqlCommand("ModClave", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            retorno.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retorno);
            cmd.Parameters.AddWithValue("@Ci", _gerente.Ci);
            cmd.Parameters.AddWithValue("@Clave", _clave);

            try
            {
                cnn.Open();
                cmd.ExecuteNonQuery();
                if ((int)retorno.Value == -1)
                    throw new Exception("No existe el Usuario.");

                if ((int)retorno.Value == -2)
                    throw new Exception("Error en la DB.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                cnn.Close();
            }
        }
    }
}
