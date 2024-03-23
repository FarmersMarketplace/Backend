﻿using FarmersMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.DataTransferObjects.Product
{
    public class ProductAutocompleteDto
    {
        public Guid ProducerId { get; set; }
        public Producer Producer { get; set; }
        public int Count { get; set; }
        public string Query { get; set; }
    }

}