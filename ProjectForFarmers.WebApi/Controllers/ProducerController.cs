using FarmersMarketplace.Application.DataTransferObjects.Producers;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Application.ViewModels.Producers;
using Microsoft.AspNetCore.Mvc;

namespace FarmersMarketplace.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProducerController : ControllerBase
    {
        private readonly ISearchProvider<GetProducerMarkersDto, ProducerMarkerListVm, ProducerAutocompleteDto> MapSearchProvider;
        private readonly ISearchProvider<GetProducerListDto, ProducerListVm, ProducerAutocompleteDto> ProducerSearchProvider;

        public ProducerController(ISearchProvider<GetProducerMarkersDto, ProducerMarkerListVm, ProducerAutocompleteDto> mapSearchProvider, ISearchProvider<GetProducerListDto, ProducerListVm, ProducerAutocompleteDto> producerSearchProvider)
        {
            MapSearchProvider = mapSearchProvider;
            ProducerSearchProvider = producerSearchProvider;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ProducerListVm), 200)]
        public async Task<IActionResult> GetAll([FromQuery] GetProducerListDto dto)
        {
            var vm = await ProducerSearchProvider.Search(dto);
            return Ok(vm);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ProducerMarkerListVm), 200)]
        public async Task<IActionResult> GetMarkers([FromQuery] GetProducerMarkersDto dto)
        {
            var vm = await MapSearchProvider.Search(dto);
            return Ok(vm);
        }
    }
}
