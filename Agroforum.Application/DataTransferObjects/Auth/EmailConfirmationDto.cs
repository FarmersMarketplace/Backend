namespace Agroforum.Application.DataTransferObjects.Auth
{
    public class EmailConfirmationDto
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
    }
}
