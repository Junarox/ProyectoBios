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

        public Gerente BuscarGerenteLogueo(string usu)
        {
            Gerente _gerente = null;

            using(SqlConnection cnn = new SqlConnection(Conexion.CnnLogueo()))
            {
                using(SqlCommand cmd = new SqlCommand("BuscarGerenteLogueo", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Usuario", usu);

                    try
                    {
                        cnn.Open();
                        using(SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();

                                _gerente = new Gerente(Convert.ToInt32(reader["Ci"]), 
                                    (string)reader["Usuario"], 
                                    (string)reader["Clave"], 
                                    (string)reader["NomCompleto"], 
                                    (string)reader["Email"]);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
            return _gerente;
        }

        public void AltaGerente(Gerente Gerente, string usuario, string clave)
        {
            using(SqlConnection cnn = new SqlConnection(Conexion.Cnn(usuario, clave)))
            {
                using(SqlCommand cmd = new SqlCommand("AltaGerente", cnn))
                {
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
                }
            }
        }

        public void ModificarClave(Gerente gerente, string clave)
        {
            using(SqlConnection cnn = new SqlConnection(Conexion.Cnn(gerente.Usu, gerente.Clave)))
            {
                using(SqlCommand cmd = new SqlCommand("ModClave", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter retorno = new SqlParameter("@Retorno", SqlDbType.Int);
                    retorno.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(retorno);

                    cmd.Parameters.AddWithValue("@Usuario", gerente.Usu);
                    cmd.Parameters.AddWithValue("@Clave", clave);

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
                }
            }
        }

        public List<Gerente> ListarGerentes(string usuario, string clave)
        {
            List<Gerente> _gerentes = null;

            using (SqlConnection cnn = new SqlConnection(Conexion.Cnn(usuario, clave)))
            {
                using (SqlCommand cmd = new SqlCommand("ListarGerentes", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        cnn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                _gerentes = new List<Gerente>();

                                while (reader.Read())
                                {
                                    Gerente _gerente = new Gerente(Convert.ToInt32(reader["Ci"]), 
                                        (string)reader["Usuario"], 
                                        (string)reader["Clave"], 
                                        (string)reader["NomCompleto"], 
                                        (string)reader["Email"]);

                                    _gerentes.Add(_gerente);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }

                    return _gerentes;
                }
            }
            
        }
    }
}
