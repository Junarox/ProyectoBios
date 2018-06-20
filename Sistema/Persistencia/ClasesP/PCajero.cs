using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EntidadesCompartidas;
using System.Data.SqlClient;
using System.Data;

namespace Persistencia
{
    internal class PCajero : IPCajero
    {
        private static PCajero _instancia = null;
        private PCajero() { }
        public static PCajero GetInstancia()
        {
            if (_instancia == null)
                _instancia = new PCajero();
            return _instancia;
        }

        public void AltaCajero(Cajero _cajero)
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand cmd = new SqlCommand("AltaCajero", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            retorno.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retorno);

            cmd.Parameters.AddWithValue("@Usuario", _cajero.Usu);
            cmd.Parameters.AddWithValue("@Clave", _cajero.Clave);
            cmd.Parameters.AddWithValue("@CI", _cajero.Ci);
            cmd.Parameters.AddWithValue("@NomCompleto", _cajero.NomCompleto);
            cmd.Parameters.AddWithValue("@HoraInicio", _cajero.HoraInicio);
            cmd.Parameters.AddWithValue("@HoraFin", _cajero.HoraFin);

            try
            {
                cnn.Open();
                cmd.ExecuteNonQuery();
                if ((int)retorno.Value == -1)
                    throw new Exception("El usuario " + _cajero.Usu + " ya existe.");
                if ((int)retorno.Value == -2)
                    throw new Exception("Ya existe el usuario de cedula: " + _cajero.Ci + ".");
                if ((int)retorno.Value == -3)
                    throw new Exception("Error.");
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

        public void BajaCajero(Cajero _cajero)
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand cmd = new SqlCommand("BajaCajero", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            retorno.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retorno);

            cmd.Parameters.AddWithValue("@Ci", _cajero.Ci);

            try
            {
                cnn.Open();
                cmd.ExecuteNonQuery();
                if ((int)retorno.Value == -1)
                    throw new Exception("No existe el Cajero.");
                if ((int)retorno.Value == -2)
                    throw new Exception("Error.");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Cajero BuscarCajero(int Ci)
        {
            Cajero cajero = null;

            SqlConnection cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand cmd = new SqlCommand("BuscarCajero", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Ci", Ci);
            try
            {
                cnn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    cajero = new Cajero(Convert.ToInt32(reader["Ci"]), (string)reader["Usuario"], (string)reader["Clave"], (string)reader["NomCompleto"], (string)reader["HoraInicio"], (string)reader["HoraFin"]);
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
            return cajero;
        }

        public Cajero BuscarCajeroLogueo(string usu)
        {
            Cajero cajero = null;

            SqlConnection cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand cmd = new SqlCommand("BuscarCajeroLogueo", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Usuario", usu);
            try
            {
                cnn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    cajero = new Cajero(Convert.ToInt32(reader["Ci"]), (string)reader["Usuario"], (string)reader["Clave"], (string)reader["NomCompleto"], (string)reader["HoraInicio"], (string)reader["HoraFin"]);
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
            return cajero;
        }

        public List<Cajero> ListarCajeros()
        {
            List<Cajero> _cajeros = null;
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand cmd = new SqlCommand("ListarCajeros", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cnn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    _cajeros = new List<Cajero>();
                    while (reader.Read())
                    {
                        Cajero _cajero = new Cajero(Convert.ToInt32(reader["Ci"]), (string)reader["Usuario"], (string)reader["Clave"], (string)reader["NomCompleto"], (string)reader["HoraInicio"], (string)reader["HoraFin"]);
                        _cajeros.Add(_cajero);
                    }
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
            return _cajeros;
        }

        public int Logueo(string Usu, string Clave)
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand cmd = new SqlCommand("Logueo", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Usuario", Usu);
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

        public void ModificarCajero(Cajero _cajero)
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand cmd = new SqlCommand("ModCajero", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter retorno = new SqlParameter("@retorno", SqlDbType.Int);
            retorno.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retorno);

            cmd.Parameters.AddWithValue("@Ci", _cajero.Ci);
            cmd.Parameters.AddWithValue("@NomCompleto", _cajero.NomCompleto);
            cmd.Parameters.AddWithValue("@HoraInicio", _cajero.HoraInicio);
            cmd.Parameters.AddWithValue("@HoraFin", _cajero.HoraFin);

            try
            {
                cnn.Open();
                cmd.ExecuteNonQuery();
                if ((int)retorno.Value == -1)
                    throw new Exception("No existe el Cajero.");
                if ((int)retorno.Value == -2)
                    throw new Exception("Ya existe el Usuario: " + _cajero.Usu + ".");
                if ((int)retorno.Value == -3)
                    throw new Exception("Error.");

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

        public void ModificarClave(Cajero _cajero, string clave)
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand cmd = new SqlCommand("ModClave", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            retorno.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retorno);
            cmd.Parameters.AddWithValue("@Ci", _cajero.Ci);
            cmd.Parameters.AddWithValue("clave", clave);

            try
            {
                cnn.Open();
                cmd.ExecuteNonQuery();
                if ((int)retorno.Value == -1)
                    throw new Exception("No existe el Usuario");
                if ((int)retorno.Value == -2)
                    throw new Exception("Error.");
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