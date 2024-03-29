using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Domain.Feedbacks
{
    public class ProducerFeedback : Feedback
    {
        public Producer Producer { get; set; }
        public Guid ProducerId { get; set; }
    }

}
