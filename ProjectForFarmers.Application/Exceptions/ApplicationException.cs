namespace FarmersMarketplace.Application.Exceptions
{
    public class ApplicationException : Exception
    {
        public string UserFacingMessageKey { get; set; }
        public string[] Details { get; set; }
        public string? Environment {  get; set; }
        public string? Action { get; set; }

        public ApplicationException(string message, string userFacingMessageKey, params string[] details) : base(message) 
        {  
            UserFacingMessageKey = userFacingMessageKey;
            details = Details;
        }

        public ApplicationException() : base()
        {
            
        }

        public ApplicationException(string message, string userFacingMessageKey, string? environment, string? action) : base(message) 
        {
            UserFacingMessageKey = userFacingMessageKey;
            Environment = environment;
            Action = action;
        }
    }

}
