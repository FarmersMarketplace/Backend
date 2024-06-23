namespace FarmersMarketplace.Application.Exceptions
{
    public class InvalidDataException : ApplicationException
    {

        public InvalidDataException(string message, string userFacingMessage, params string[] details) : base(message, userFacingMessage, details)
        {
        }

        public InvalidDataException() : base()
        {
        }

        public InvalidDataException(string message, string userFacingMessage, string? environment, string? action) : base(message, userFacingMessage, environment, action)
        {
        }
    }

}
