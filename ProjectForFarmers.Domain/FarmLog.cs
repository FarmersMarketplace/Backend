using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Domain
{
    public class FarmLog
    {
        public Guid Id { get; set; }
        public Guid FarmId { get; set; }
        public string Message { get; set; }
        public string? PropertyName { get; set; }
        public string[]? Parameters { get; set; }
        public DateTime CreationDate { get; set; }

        public FarmLog(Guid id, Guid farmId, string message, string[]? parameters, DateTime creationDate)
        {
            Id = id;
            FarmId = farmId;
            Message = message;
            Parameters = parameters;
            CreationDate = creationDate;
        }

        public FarmLog()
        {
        }
    }

}
