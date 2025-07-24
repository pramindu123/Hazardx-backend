using Disaster_demo.Models.Entities;
using Disaster_demo.Services;
using Microsoft.AspNetCore.Mvc;

namespace Disaster_demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContributionController : ControllerBase
    {
        private readonly IContributionService _contributionService;

        public ContributionController(IContributionService contributionServices)
        {
            this._contributionService = contributionServices;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddContribution([FromBody] ContributionDTO contribution)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _contributionService.AddContributionAsync(contribution);
            return result
                ? Ok(new { message = "Contribution added successfully" })
                : StatusCode(500, "Failed to add contribution");
        }



        [HttpGet("volunteer/{volunteerId}")]
        public async Task<IActionResult> GetByVolunteerId(int volunteerId)
        {
            var contributions = await _contributionService.GetContributionsByVolunteerIdAsync(volunteerId);
            return Ok(contributions);
        }




        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingContributions([FromQuery] string divisional_secretariat)
        {
            var pending = await _contributionService.GetPendingContributionsAsync(divisional_secretariat);
            return Ok(pending);
        }




        [HttpPost("approve/{id}")]
        public async Task<IActionResult> ApproveContribution(int id)
        {
            var result = await _contributionService.ApproveContributionAsync(id);
            return result ? Ok(new { message = "Contribution approved" }) : NotFound(new { message = "Contribution not found" });
        }

        [HttpPost("reject/{id}")]
        public async Task<IActionResult> RejectContribution(int id)
        {
            var result = await _contributionService.RejectContributionAsync(id);
            return result ? Ok(new { message = "Contribution rejected" }) : NotFound(new { message = "Contribution not found" });
        }




        [HttpGet("total/{volunteerId}")]
        public async Task<IActionResult> GetTotalContributions(int volunteerId)
        {
            var total = await _contributionService.GetTotalContributionsCountAsync(volunteerId);
            return Ok(new { totalContributions = total });
        }

        [HttpGet("latest/{volunteerId}")]
        public async Task<IActionResult> GetLatestContribution(int volunteerId)
        {
            var latest = await _contributionService.GetLatestContributionAsync(volunteerId);
            if (latest == null)
                return NotFound("No contributions found.");
            return Ok(latest);
        }

    }
}