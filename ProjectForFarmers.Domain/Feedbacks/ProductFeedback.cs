using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Domain.Feedbacks
{
    public class ProductFeedback : Feedback
    {
        public Guid ProductId { get; set; }
    }

}
