using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.ViewModels.Farm
{
    public class FarmLogVm
    {
        public string Message { get; set; }
        public DateTime Date { get; set; }

        public FarmLogVm(string message, DateTime date)
        {
            Message = message;
            Date = date;
        }
    }

}
