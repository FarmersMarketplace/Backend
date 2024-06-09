namespace FarmersMarketplace.Application.Exceptions
{
    public class InvalidFormatException : Exception
    {
        public InvalidFormatException(string message, string userFacingMessage) : base(message)
        {
        }

        public InvalidFormatException() : base()
        {
        }
    }

}
