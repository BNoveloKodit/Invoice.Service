namespace Infraction.Backend.Image.Service.Api.Models
{
    /// <summary>
    /// Modelo para la solicitud de la actualización parcial de un objeto.
    /// </summary>
    public class PatchInfraccionImagenRequest
    {

        public string? UrlImagen { get; set; }
        public int? Tipo { get; set; }
        public Guid? IdInfraccion { get; set; }
        public int? User_update { get; set; }


    }
}
