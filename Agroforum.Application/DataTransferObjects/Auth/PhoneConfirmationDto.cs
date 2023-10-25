namespace Agroforum.Application.DataTransferObjects.Auth
{
    public class PhoneConfirmationDto
    {
        public Guid UserId { get; set; }
        public string Code { get; set; }
    }
}