using ProjectForFarmers.Application.DataTransferObjects.Account;
using ProjectForFarmers.Application.Exceptions;
using ProjectForFarmers.Application.Interfaces;
using ProjectForFarmers.Application.ViewModels;
using ProjectForFarmers.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using ProjectForFarmers.Application.DataTransferObjects.Farm;
using ProjectForFarmers.Application.DataTransferObjects;

namespace ProjectForFarmers.Application.Services.Business
{
    public class AccountService : Service, IAccountService
    {
        public AccountService(IMapper mapper, IApplicationDbContext dbContext, IConfiguration configuration) : base(mapper, dbContext, configuration)
        {
        }
    }
}
