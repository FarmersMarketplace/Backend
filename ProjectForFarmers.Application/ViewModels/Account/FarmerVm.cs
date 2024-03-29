using FarmersMarketplace.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.ViewModels.Account
{
    public class FarmerVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Patronymic { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? AdditionalPhone { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? AvatarName { get; set; }
        public ProducerPaymentDataVm? PaymentData { get; set; }
        public AddressVm? Address { get; set; }
    }

}
