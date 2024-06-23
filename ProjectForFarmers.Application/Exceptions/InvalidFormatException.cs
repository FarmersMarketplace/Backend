namespace FarmersMarketplace.Application.Exceptions
{
    public class InvalidFormatException : ApplicationException
    {
        public InvalidFormatException(string message, string userFacingMessage, params string[] details) : base(message, userFacingMessage, details)
        {
        }

        public InvalidFormatException() : base()
        {
        }

        public InvalidFormatException(string message, string userFacingMessage, string? environment, string? action) : base(message, userFacingMessage, environment, action)
        {
        }
    }

}
