using Infraction.Backend.Image.Service.Api.Models;
using Infraction.Backend.Image.Service.Infrastructure.Utils.Enums;
using Infraction.Backend.Image.Service.Infrastructure.Utils.Shared;
using Invoice.Service.Core.Interfaces;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.IO.Compression;
using System.Xml.Linq;
namespace Infraction.Backend.Image.Service.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvoiceController : ControllerBase
    {        
        private readonly IConfiguration _configuration;  
        private readonly IReportService _reportService;

        public InvoiceController(IConfiguration configuration, IReportService reportService)
        {            
            _configuration = configuration;         
            _reportService = reportService;
        }

        /// <summary>
        /// Sube el xml para generar el reporte d ela factura
        /// </summary>        
        /// <param name="xmlFile">Archivo XML</param>
        /// <returns>Mensaje de resultado</returns>
        [HttpPost("get-pdf")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
        {
            IFormFile zipFile = file;
            if (zipFile == null || zipFile.Length == 0)
            {
                return BadRequest("No se proporcionó un archivo válido.");
            }

            // Validar que el archivo tenga la extensión .zip
            var extension = Path.GetExtension(file.FileName);
            if (extension != ".zip")
            {
                return BadRequest("Solo se permite subir archivo ZIP.");
            }

            List<XDocument> XMLs = new List<XDocument>();

            // Cargar el archivo ZIP a un stream de memoria
            using (var memoryStream = new MemoryStream())
            {
                await zipFile.CopyToAsync(memoryStream);

                // Volver al inicio del stream
                memoryStream.Seek(0, SeekOrigin.Begin);

                
                // Abrir el ZIP
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Read))
                {
                    if (!archive.Entries.Any(entry => entry.FullName.Contains("SIGN")))
                    {
                        return BadRequest("Se requiere el XML con el timbre fiscal.");
                    }

                    foreach (var entry in archive.Entries)
                    {
                        // Verifica si el archivo es una carpeta (termina en '/')
                        if (entry.FullName.EndsWith("/"))
                            continue;

                        

                        // Leer el contenido de cada archivo dentro del ZIP
                        using (var entryStream = entry.Open())
                        {
                            // Aquí puedes leer el contenido del archivo
                            // Por ejemplo, podrías leer el archivo como un string
                            using (var reader = new StreamReader(entryStream))
                            {                                
                                XDocument xmlDocument;
                                xmlDocument = XDocument.Load(reader);
                                XMLs.Add(xmlDocument);
                                
                            }
                        }
                    }
                }
            }

            // Leer el contenido del archivo XML
            //XDocument xmlDocument;
            //using (var stream = file.OpenReadStream())
            //{
            //    xmlDocument = XDocument.Load(stream);
            //}            

            

            // Retornar el PDF generado
            return File(_reportService.Report(XMLs), "application/pdf", "CFDI_Report.pdf");
        }         
    }

}