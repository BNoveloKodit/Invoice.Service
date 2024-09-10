namespace Infraction.Backend.Image.Service.Infrastructure.Exceptions
{
    /// <summary>
    /// Clase para el control de las reglas de negocio
    /// </summary>    
    public class BusinessRuleValidationException : Exception
    {
        public BusinessRuleValidationException() { }

        public BusinessRuleValidationException(string message) : base(message) { }

        public BusinessRuleValidationException(string message, Exception innerException) : base(message, innerException) { }
    }
}