using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Infraction.Backend.Image.Service.Core.Entities
{
    public class InfraccionImagenEntity
    {

        public Guid? id { get; set; }
        public string? UrlImagen { get; set; }
        public int? Tipo { get; set; }
        public string? IdInfraccion { get; set; }
        public int? User_update { get; set; }
        public int? Estatus { get; set; } = 1;

    }
}
