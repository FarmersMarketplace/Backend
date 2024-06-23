using FarmersMarketplace.Application.DataTransferObjects.Feedback;
using FarmersMarketplace.Application.Exceptions;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Domain.Feedbacks;
using Microsoft.EntityFrameworkCore;

namespace FarmersMarketplace.Application.Services.Business
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IApplicationDbContext DbContext;
        private readonly IFeedbackSearchSynchronizer FeedbackSearchSynchronizer;

        public FeedbackService(IApplicationDbContext dbcontext, IFeedbackSearchSynchronizer feedbackSearchSynchronizer)
        {
            DbContext = dbcontext;
            FeedbackSearchSynchronizer = feedbackSearchSynchronizer;
        }

        public async Task Create(CreateFeedbackDto dto, Guid customerId)
        {
            var customer = await DbContext.Customers.FirstOrDefaultAsync(c => c.Id == customerId);

            if (customer == null)
            {
                string message = $"Account with Id {customerId} was not found.";
                throw new NotFoundException(message, "AccountNotFound");
            }

            Feedback feedback = new Feedback 
            { 
                CustomerId = customerId, 
                Comment = dto.Comment, 
                Rating = dto.Rating, 
                Date = DateTime.Now
            };

            string reviewedEntityName = string.Empty;
            string reviewedEntityImage = string.Empty;

            if (dto.Type == FeedbackType.Product)
            {
                var product = await DbContext.Products.Include(p => p.Feedbacks).FirstOrDefaultAsync(p => p.Id == dto.ReviewedEntityId);

                if (product == null)
                {
                    string message = $"Product with Id {dto.ReviewedEntityId} was not found.";
                    throw new NotFoundException(message, "ProductNotFound");
                }

                feedback.CollectionId = product.Feedbacks.Id;
                reviewedEntityName = product.Name;
                reviewedEntityImage = product.ImagesNames.Count > 0 ? product.ImagesNames[0] : "";
            }
            else if(dto.Type == FeedbackType.Seller)
            {
                var seller = await DbContext.Sellers.Include(s => s.Feedbacks).FirstOrDefaultAsync(s => s.Id == dto.ReviewedEntityId);

                if (seller == null)
                {
                    string message = $"Seller with Id {dto.ReviewedEntityId} was not found.";
                    throw new NotFoundException(message, "SellerNotFound");
                }

                feedback.CollectionId = seller.Feedbacks.Id;
                reviewedEntityName = seller.Name + " " + seller.Surname;
                reviewedEntityImage = seller.ImagesNames.Count > 0 ? seller.ImagesNames[0] : "";
            }
            else if(dto.Type == FeedbackType.Farm)
            {
                var farm = await DbContext.Farms.Include(f => f.Feedbacks).FirstOrDefaultAsync(f => f.Id == dto.ReviewedEntityId);

                if (farm == null)
                {
                    string message = $"Farm with Id {dto.ReviewedEntityId} was not found.";
                    throw new NotFoundException(message, "FarmNotFound");
                }

                feedback.CollectionId = farm.Feedbacks.Id;
                reviewedEntityName = farm.Name;
                reviewedEntityImage = farm.ImagesNames.Count > 0 ? farm.ImagesNames[0] : "";
            }
            else
            {
                throw new NotImplementedException("Feedback type not implemented.");
            }

            await DbContext.Feedbacks.AddAsync(feedback);
            await DbContext.SaveChangesAsync();
            await FeedbackSearchSynchronizer.Create(feedback, dto.ReviewedEntityId, reviewedEntityName, dto.Type, reviewedEntityImage);
        }

        public async Task Delete(Guid id, Guid customerId)
        {
            var feedback = await DbContext.Feedbacks.FirstOrDefaultAsync(f => f.Id == id);

            if (feedback == null)
            {
                string message = $"Feedback with Id {id} was not found.";
                throw new NotFoundException(message, "FeedbackNotFound");
            }
            else if(feedback.CustomerId != customerId)
            {
                string message = $"Access for customer with Id {customerId} to feedback with Id {feedback.Id} denied: Permission denied to modify data.";
                throw new AuthorizationException(message, "AccessDenied");
            }

            DbContext.Feedbacks.Remove(feedback);
            await DbContext.SaveChangesAsync();
            await FeedbackSearchSynchronizer.Delete(id);
        }

        public async Task Update(UpdateFeedbackDto dto, Guid customerId)
        {
            var feedback = await DbContext.Feedbacks.FirstOrDefaultAsync(f => f.Id == dto.Id);

            if (feedback.CustomerId != customerId)
            {
                string message = $"Access for customer with Id {customerId} to feedback with Id {feedback.Id} denied: Permission denied to modify data.";
                throw new AuthorizationException(message, "AccessDenied");
            }

            feedback.Comment = dto.Comment;
            feedback.Rating = dto.Rating;
            feedback.Date = DateTime.Now;

            DbContext.SaveChangesAsync();
            await FeedbackSearchSynchronizer.Update(feedback);
        }
    }
}
