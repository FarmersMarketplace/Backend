using FarmersMarketplace.Application.DataTransferObjects.Feedback;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Application.Services.Business;
using FarmersMarketplace.Application.ViewModels.Feedback;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FarmersMarketplace.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService FeedbackService;
        private readonly ISearchProvider<GetReviewedEntityFeedbackListDto, ReviewedEntityFeedbackListVm, object> ReviewedEntityFeedbackSearchProivder;
        private readonly ISearchProvider<GetCustomerFeedbackListDto, CustomerFeedbackListVm, object> CustomerFeedbackSearchProvider;
        private Guid AccountId => Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        public FeedbackController(IFeedbackService feedbackService, ISearchProvider<GetReviewedEntityFeedbackListDto, ReviewedEntityFeedbackListVm, object> reviewedEntityFeedbackSearchProivder, ISearchProvider<GetCustomerFeedbackListDto, CustomerFeedbackListVm, object> customerFeedbackSearchProvider)
        {
            FeedbackService = feedbackService;
            ReviewedEntityFeedbackSearchProivder = reviewedEntityFeedbackSearchProivder;
            CustomerFeedbackSearchProvider = customerFeedbackSearchProvider;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ReviewedEntityFeedbackListVm), 200)]
        public async Task<IActionResult> GetAllForReviewedEntity([FromQuery] GetReviewedEntityFeedbackListDto dto)
        {
            var vm = await ReviewedEntityFeedbackSearchProivder.Search(dto);
            return Ok(vm);
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(typeof(CustomerFeedbackListVm), 200)]
        public async Task<IActionResult> GetAllForCustomer([FromQuery] GetCustomerFeedbackListDto dto)
        {
            var vm = await CustomerFeedbackSearchProvider.Search(dto);
            return Ok(vm);
        }


        [HttpPost]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Create([FromBody] CreateFeedbackDto dto)
        {
            await FeedbackService.Create(dto, AccountId);
            return NoContent();
        }

        [HttpDelete]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            await FeedbackService.Delete(id, AccountId);
            return NoContent();
        }

        [HttpPut]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Update([FromBody] UpdateFeedbackDto dto)
        {
            await FeedbackService.Update(dto, AccountId);
            return NoContent();
        }
    }
}
