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
                    string userFacingMessage = CultureHelper.Exception("AccessDenied");

                    throw new AuthorizationException(message, userFacingMessage);
                }
            }
            else if (producer == Producer.Farm)
            {
                var farm = DbContext.Farms.FirstOrDefault(f => f.Id == producerId);
                if (farm == null)
                {
                    string message = $"Farm with Id {producerId} was not found.";
                    string userFacingMessage = CultureHelper.Exception("FarmNotFound", producerId.ToString());

                    throw new NotFoundException(message, userFacingMessage);
                }
                if (farm.OwnerId != accountId)
                {
                    string message = $"Access for account with Id {accountId} to producer {producer.ToString()} with Id {producerId} denied: Permission denied to modify data.";
                    string userFacingMessage = CultureHelper.Exception("AccessDenied");

                    throw new AuthorizationException(message, userFacingMessage);
                }
            }
            else
            {
                throw new Exception("Producer is not validated.");
            }
        }
    }

}
