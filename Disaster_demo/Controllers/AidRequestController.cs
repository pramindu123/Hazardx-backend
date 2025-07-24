using Disaster_demo.Models.Entities;
using Disaster_demo.Services;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace Disaster_demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AidRequestController : ControllerBase
    {
        private readonly IAidRequestServices _aidrequestServices;

        public AidRequestController(IAidRequestServices aidRequestServices)
        {
            this._aidrequestServices = (AidRequestServices)aidRequestServices;
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateAidRequest([FromBody] AidRequests request)
        {
            bool result = await _aidrequestServices.CreateAidRequestAsync(request);
            if (!result)
            {
                return BadRequest("Invalid request type.");
            }

            return Ok(new { message = "Aid request submitted!" });
        }




        [HttpGet("pending-post-disaster")]
        public async Task<IActionResult> GetPendingPostDisasterAidRequests([FromQuery] string divisionalSecretariat)
        {
            if (string.IsNullOrWhiteSpace(divisionalSecretariat))
            {
                return BadRequest("Divisional Secretariat is required.");
            }

            var result = await _aidrequestServices.GetPendingPostDisasterAidRequestsAsync(divisionalSecretariat);
            return Ok(result);
        }

        [HttpGet("pending-emergency")]
        public async Task<IActionResult> GetPendingEmergencyAidRequests()
        {
            var result = await _aidrequestServices.GetPendingEmergencyAidRequestsAsync();
            return Ok(result);
        }




        [HttpPost("updateStatus")]
        public IActionResult UpdateGnStatus([FromBody] StatusUpdateModel model)
        {
            var success = _aidrequestServices.UpdateStatus(model);
            if (!success)
                return NotFound(new { message = "Aid request not found." });

            return Ok(new { message = "Status updated successfully." });
        }

        [HttpGet("ds-approved")]
        public async Task<IActionResult> GetDsApprovedAidRequests()
        {
            var result = await _aidrequestServices.GetDsApprovedAidRequests();
            return Ok(result);
        }




        [HttpGet("ongoing")]
        public async Task<List<AidRequests>> GetOngoingAidRequests([FromQuery] string? divisionalSecretariat)
        {
            return await _aidrequestServices.GetOngoingAidRequestsAsync(divisionalSecretariat);
        }


        [HttpPost("resolve/{aidId}")]
        public async Task<IActionResult> MarkAidRequestAsResolved(int aidId)
        {
            var result = await _aidrequestServices.MarkAidRequestAsResolvedAsync(aidId);
            if (!result)
                return NotFound(new { message = "Aid request not found." });

            return Ok(new { message = "Aid request marked as resolved!" });
        }




        [HttpGet("contribution-count/{aidId}")]
        public async Task<IActionResult> GetContributionCount(int aidId)
        {
            var count = await _aidrequestServices.GetContributionCountAsync(aidId);
            return Ok(new { aidId, contributionsReceived = count });
        }


        [HttpGet("delivered")]
        public async Task<IActionResult> GetDeliveredAidRequests()
        {
            var delivered = await _aidrequestServices.GetDeliveredAidRequestsAsync();
            return Ok(delivered);
        }








    }
}