using Invoice.Service.Core.Interfaces;
using Microsoft.Reporting.NETCore;
using System.Data;
using System.Xml;
using System.Xml.Linq;
using System;
using Microsoft.Reporting.NETCore.Internal.Soap.ReportingServices2005.Execution;
using ReportParameter = Microsoft.Reporting.NETCore.ReportParameter;
using QRCoder;
using Invoice.Service.Infrastructure.Utils.Shared;

namespace Invoice.Service.Core.Services
{
    public class ReportService : IReportService
    {
        public byte[] Report(List<XDocument> xmls)
        {

            List<ReportParameter> reportParameters = new List<ReportParameter>();
            // Crear el ReportViewer
            LocalReport localReport = new LocalReport();
            DataTable dataTable = new DataTable();
            DataTable dataTable2 = new DataTable();
            // Cargar el archivo RDLC
            var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Api\\Reports", "CFDI_Report.rdlc");
            if (!System.IO.File.Exists(reportPath))
            {
                //return NotFound("No se encontró el archivo de informe RDLC.");
            }

            if (xmls != null && xmls.Count > 1)
            {
                foreach(var xml in xmls) {
                    // Definir namespaces para poder extraer correctamente la información
                    XNamespace ns0 = "http://www.sap.com/eDocument/Mexico/CFDI/v4.0";
                    XNamespace ns5 = "http://www.sat.gob.mx/cfd/4";
                    XNamespace ns9 = "http://www.sap.com/eDocument/Mexico/CFDI/v4.0";
                    XNamespace ns3 = "http://www.sat.gob.mx/TimbreFiscalDigital";

                    // Extraer los datos clave del XML
                    var comprobante = xml.Descendants(ns5 + "Comprobante").FirstOrDefault();
                    if(comprobante != null)
                    {
                        var emisor = comprobante.Element(ns5 + "Emisor");
                        var receptor = comprobante.Element(ns5 + "Receptor");
                        var conceptos = comprobante.Element(ns5 + "Conceptos").Elements(ns5 + "Concepto");
                        var impuestos = comprobante.Element(ns5 + "Impuestos");

                        // Crear el archivo PDF en memoria
                        // Crear DataTable para los datos
                        dataTable.Columns.Add("RFCEmisor", typeof(string));
                        dataTable.Columns.Add("NombreEmisor", typeof(string));
                        dataTable.Columns.Add("RegimenFiscalEmisor", typeof(string));
                        dataTable.Columns.Add("LugarExpedicionEmisor", typeof(string));
                        dataTable.Columns.Add("RFCReceptor", typeof(string));
                        dataTable.Columns.Add("NombreReceptor", typeof(string));
                        dataTable.Columns.Add("UsoCFDIReceptor", typeof(string));
                        dataTable.Columns.Add("DomicilioFiscalReceptor", typeof(string));
                        dataTable.Columns.Add("RegimenFiscalReceptor", typeof(string));
                        dataTable.Columns.Add("DomicilioReceptor", typeof(string));
                        dataTable.Columns.Add("Exportacion", typeof(string));
                        dataTable.Columns.Add("Moneda", typeof(string));
                        dataTable.Columns.Add("TipoCambio", typeof(string));
                        dataTable.Columns.Add("CondicionCambio", typeof(string));
                        dataTable.Columns.Add("FormaPago", typeof(string));
                        dataTable.Columns.Add("MetodoPago", typeof(string));
                        dataTable.Columns.Add("QRCode", typeof(string));

                        dataTable.Rows.Add(
                                emisor.Attribute("Rfc")?.Value,
                                emisor.Attribute("Nombre")?.Value,
                                emisor.Attribute("RegimenFiscal")?.Value,
                                comprobante.Attribute("LugarExpedicion")?.Value,
                                receptor.Attribute("Rfc")?.Value,
                                receptor.Attribute("Nombre")?.Value,
                                receptor.Attribute("UsoCFDI")?.Value,
                                receptor.Attribute("DomicilioFiscalReceptor")?.Value,
                                "",
                                "",
                                comprobante.Attribute("Exportacion")?.Value,
                                comprobante.Attribute("Moneda")?.Value,
                                "1.0",
                                comprobante.Attribute("CondicionesDePago")?.Value,
                                comprobante.Attribute("FormaPago")?.Value,
                                comprobante.Attribute("MetodoPago")?.Value,
                                ""
                            );

                        
                        dataTable2.Columns.Add("ClaveProdServ", typeof(string));
                        dataTable2.Columns.Add("NoID", typeof(string));
                        dataTable2.Columns.Add("Descripcion", typeof(string));
                        dataTable2.Columns.Add("ClaveUnidad");
                        dataTable2.Columns.Add("Cantidad", typeof(string));
                        dataTable2.Columns.Add("ValorUnitario", typeof(string));
                        dataTable2.Columns.Add("Importe", typeof(string));
                        foreach (var concepto in conceptos)
                        {
                            dataTable2.Rows.Add(
                                concepto.Attribute("ClaveProdServ")?.Value,
                                concepto.Attribute("NoIdentificacion")?.Value,
                                concepto.Attribute("Descripcion")?.Value,
                                concepto.Attribute("ClaveUnidad")?.Value,
                                concepto.Attribute("Cantidad")?.Value,
                                concepto.Attribute("ValorUnitario")?.Value,
                                concepto.Attribute("Importe")?.Value
                            );
                        }

                        reportParameters.Add(new ReportParameter("TipoComprobante", comprobante?.Attribute("TipoDeComprobante")?.Value));
                        reportParameters.Add(new ReportParameter("Version", comprobante?.Attribute("Version")?.Value));
                        reportParameters.Add(new ReportParameter("FechaEmision", comprobante?.Attribute("Fecha")?.Value.Replace("T", " ")));
                        reportParameters.Add(new ReportParameter("Folio", comprobante?.Attribute("Folio")?.Value));
                        reportParameters.Add(new ReportParameter("Serie", comprobante?.Attribute("Serie")?.Value));
                        reportParameters.Add(new ReportParameter("SubTotal", comprobante?.Attribute("SubTotal")?.Value));
                        reportParameters.Add(new ReportParameter("Descuento", "0.0"));
                        reportParameters.Add(new ReportParameter("IVA", impuestos?.Attribute("TotalImpuestosTrasladados")?.Value));
                        reportParameters.Add(new ReportParameter("RET", "0.0"));
                        reportParameters.Add(new ReportParameter("TOTAL", comprobante?.Attribute("Total")?.Value));
                        reportParameters.Add(new ReportParameter("ImporteConLetra", NumeroALetras.Convertir(Convert.ToDecimal(comprobante?.Attribute("Total")?.Value)).ToUpper()));
                    }
                    else
                    {
                        var timbreFiscal = xml.Descendants(ns3 + "TimbreFiscalDigital").FirstOrDefault();
                        if (timbreFiscal != null)
                        {
                            string uuid = timbreFiscal.Attribute("UUID")?.Value;
                            string version = timbreFiscal.Attribute("Version")?.Value;
                            string fechaTimbrado = timbreFiscal.Attribute("FechaTimbrado")?.Value;
                            string selloCFD = timbreFiscal.Attribute("SelloCFD")?.Value;
                            string noCertificadoSAT = timbreFiscal.Attribute("NoCertificadoSAT")?.Value;
                            string selloSAT = timbreFiscal.Attribute("SelloSAT")?.Value;

                            Console.WriteLine("UUID: " + uuid);
                            Console.WriteLine("Fecha Timbrado: " + fechaTimbrado);
                            Console.WriteLine("Sello CFD: " + selloCFD);
                            Console.WriteLine("No. Certificado SAT: " + noCertificadoSAT);
                            Console.WriteLine("Sello SAT: " + selloSAT);

                            reportParameters.Add(new ReportParameter("UUID", uuid));
                            reportParameters.Add(new ReportParameter("Observaciones", ""));
                            reportParameters.Add(new ReportParameter("ComplementoCertSAT", $"||{version}|{uuid}|{fechaTimbrado}|{selloCFD}|{noCertificadoSAT}||"));
                            reportParameters.Add(new ReportParameter("NoCertEmisor", ""));
                            reportParameters.Add(new ReportParameter("ComplementoCertEmisor", selloCFD));
                            reportParameters.Add(new ReportParameter("NoCertSAT", noCertificadoSAT));
                            reportParameters.Add(new ReportParameter("ComplementoCertSelloSAT", selloSAT));
                        }

                        
                    }                                       
                }
            }

            // Convertir el byte[] a Base64 para usarlo como Embedded Image en el ReportViewer
            string base64Image = GenerateQrCode(xmls);
            dataTable.Rows[0].SetField(16, $"{base64Image}");

            // Agregar la imagen como parámetro al ReportViewer
            reportParameters.Add(new ReportParameter("QRCodeImage", $""));

            // Agregar el DataSource al ReportViewer
            localReport.DataSources.Add(new ReportDataSource("CFDIDataSet", dataTable));
            localReport.DataSources.Add(new ReportDataSource("CFDIConceptosDataSet", dataTable2));

            localReport.ReportPath = reportPath;
            localReport.EnableExternalImages = true;
            
            localReport.SetParameters(reportParameters);            
            
            // Generar el informe como PDF
            return localReport.Render("PDF");
        }


        public string GenerateQrCode(List<XDocument> xmls)
        {
            // Cargar el XML
            XNamespace ns5 = "http://www.sat.gob.mx/cfd/4";
            XNamespace ns4 = "http://www.sat.gob.mx/TimbreFiscalDigital";
            XNamespace ns3 = "http://www.sat.gob.mx/TimbreFiscalDigital";
            XDocument xmlDocument = new XDocument();
            XDocument xmlDocumentTimbre = new XDocument();
            foreach (var document in xmls)
            {
                // Extraer información del XML
                if(document.Descendants(ns5 + "Comprobante").FirstOrDefault() != null)
                {
                    xmlDocument = document;
                }
                else if(document.Descendants(ns3 + "TimbreFiscalDigital").FirstOrDefault() != null)
                {
                    xmlDocumentTimbre = document;
                }
            }

            // Extraer información del XML
            var comprobante = xmlDocument.Descendants(ns5 + "Comprobante").FirstOrDefault();
            var emisor = comprobante.Element(ns5 + "Emisor");
            var receptor = comprobante.Element(ns5 + "Receptor");
            var total = comprobante.Attribute("Total").Value;

            // UUID del Timbre Fiscal Digital
            var timbreFiscalDigital = xmlDocumentTimbre.Descendants(ns4 + "TimbreFiscalDigital").FirstOrDefault();
            var uuid = timbreFiscalDigital.Attribute("UUID").Value;

            // RFC Emisor y Receptor
            var rfcEmisor = emisor.Attribute("Rfc").Value;
            var rfcReceptor = receptor.Attribute("Rfc").Value;

            // Generar URL para el QR
            string url = $"https://verificacfdi.facturaelectronica.sat.gob.mx/default.aspx?re={rfcEmisor}&rr={rfcReceptor}&tt={total}&id={uuid}";

            // Crear el QR
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeImage = qrCode.GetGraphic(20); // Cambia el tamaño según tus necesidades

            // Guardar o devolver el QR
            return Convert.ToBase64String(qrCodeImage);
        }
    }
}
