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

        public void AltaCajero(Cajero _cajero, string usuario, string clave)
        {
            using(SqlConnection cnn = new SqlConnection(Conexion.Cnn(usuario, clave)))
            {
                using(SqlCommand cmd = new SqlCommand("AltaCajero", cnn))
                {
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
                }
            }
        }

        public void BajaCajero(Cajero _cajero, string usuario, string clave)
        {
            using(SqlConnection cnn = new SqlConnection(Conexion.Cnn(usuario, clave)))
            {
                using(SqlCommand cmd = new SqlCommand("BajaCajero", cnn))
                {
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
            }
        }

        public Cajero BuscarCajero(int cedula, string usuario, string clave)
        {
            Cajero cajero = null;

            using(SqlConnection cnn = new SqlConnection(Conexion.Cnn(usuario, clave)))
            {
                using(SqlCommand cmd = new SqlCommand("BuscarCajero", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Ci", cedula);
                    try
                    {
                        cnn.Open();

                        using(SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();

                                cajero = new Cajero(Convert.ToInt32(reader["Ci"]), (string)reader["Usuario"], (string)reader["Clave"], (string)reader["NomCompleto"], (string)reader["HoraInicio"], (string)reader["HoraFin"]);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }

            return cajero;
        }

        public Cajero BuscarCajeroLogueo(string usuario, string clave)
        {
            Cajero cajero = null;

            using(SqlConnection cnn = new SqlConnection(Conexion.Cnn(usuario, clave)))
            {
                using(SqlCommand cmd = new SqlCommand("BuscarCajeroLogueo", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Usuario", usuario);
                    try
                    {
                        cnn.Open();

                        using(SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();

                                cajero = new Cajero(Convert.ToInt32(reader["Ci"]), (string)reader["Usuario"], (string)reader["Clave"], (string)reader["NomCompleto"], (string)reader["HoraInicio"], (string)reader["HoraFin"]);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }

            return cajero;
        }

        public List<Cajero> ListarCajeros(string usuario, string clave)
        {
            List<Cajero> _cajeros = null;

            using(SqlConnection cnn = new SqlConnection(Conexion.Cnn(usuario, clave)))
            {
                using(SqlCommand cmd = new SqlCommand("ListarCajeros", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        cnn.Open();
                        using(SqlDataReader reader = cmd.ExecuteReader())
                        {
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
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }

            return _cajeros;
        }

        public int Logueo(string usuario, string clave)
        {
            using(SqlConnection cnn = new SqlConnection(Conexion.Cnn(usuario, clave)))
            {
                using(SqlCommand cmd = new SqlCommand("Logueo", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Usuario", usuario);
                    cmd.Parameters.AddWithValue("@Clave", clave);

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
                }
            }
        }

        public void ModificarCajero(Cajero cajero, string usuario, string clave)
        {
            using(SqlConnection cnn = new SqlConnection(Conexion.Cnn(usuario, clave)))
            {
                using(SqlCommand cmd = new SqlCommand("ModCajero", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter retorno = new SqlParameter("@retorno", SqlDbType.Int);
                    retorno.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(retorno);

                    cmd.Parameters.AddWithValue("@Ci", cajero.Ci);
                    cmd.Parameters.AddWithValue("@NomCompleto", cajero.NomCompleto);
                    cmd.Parameters.AddWithValue("@HoraInicio", cajero.HoraInicio);
                    cmd.Parameters.AddWithValue("@HoraFin", cajero.HoraFin);

                    try
                    {
                        cnn.Open();
                        cmd.ExecuteNonQuery();

                        if ((int)retorno.Value == -1)
                            throw new Exception("No existe el Cajero.");
                        if ((int)retorno.Value == -2)
                            throw new Exception("Ya existe el Usuario: " + cajero.Usu + ".");
                        if ((int)retorno.Value == -3)
                            throw new Exception("Error.");

                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        public void ModificarClave(Cajero cajero, string clave)
        {
            using(SqlConnection cnn = new SqlConnection(Conexion.Cnn(cajero.Usu, cajero.Clave)))
            {
                using(SqlCommand cmd = new SqlCommand("ModClave", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter retorno = new SqlParameter("@Retorno", SqlDbType.Int);
                    retorno.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(retorno);

                    cmd.Parameters.AddWithValue("@Usuario", cajero.Usu);
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
                }
            }
        }
    }
}