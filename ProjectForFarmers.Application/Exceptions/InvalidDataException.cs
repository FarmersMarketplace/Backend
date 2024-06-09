namespace FarmersMarketplace.Application.Exceptions
{
    public class InvalidDataException : Exception
    {

        public InvalidDataException(string message, string userFacingMessage) : base(message)
        {
        }

        public InvalidDataException() : base()
        {
        }
    }

}
