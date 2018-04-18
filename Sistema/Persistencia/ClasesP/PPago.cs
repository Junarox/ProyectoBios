using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using EntidadesCompartidas;
using System.Transactions;

namespace Persistencia
{
    internal class PPago : IPPago
    {
        private static PPago _instancia = null;
        private PPago() { }
        public static PPago GetInstancia()
        {
            if (_instancia == null)
                _instancia = new PPago();
            return _instancia;
        }
        public List<Pago> ListarPagos()
        {
            List<Pago> listaPagos = null;

            SqlConnection cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand cmd = new SqlCommand("ListarPagos", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cnn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    listaPagos = new List<Pago>();
                    while (reader.Read())
                    {
                        listaPagos.Add(new Pago((int)reader["NumInterno"],
                            (DateTime)reader["Fecha"], (int)reader["Monto"],
                            PCajero.GetInstancia().BuscarCajero(Convert.ToInt32(reader["Cajero"])),
                            ListarFacturas(Convert.ToInt32(reader["NumInterno"]))));
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
            return listaPagos;

        }
        public List<LineaPago> ListarFacturas(int _NumeroInterno)
        {

            List<LineaPago> listaFacturas = null;
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand cmd = new SqlCommand("ListarFacturas", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@NumeroInterno", _NumeroInterno);

            try
            {
                cnn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    listaFacturas = new List<LineaPago>();
                    while (reader.Read())
                    {
                        listaFacturas.Add(new LineaPago(Convert.ToInt32(reader["Monto"]), 
                            Convert.ToDateTime(reader["FechaVencimiento"]), 
                            Convert.ToInt32(reader["CodCliente"]), 
                            PContrato.GetInstancia().BuscarContrato(Convert.ToInt32(reader["CodigoEmpresa"]), Convert.ToInt32(reader["TipoContrato"]))));
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
            return listaFacturas;
        }
        public void AltaPago(Pago _pago)
        {
            using (SqlConnection cnn = new SqlConnection(Conexion.Cnn))
            {

                cnn.Open();

                SqlCommand cmd = new SqlCommand("AltaPago", cnn);

                SqlTransaction transaccion;
                transaccion = cnn.BeginTransaction("SampleTransaction");

                cmd.Connection = cnn;
                cmd.Transaction = transaccion;

                try
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Fecha", _pago.Fecha);
                    cmd.Parameters.AddWithValue("@Monto", _pago.Monto);
                    cmd.Parameters.AddWithValue("@Cajero", _pago.Cajero.Ci);
                    SqlParameter retorno = new SqlParameter("@Retorno", SqlDbType.Int);
                    retorno.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(retorno);

                    cmd.ExecuteNonQuery();

                    //Verifico que el primer SP no haya dado error
                    switch ((int)retorno.Value)
                    {
                        case -1:
                            throw new Exception("Error al ingresar pago, intente nuevamente más tarde.");
                        case -2:
                            throw new Exception("No existe el usuario cajero que está ejecutando esta operación.");
                        default:
                            foreach (LineaPago factura in _pago.LineasPago)
                            {

                                cmd.Parameters.Clear();
                                cmd.Transaction = transaccion;
                                cmd.CommandText = "RegistrarFacturaEnPago";

                                cmd.Parameters.Add(retorno);
                                cmd.Parameters.AddWithValue("@CodigoEmpresa", factura.Contrato.Empresa.Codigo);
                                cmd.Parameters.AddWithValue("@TipoContrato", factura.Contrato.CodContrato);
                                cmd.Parameters.AddWithValue("@CodCliente", factura.CodigoCliente);
                                cmd.Parameters.AddWithValue("@FechaVencimiento", factura.FechaVencimiento);
                                cmd.Parameters.AddWithValue("@Monto", factura.Monto);

                                cmd.ExecuteNonQuery();
                                //Verifico que cada ingreso de factura no de error
                                switch ((int)retorno.Value)
                                {
                                    case -1:
                                        throw new Exception("Error al registrar factura, intente nuevamente más tarde.");
                                    case -2:
                                        throw new Exception("Error al registrar factura, no existe el tipo de contrato de la factura.\r\nPor favor verifique que el tipo de contrato exista y que no haya más de una factura para el mismo contrato.");
                                    default:
                                        break;
                                }
                            }
                            break;
                    }
                    transaccion.Commit();
                }
                catch (Exception ex)
                {
                    try
                    {
                        //En caso de error hago Rollback para cancelar la transacción
                        transaccion.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        throw new Exception(ex2.Message);
                    }
                    throw new Exception(ex.Message);
                }
            }
        }



    }
}
