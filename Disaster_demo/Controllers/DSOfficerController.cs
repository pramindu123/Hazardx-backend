using Disaster_demo.Services;
using Microsoft.AspNetCore.Mvc;

namespace Disaster_demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DSOfficerController : ControllerBase
    {
        private readonly IDSOfficerServices _dsofficerServices;

        public DSOfficerController(IDSOfficerServices DsofficerServices)
        {
            this._dsofficerServices = DsofficerServices;
        }

        [HttpGet("details/{userId}")]
        public async Task<IActionResult> GetDsOfficerDetails(string userId)
        {
            var officerDetails = await _dsofficerServices.GetDsOfficerDetailsAsync(userId);

            if (officerDetails == null)
                return NotFound();

            return Ok(officerDetails);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllDsOfficers()
        {
            var officers = await _dsofficerServices.GetAllDsOfficersAsync();
            return Ok(officers);
        }
    }
}