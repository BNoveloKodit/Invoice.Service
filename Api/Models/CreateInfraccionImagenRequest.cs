using System.ComponentModel.DataAnnotations;

namespace Infraction.Backend.Image.Service.Api.Models
{
    /// <summary>
    /// Modelo para la solicitud del alta de un Estado.
    /// </summary>
    public class CreateInfraccionImagenRequest
    { 
        public string? UrlImagen { get; set; }
        public int? Tipo { get; set; }
        public Guid? IdInfraccion { get; set; }
        public int? User_update { get; set; } 


    }
}
