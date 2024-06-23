namespace FarmersMarketplace.Application.Exceptions
{
    public class AuthorizationException : ApplicationException
    {
        public AuthorizationException(string message, string userFacingMessage, params string[] details) : base(message, userFacingMessage, details)
        {
        }

        public AuthorizationException() : base()
        {

        }

        public AuthorizationException(string message, string userFacingMessage, string? environment, string? action) : base(message, userFacingMessage, environment, action)
        {
        }
    }

}
