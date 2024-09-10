using Infraction.Backend.Image.Service.Infrastructure.Exceptions;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Infraction.Backend.Image.Service.Infrastructure.Middleware
{
    /// <summary>
    /// Clase encargada de capturar todos los errores y devolverlos como respuesta.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        /// <summary>
        ///Delegado.
        /// </summary>
        private readonly RequestDelegate _next;
        /// <summary>
        /// Servicio de Logger
        /// </summary>
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        /// <summary>
        /// Constructor para instanciar la clase.
        /// </summary>
        /// <param name="next">Delegado.</param>
        /// <param name="logger">Servicio de Logger</param>
        /// <param name="messageManager">Servicio de administración de mensajes</param>
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger;
        }
        

        /// <summary>
        /// Método encargado de capturar los errores que puedan ocurrir en el trancurso de la petición.
        /// </summary>
        /// <param name="context">Contexto actual de petición HttpContext.</param>
        /// <exception cref="ArgumentNullException">Excepción en caso de contexto nulo.</exception>
        /// <returns>Operación asíncrona.</returns>
        public async Task Invoke(HttpContext context)
        {            
            ArgumentNullException.ThrowIfNull(nameof(context));
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, """Se produjo un error. Mensaje: {exceptionMsg} - StackTrace: {exceptionStack} - InnerException: {exceptionInnerMsg}""");
                
            }
        }

        /// <summary>
        /// Método encargado para la respuesta de excepciones
        /// </summary>
        /// <param name="StatusCode">Código de estatus.</param>
        /// <param name="Message">Mensaje de la excepción.</param>
        /// <param name="Error">Bandera que indica si la respuesta es error, true => Error, false => Respuesta correcta, para las excepciones por defualt será true.</param>
        /// <param name="Data">Objeto de respuesta.</param>
        public record ExceptionResponse(HttpStatusCode StatusCode, string Message, bool Error = true, Object? Data = null);

        /// <summary>
        /// Método encargado para controlar las excepciones
        /// </summary>
        /// <param name="context">Contexto actual de petición HttpContext.</param>
        /// <param name="exception">Excepción capturada.</param>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception, string message)
        {
            if (exception.GetType() != typeof(BusinessRuleValidationException))
            {
                string? exceptionMsg = exception.Message;
                string? exceptionStack = exception.StackTrace;
                string? exceptionInnerMsg = exception.InnerException?.Message;
                _logger.LogWarning(message, exceptionMsg, exceptionStack, exceptionInnerMsg);
            }
          
                //More log stuff        

                ExceptionResponse response = exception switch
            {
                ApplicationException _ => new ExceptionResponse(HttpStatusCode.BadRequest, "Application exception occurred."),
                KeyNotFoundException _ => new ExceptionResponse(HttpStatusCode.NotFound, "The request key not found."),
                UnauthorizedAccessException _ => new ExceptionResponse(HttpStatusCode.Unauthorized, "Unauthorized."),
                BusinessRuleValidationException _ => new ExceptionResponse(HttpStatusCode.NonAuthoritativeInformation, exception.Message),
                _ => new ExceptionResponse(HttpStatusCode.InternalServerError, $"Se produjo un error. Mensaje: {exception.Message} - StackTrace: {exception.StackTrace} - InnerException: {exception.Message}")
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)response.StatusCode;
            await context.Response.WriteAsJsonAsync(response);
            
        }
    }

}
