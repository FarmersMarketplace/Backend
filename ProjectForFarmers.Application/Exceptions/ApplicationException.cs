namespace FarmersMarketplace.Application.Exceptions
{
    public class ApplicationException : Exception
    {
        public string UserFacingMessage { get; set; }
        public string? Environment {  get; set; }
        public string? Action { get; set; }

        public ApplicationException(string message, string userFacingMessage) : base(message) 
        {  
            UserFacingMessage = userFacingMessage;
        }

        public ApplicationException() : base()
        {
            
        }

        public ApplicationException(string message, string userFacingMessage, string? environment, string? action) : base(message) 
        {
            UserFacingMessage = userFacingMessage;
            Environment = environment;
            Action = action;
        }
    }

}
