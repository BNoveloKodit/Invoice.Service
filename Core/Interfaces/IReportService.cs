using System.Xml.Linq;

namespace Invoice.Service.Core.Interfaces
{
    public interface IReportService
    {
        public byte[] Report(List<XDocument> xmls);
    }
}
