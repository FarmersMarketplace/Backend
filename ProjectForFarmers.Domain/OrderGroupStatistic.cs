using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Domain
{
    public class OrderGroupStatistic
    {
        public Guid Id { get; set; }
        public uint Count { get; set; }
        public float PercentageChange { get; set; }
    }

}
