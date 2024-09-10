using System.ComponentModel;

namespace Infraction.Backend.Image.Service.Infrastructure.Utils.Enums
{
    public enum FolderEnum
    {
        [Description("Infraccion")]
        Infraccion = 1,
             [Description("RetencionVehiculo")]
        RetencionVehiculo = 2,
             [Description("RetencionDocumento")]
        RetencionDocumento = 3
    }
}
