using FarmersMarketplace.Application.DataTransferObjects.Account;
using FarmersMarketplace.Application.Exceptions;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Application.ViewModels;
using FarmersMarketplace.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using FarmersMarketplace.Application.DataTransferObjects.Farm;
using FarmersMarketplace.Application.DataTransferObjects;

namespace FarmersMarketplace.Application.Services.Business
{
    public class AccountService : Service, IAccountService
    {
        public AccountService(IMapper mapper, IApplicationDbContext dbContext, IConfiguration configuration) : base(mapper, dbContext, configuration)
        {
        }
    }
}
