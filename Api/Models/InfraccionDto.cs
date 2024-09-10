using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Infraction.Backend.Image.Service.Api.Models.LinePay
{
    /// <summary>
    /// Objeto para la entidad LineaCaptura.
    /// </summary>
    public class InfraccionDto
    {

        public Guid? Id_infraccion { get; set; }
        public string? Folio { get; set; }
        public int? Id_usuario { get; set; }
        public int? User_update { get; set; }
        public int? Estatus { get; set; } 


    }
}
