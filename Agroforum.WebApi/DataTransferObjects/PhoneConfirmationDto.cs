namespace Agroforum.WebApi.Controllers
{
    public class PhoneConfirmationDto
    {
        public Guid UserId { get; set; }
        public string Code { get; set; }
    }
}