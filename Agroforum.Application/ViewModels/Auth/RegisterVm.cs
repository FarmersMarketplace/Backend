namespace Agroforum.Application.ViewModels.Auth
{
    public class RegisterVm
    {
        public Guid Id { get; set; }

        public RegisterVm(Guid id)
        {
            Id = id;
        }

        public RegisterVm() { }
    }
}
