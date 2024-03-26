using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime? ToUniversalTime(this DateTime? date)
        {
            return date != null ? ((DateTime)date).ToUniversalTime() : null;
        }
    }

}
