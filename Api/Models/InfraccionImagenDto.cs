using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Infraction.Backend.Image.Service.Api.Models.LinePay
{
    /// <summary>
    /// Objeto para la entidad LineaCaptura.
    /// </summary>
    public class InfraccionImagenDto
    {

        public Guid? id { get; set; }
        public string? UrlImagen { get; set; }
        public int? Tipo { get; set; }
        public Guid? IdInfraccion { get; set; }
        public int? User_update { get; set; }
        public int? Estatus { get; set; } = 1;


    }
}
