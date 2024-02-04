using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.Filters
{
    internal interface IFilter<T>
    {
        Task Filter(T collection);
    }
}
