using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EntidadesCompartidas;
using System.Data;
using System.Data.SqlClient;

namespace Persistencia
{
    internal class PEmpresa : IPEmpresa
    {
        private static PEmpresa _instancia = null;
        private PEmpresa() { }
        public static PEmpresa GetInstancia()
        {
            if (_instancia == null)
                _instancia = new PEmpresa();
            return _instancia;
        }

        public void AltaEmpresa(Empresa _empresa, string usuario, string clave)
        {
            using(SqlConnection cnn = new SqlConnection(Conexion.Cnn(usuario, clave)))
            {
                using(SqlCommand cmd = new SqlCommand("AltaEmpresa", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter retorno = new SqlParameter("@Retorno", SqlDbType.Int);
                    retorno.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(retorno);

                    cmd.Parameters.AddWithValue("@Rut", _empresa.Rut);
                    cmd.Parameters.AddWithValue("@Codigo", _empresa.Codigo);
                    cmd.Parameters.AddWithValue("@Nombre", _empresa.Nombre);
                    cmd.Parameters.AddWithValue("@DirFiscal", _empresa.DirFiscal);
                    cmd.Parameters.AddWithValue("@Tel", _empresa.Tel);

                    try
                    {
                        cnn.Open();
                        cmd.ExecuteNonQuery();
                        if ((int)retorno.Value == -1)
                            throw new Exception("El código: " + _empresa.Codigo.ToString() + " ya está siendo utilizado por otra Empresa.");
                        if ((int)retorno.Value == -2)
                            throw new Exception("La Empresa de Rut: " + _empresa.Rut + " ya existe.");
                        if ((int)retorno.Value == -3)
                            throw new Exception("Error en la DB.");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        public void BajaEmpresa(Empresa empresa, string usuario, string clave)
        {
            using(SqlConnection cnn = new SqlConnection(Conexion.Cnn(usuario, clave)))
            {
                using(SqlCommand cmd = new SqlCommand("BajaEmpresa", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter retorno = new SqlParameter("@Retorno", SqlDbType.Int);
                    retorno.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(retorno);

                    cmd.Parameters.AddWithValue("@Codigo", empresa.Codigo);

                    try
                    {
                        cnn.Open();
                        cmd.ExecuteNonQuery();

                        if ((int)retorno.Value == -1)
                            throw new Exception("No existe la Empresa");
                        if ((int)retorno.Value == -2)
                            throw new Exception("Error en la DB");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        public Empresa BuscarEmpresa(int _codigo)
        {
            Empresa _Empresa = null;

            using(SqlConnection cnn = new SqlConnection(Conexion.CnnLogueo()))
            {
                using(SqlCommand cmd = new SqlCommand("BuscarEmpresa", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Codigo", _codigo);

                    try
                    {
                        cnn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    _Empresa = new Empresa((long)reader["Rut"], 
                                        Convert.ToInt32(reader["Codigo"]), 
                                        (string)reader["Nombre"], 
                                        (string)reader["DirFiscal"], 
                                        Convert.ToInt64(reader["Tel"]));
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }

                    return _Empresa;
                }
            }
        }

        public List<Empresa> ListarEmpresa(string usuario, string clave)
        {
            List<Empresa> _Empresas = new List<Empresa>();

            using(SqlConnection cnn = new SqlConnection(Conexion.Cnn(usuario, clave)))
            {
                using(SqlCommand cmd = new SqlCommand("ListarEmpresas", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        cnn.Open();
                        using(SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Empresa _empresa = new Empresa((long)reader["Rut"], 
                                        Convert.ToInt32(reader["Codigo"]), 
                                        (string)reader["Nombre"], 
                                        (string)reader["DirFiscal"], 
                                        Convert.ToInt64(reader["Tel"]));

                                    _Empresas.Add(_empresa);
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

            return _Empresas;
        }

        public void ModEmpresa(Empresa _empresa, string usuario, string clave)
        {
            using(SqlConnection cnn = new SqlConnection(Conexion.Cnn(usuario, clave)))
            {
                using(SqlCommand cmd = new SqlCommand("ModEmpresa", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter retorno = new SqlParameter("@Retorno", SqlDbType.Int);
                    retorno.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(retorno);

                    cmd.Parameters.AddWithValue("@Codigo", _empresa.Codigo);
                    cmd.Parameters.AddWithValue("@Rut", _empresa.Rut);
                    cmd.Parameters.AddWithValue("@DirFiscal", _empresa.DirFiscal);
                    cmd.Parameters.AddWithValue("@Nombre", _empresa.Nombre);
                    cmd.Parameters.AddWithValue("@Tel", _empresa.Tel);

                    try
                    {
                        cnn.Open();
                        cmd.ExecuteNonQuery();

                        if ((int)retorno.Value == -1)
                            throw new Exception("La Empresa no existe.");
                        if ((int)retorno.Value == -2)
                            throw new Exception("Ya existe la Empresa de Rut: " + _empresa.Rut + " .");
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
    }
}
