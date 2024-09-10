using System.Net;

namespace Infraction.Backend.Image.Service.Infrastructure.Utils.ResponseService
{
    /// <summary>
    /// clase para el Response de las API's
    /// </summary>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Código de estatus
        /// </summary>
        public int? StatusCode { get; set; } = StatusCodes.Status200OK;
        /// <summary>
        /// Mensaje de respuesta
        /// </summary>
        public string? Message { get; set; }
        /// <summary>
        /// Objeto de respuesta
        /// </summary>
        public T? Data { get; set; }
        /// <summary>
        /// Bandera que indica si la respuesta es error, true => Error, false => Respuesta correcta,por default será false.
        /// </summary>
        public bool Error { get; set; } = false;        
    }
}
