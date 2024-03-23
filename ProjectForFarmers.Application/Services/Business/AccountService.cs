using FarmersMarketplace.Application.DataTransferObjects.Account;
using FarmersMarketplace.Application.Exceptions;
using FarmersMarketplace.Application.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace FarmersMarketplace.Application.Services.Business
{
    public class AccountService : Service, IAccountService
    {
        public AccountService(IMapper mapper, IApplicationDbContext dbContext, IConfiguration configuration) : base(mapper, dbContext, configuration)
        {
        }
    }
}
