using FarmersMarketplace.Application.Exceptions;
using FarmersMarketplace.Application.Helpers;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Domain;

namespace FarmersMarketplace.Application.Services.Business
{
    public class ValidateService
    {
        protected readonly IApplicationDbContext DbContext;

        public ValidateService(IApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public void ValidateProducer(Guid accountId, Guid producerId, Producer producer)
        {
            if (producer == Producer.Seller)
            {
                if (producerId != accountId)
                {
                    string message = $"Access for account with Id {accountId} to producer {producer.ToString()} with Id {producerId} denied: Permission denied to modify data.";
                    throw new AuthorizationException(message, "AccessDenied");
                }
            }
            else if (producer == Producer.Farm)
            {
                var farm = DbContext.Farms.FirstOrDefault(f => f.Id == producerId);
                if (farm == null)
                {
                    string message = $"Farm with Id {producerId} was not found.";
                    throw new NotFoundException(message, "FarmNotFound", producerId.ToString());
                }
                if (farm.OwnerId != accountId)
                {
                    string message = $"Access for account with Id {accountId} to producer {producer.ToString()} with Id {producerId} denied: Permission denied to modify data.";
                    throw new AuthorizationException(message, "AccessDenied");
                }
            }
            else
            {
                throw new Exception("Producer is not validated.");
            }
        }
    }

}
