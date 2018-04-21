using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;

using EntidadesCompartidas;

namespace Persistencia
{
    internal class PContrato : IPContrato
    {
        private static PContrato _instancia = null;
        private PContrato() { }
        public static PContrato GetInstancia()
        {
            if (_instancia == null)
                _instancia = new PContrato();
            return _instancia;
        }

        public void AltaContrato(Contrato _contrato)
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand cmd = new SqlCommand("AltaContrato", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            retorno.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retorno);

            cmd.Parameters.AddWithValue("@CodEmpresa", _contrato.Empresa.Codigo);
            cmd.Parameters.AddWithValue("@CodTipo", _contrato.CodContrato);
            cmd.Parameters.AddWithValue("@Nombre", _contrato.NomContrato);

            try
            {
                cnn.Open();
                cmd.ExecuteNonQuery();
                if ((int)retorno.Value == -1)
                    throw new Exception("Este Contrato ya existe para esta Empresa.");
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

        public void BajaContrato(Contrato _contrato)
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand cmd = new SqlCommand("BajaContrato", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            retorno.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retorno);

            cmd.Parameters.AddWithValue("@CodEmpresa", _contrato.Empresa.Codigo);
            cmd.Parameters.AddWithValue("@CodTipo", _contrato.CodContrato);

            try
            {
                cnn.Open();
                cmd.ExecuteNonQuery();

                if ((int)retorno.Value == -1)
                    throw new Exception("No existe este Contrato.");
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

        public Contrato BuscarContrato(int CodEmpresa, int CodTipo)
        {
            Contrato _Contrato = null;

            SqlConnection cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand cmd = new SqlCommand("BuscarContrato", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CodEmpresa", CodEmpresa);
            cmd.Parameters.AddWithValue("@CodTipo", CodTipo);

            try
            {
                cnn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    _Contrato = new Contrato();
                    _Contrato.Empresa = FabricaP.GetPEmpresa().BuscarEmpresa((int)reader["CodEmpresa"]);
                    _Contrato.CodContrato = CodTipo;
                    _Contrato.NomContrato = (string)reader["Nombre"];
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
            return _Contrato;
        }

        public List<Contrato> ListarContrato(int CodEmpresa)
        {
            List<Contrato> _Contratos = null;
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand cmd = new SqlCommand("ListarContrato", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CodEmpresa", CodEmpresa);

            try
            {
                cnn.Open();
                cmd.ExecuteNonQuery();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    _Contratos = new List<Contrato>();
                    while (reader.Read())
                    {
                        Contrato _Contrato = new Contrato();
                        _Contrato.Empresa = FabricaP.GetPEmpresa().BuscarEmpresa(CodEmpresa);
                        _Contrato.CodContrato = Convert.ToInt32(reader["CodTipo"]);
                        _Contrato.NomContrato = (string)reader["Nombre"];
                        _Contratos.Add(_Contrato);
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

            return _Contratos;
        }

        public List<Contrato> ListarTodosLosContratos()
        {
            List<Contrato> _Contratos = null;
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand cmd = new SqlCommand("ListarTodosLosContratos", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cnn.Open();
                cmd.ExecuteNonQuery();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    _Contratos = new List<Contrato>();
                    while (reader.Read())
                    {
                        Contrato _Contrato = new Contrato();
                        _Contrato.Empresa = FabricaP.GetPEmpresa().BuscarEmpresa((int)reader["CodEmpresa"]);
                        _Contrato.CodContrato = Convert.ToInt32(reader["CodTipo"]);
                        _Contrato.NomContrato = (string)reader["Nombre"];
                        _Contratos.Add(_Contrato);
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

            return _Contratos;
        }

        public void ModContrato(Contrato _contrato)
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand cmd = new SqlCommand("ModContrato", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            retorno.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retorno);

            cmd.Parameters.AddWithValue("@CodEmpresa", _contrato.Empresa.Codigo);
            cmd.Parameters.AddWithValue("@CodTipo", _contrato.CodContrato);
            cmd.Parameters.AddWithValue("@Nombre", _contrato.NomContrato);

            try
            {
                cnn.Open();
                cmd.ExecuteNonQuery();

                if ((int)retorno.Value == -1)
                    throw new Exception("No existe este Contrato en esta Empresa.");
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

        public DateTime ChequearFacturaPaga(string[] _factura)
        {
            DateTime _fecha = new DateTime(9999, 1, 1);
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand cmd = new SqlCommand("ChequearFacturaPaga", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            string x = _factura[2].Substring(0, 2) + "/" + _factura[2].Substring(2, 2) + "/" + _factura[2].Substring(4, 4);

            cmd.Parameters.AddWithValue("@CodigoEmpresa", _factura[0]);
            cmd.Parameters.AddWithValue("@TipoContrato", _factura[1]);
            cmd.Parameters.AddWithValue("@FechaVencimiento", _factura[2].Substring(0, 2) + "/" + _factura[2].Substring(2, 2) + "/" + _factura[2].Substring(4, 4));
            cmd.Parameters.AddWithValue("@CodigoCliente", _factura[3]);
            cmd.Parameters.AddWithValue("@Monto", _factura[4]);

            try
            {
                cnn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        _fecha = (DateTime)reader["Fecha"];
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
            return _fecha;
        }
    }
}
