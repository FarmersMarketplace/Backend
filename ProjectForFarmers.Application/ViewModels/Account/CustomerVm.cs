using FarmersMarketplace.Application.DataTransferObjects.Account;
using FarmersMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.ViewModels.Account
{
    public class CustomerVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string AvatarName { get; set; }
        public CustomerPaymentDataVm PaymentData { get; set; }
        public CustomerAddressVm Address { get; set; }
    }

}
