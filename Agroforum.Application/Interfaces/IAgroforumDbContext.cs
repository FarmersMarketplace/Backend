using Agroforum.Application.Models;
using Agroforum.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Agroforum.Application.Interfaces
{
    public interface IAgroforumDbContext
    {
        DbSet<Account> Accounts { get; set; }
        DbSet<UnconfirmedPhone> UnconfirmedPhones { get; set; }
        Task<int> SaveChangesAsync(CancellationToken token);
        Task<int> SaveChangesAsync();
    }
}
