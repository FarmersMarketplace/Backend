﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.Interfaces
{
    public interface ISearchProvider
    {
        Task<CustomerListVm> GetProducts();
    }
}