﻿using System;
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

        public void AltaContrato(Contrato contrato, Usuario logueo)
        {
            FabricaP.GetPContrato().AltaContrato(contrato, logueo.Usu, logueo.Clave);
        }

        public void BajaContrato(Contrato contrato, Usuario logueo)
        {
            FabricaP.GetPContrato().BajaContrato(contrato, logueo.Usu, logueo.Clave);
        }

        public Contrato BuscarContrato(int codEmpresa, int codTipo, Usuario logueo)
        {
            return (FabricaP.GetPContrato().BuscarContrato(codEmpresa, codTipo, logueo.Usu, logueo.Clave));
        }

        public List<Contrato> ListarContrato(Empresa empresa, Usuario logueo)
        {
            return (FabricaP.GetPContrato().ListarContrato(empresa.Codigo, logueo.Usu, logueo.Clave));
        }

        public List<Contrato> ListarTodosLosContratos()
        {
            return (FabricaP.GetPContrato().ListarTodosLosContratos());
        }

        public void ModContrato(Contrato contrato, Usuario logueo)
        {
            FabricaP.GetPContrato().ModContrato(contrato, logueo.Usu, logueo.Clave);
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
                    doc.Save("C:\\XMLPROYECTO\\archivo.xml");
                }


                return doc.OuterXml;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DateTime ChequearFacturaPaga(string factura)
        {
            //Creo array con los fragmentos del código de factura
            string[] facturas = new string[5];
            facturas[0] = factura.Substring(0, 4);
            facturas[1] = factura.Substring(4, 2);
            facturas[2] = factura.Substring(6, 8);
            facturas[3] = factura.Substring(14, 6);
            facturas[4] = factura.Substring(20, 5);

            DateTime fecha = (FabricaP.GetPContrato().ChequearFacturaPaga(facturas));

            return fecha;

        }
    }
}
