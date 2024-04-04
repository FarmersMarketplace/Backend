using FarmersMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.DataTransferObjects.Product
{
    public class ProducerProductAutocompleteDto
    {
        public Producer Producer { get; set; }
        public Guid ProducerId { get; set; }
        public string Query { get; set; }
        public int Count { get; set; }
    }

}
