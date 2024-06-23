namespace FarmersMarketplace.Application.Exceptions
{
    public class DuplicateException : ApplicationException
    {
        public DuplicateException(string message, string userFacingMessage, params string[] details) : base(message, userFacingMessage, details)
        {
        }

        public DuplicateException() : base()
        {
            
        }

        public DuplicateException(string message, string userFacingMessage, string? environment, string? action) : base(message, userFacingMessage, environment, action)
        {
        }
    }

}
