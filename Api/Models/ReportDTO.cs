namespace Invoice.Service.Api.Models
{
    public class ReportDTO
    {
        public string RFCEmisor { get; set; }
        public string NombreEmisor { get; set; }
        public string RegimenFiscalEmisor { get; set; }
        public string LugarExpedicionEmisor { get; set; }
        public string RFCReceptor { get; set; }
        public string NombreReceptor { get; set; }
        public string UsoCFDIReceptor { get; set; }
        public string DomicilioFiscalReceptor { get; set; }
        public string RegimenFiscalReceptor { get; set; }
        public string DomicilioReceptor { get; set; }
        public string Exportacion { get; set; }
        public string Moneda { get; set; }
        public string TipoCambio { get; set; }
        public string CondicionCambio { get; set; }
        public string FormaPago { get; set; }
        public string MetodoPago { get; set; }

    }
}
