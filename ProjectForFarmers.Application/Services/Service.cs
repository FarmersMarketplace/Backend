using AutoMapper;
using Microsoft.Extensions.Configuration;
using ProjectForFarmers.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.Services
{
    public abstract class Service
    {
        protected readonly IMapper Mapper;
        protected readonly IApplicationDbContext DbContext;
        protected IConfiguration Configuration { get; set; }

        protected Service(IMapper mapper, IApplicationDbContext dbContext, IConfiguration configuration)
        {
            Mapper = mapper;
            DbContext = dbContext;
            Configuration = configuration;
        }
    }

}
