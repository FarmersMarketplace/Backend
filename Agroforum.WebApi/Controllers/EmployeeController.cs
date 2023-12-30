using Agroforum.Application.DataTransferObjects.Employee;
using Agroforum.Application.Services.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Agroforum.WebApi.Controllers
{
    //[ApiController]
    //[Authorize(Roles = "User")]
    //[Route("api/[controller]/[action]")]
    //public class EmployeeController : ControllerBase
    //{
    //    private IEmployeeService EmployeeService;

    //    public EmployeeController(IEmployeeService employeeService)
    //    {
    //        EmployeeService = employeeService;
    //    }

    //    [HttpPut]
    //    public async Task<IActionResult> Add([FromBody] AddPermissionsDto addPermissionsDto)
    //    {
    //        await EmployeeService.Add(addPermissionsDto);
    //        return NoContent();
    //    }

    //    [HttpPost]
    //    public async Task<IActionResult> Update([FromBody] UpdatePermissionsDto updatePermissionsDto)
    //    {
    //        await EmployeeService.Update(updatePermissionsDto);
    //        return NoContent();
    //    }

    //    [HttpDelete("{permissionsId}")]
    //    public async Task<IActionResult> Delete([FromRoute] Guid permissionsId)
    //    {
    //        await EmployeeService.Delete(permissionsId);
    //        return NoContent();
    //    }

    //    [HttpGet("permissionsId")]
    //    public async Task<IActionResult> Get([FromRoute] Guid permissionsId)
    //    {
    //        var result = await EmployeeService.Get(permissionsId);
    //        return Ok(result);
    //    }

    //    [HttpGet("{farmId}")]
    //    public async Task<IActionResult> GetAll([FromRoute] Guid farmId)
    //    {
    //        var result = await EmployeeService.GetAll(farmId);
    //        return Ok(result);
    //    }

    //    [HttpGet("{identifier}")]
    //    public async Task<IActionResult> Search([FromRoute] string identifier)
    //    {
    //        var result = await EmployeeService.Search(identifier);
    //        return Ok(result);
    //    }

    //    [HttpPost]
    //    [AllowAnonymous]
    //    public async Task<IActionResult> Confirm()
    //    {
    //        var permissionId = Guid.Parse(User.FindFirst("permissionId")?.Value);
    //        var accountId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    //        await EmployeeService.Confirm(permissionId, accountId);
    //        return NoContent();
    //    }
    //}
}
