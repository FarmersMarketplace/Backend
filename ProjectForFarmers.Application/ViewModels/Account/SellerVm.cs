using FarmersMarketplace.Application.DataTransferObjects;
using FarmersMarketplace.Application.ViewModels.Category;
using FarmersMarketplace.Application.ViewModels.Subcategory;
using FarmersMarketplace.Domain;
using FarmersMarketplace.Domain.Account;
using FarmersMarketplace.Domain.Payment;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.ViewModels.Account
{
    public class SellerVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Patronymic { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? AdditionalPhone { get; set; }
        public string? Password { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public IFormFile? ImagesNames { get; set; }
        public ProducerPaymentDataVm? PaymentData { get; set; }
        public AddressVm? Address { get; set; }
        public virtual ScheduleVm Schedule { get; set; }
        public string? FirstSocialPageUrl { get; set; }
        public string? SecondSocialPageUrl { get; set; }
        public List<ReceivingMethod>? ReceivingMethods { get; set; }
        public List<PaymentType>? PaymentTypes { get; set; }
        public List<CategoryLookupVm>? Categories { get; set; }
        public List<SubcategoryVm>? Subcategories { get; set; }
    }

}
