using AutoMapper;
using Microsoft.Extensions.Configuration;
using FarmersMarketplace.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.Services
{
    public abstract class Service
    {
        protected readonly IMapper Mapper;
        protected readonly IApplicationDbContext DbContext;
        protected readonly IConfiguration Configuration;
        
        protected Service(IMapper mapper, IApplicationDbContext dbContext, IConfiguration configuration)
        {
            Mapper = mapper;
            DbContext = dbContext;
            Configuration = configuration;
        }
    }

}
