namespace Agroforum.Application.DataTransferObjects.Auth
{
    public class AddPhoneDto
    {
        public Guid UserId { get; set; }
        public string Number { get; set; }
        public DateTime DispatchDate { get; set; }
    }
}
