namespace Agroforum.WebApi.DataTransferObjects
{
    public class EmailConfirmationDto
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
    }
}
