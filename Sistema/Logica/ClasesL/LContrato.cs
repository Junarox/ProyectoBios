using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EntidadesCompartidas;
using Persistencia;
using System.Xml.Linq;
using System.Xml;

namespace Logica
{
    internal class LContrato : ILContrato
    {
        //Creo lista de Contratos que se cargarà con los datos que provienen de la DB/Persistencia
        static List<Contrato> listaContratos = new List<Contrato>();

        private static LContrato _instancia = null;
        private LContrato() { }
        public static LContrato GetInstancia()
        {
            if (_instancia == null)
                _instancia = new LContrato();
            return _instancia;
        }

        public void AltaContrato(Contrato _contrato)
        {
            FabricaP.GetPContrato().AltaContrato(_contrato);
        }

        public void BajaContrato(Empresa Empresa, int CodTipo)
        {
            FabricaP.GetPContrato().BajaContrato(Empresa.Codigo, CodTipo);
        }

        public Contrato BuscarContrato(int _CodEmpresa, int _CodTipo)
        {
            return (FabricaP.GetPContrato().BuscarContrato(_CodEmpresa, _CodTipo));
        }

        public List<Contrato> ListarContrato(Empresa Empresa)
        {
            return (FabricaP.GetPContrato().ListarContrato(Empresa.Codigo));
        }

        public List<Contrato> ListarTodosLosContratos()
        {
            return (FabricaP.GetPContrato().ListarTodosLosContratos());
        }

        public void ModContrato(Contrato _contrato)
        {
            FabricaP.GetPContrato().ModContrato(_contrato);
        }

        public string GenerarXMLContratos()
        {
            //Creo XML de contratos
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<?xml version='1.0' encoding='utf-8' ?> <Contratos> </Contratos>");
            XmlNode _raiz = doc.DocumentElement;
            try
            {
                if (listaContratos != null)
                {
                    listaContratos.Clear();
                }

                listaContratos = ListarTodosLosContratos();

                if (listaContratos != null)
                {                 
                    //Recorro lista de contratos, creo un nuevo nodo XML y cargo datos
                    foreach (Contrato c in listaContratos)
                    {

                        XmlElement _CodigoContrato = doc.CreateElement("CodigoContrato");
                        _CodigoContrato.InnerText = c.CodContrato.ToString();

                        XmlElement _NombreContrato = doc.CreateElement("NombreContrato");
                        _NombreContrato.InnerText = c.NomContrato;

                        XmlElement _NombreEmpresa = doc.CreateElement("NombreEmpresa");
                        _NombreEmpresa.InnerText = c.Empresa.Nombre;

                        XmlElement _CodigoEmpresa = doc.CreateElement("CodigoEmpresa");
                        _CodigoEmpresa.InnerText = c.Empresa.Codigo.ToString();

                        XmlElement _RUT = doc.CreateElement("RUT");
                        _RUT.InnerText = c.Empresa.Rut.ToString();

                        XmlElement _DirFiscal = doc.CreateElement("DirFiscal");
                        _DirFiscal.InnerText = c.Empresa.DirFiscal;

                        XmlElement _TelefonoEmpresa = doc.CreateElement("TelefonoEmpresa");
                        _TelefonoEmpresa.InnerText = c.Empresa.Tel.ToString();

                        XmlElement _Nodo = doc.CreateElement("Contrato");
                        _Nodo.AppendChild(_CodigoContrato);
                        _Nodo.AppendChild(_NombreContrato);
                        _Nodo.AppendChild(_NombreEmpresa);
                        _Nodo.AppendChild(_CodigoEmpresa);
                        _Nodo.AppendChild(_RUT);
                        _Nodo.AppendChild(_DirFiscal);
                        _Nodo.AppendChild(_TelefonoEmpresa);

                        _raiz.AppendChild(_Nodo);
                    }
                }

                if (doc != null)
                {
                    doc.Save("D:\\XMLPROYECTO\\archivo.xml");
                }


                return doc.OuterXml;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DateTime ChequearFacturaPaga(string _factura)
        {
            //Creo array con los fragmentos del código de factura
            string[] _facturas = new string[5];
            _facturas[0] = _factura.Substring(0, 4);
            _facturas[1] = _factura.Substring(4, 2);
            _facturas[2] = _factura.Substring(6, 8);
            _facturas[3] = _factura.Substring(14, 6);
            _facturas[4] = _factura.Substring(20, 5);

            DateTime fecha = (FabricaP.GetPContrato().ChequearFacturaPaga(_facturas));

            return fecha;

        }
    }
}
