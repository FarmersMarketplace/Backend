using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.Filters
{
    internal interface IFilter<T>
    {
        Task<IQueryable<T>> ApplyFilter(IQueryable<T> query);
    }
}
