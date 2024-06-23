namespace FarmersMarketplace.Application.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string message, string userFacingMessage, params string[] details) : base(message, userFacingMessage, details)
        {
        }

        public NotFoundException() : base()
        {
        }

        public NotFoundException(string message, string userFacingMessage, string? environment, string? action) : base(message, userFacingMessage, environment, action)
        {
        }
    }
}
