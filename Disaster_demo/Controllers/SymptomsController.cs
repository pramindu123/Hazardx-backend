using Disaster_demo.Models.Entities;
using Disaster_demo.Services;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace Disaster_demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SymptomsController : ControllerBase
    {
        private readonly ISymptomsServices _symptomServices;

        public SymptomsController(ISymptomsServices SymptomsService)
        {
            this._symptomServices = SymptomsService;
        }

        
        [HttpPost("create")]
        public async Task<IActionResult> CreateSymptoms([FromBody] Symptoms symptoms)
        {
            if (symptoms == null)
            {
                return BadRequest("Symptom data is null.");
            }

            // Log incoming data for debugging
            Console.WriteLine($"Received report: {symptoms.reporter_name}, {symptoms.contact_no}, {symptoms.district}, {symptoms.divisional_secretariat}, {symptoms.date_time}, {symptoms.description}");

            try
            {
                await _symptomServices.createSymptoms(symptoms);
                return Ok("Symptom created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }




        //[HttpGet("pendingReports")]
        //public async Task<IActionResult> GetPendingReportsByDivision([FromQuery] string divisionalSecretariat)
        //{
        //    if (string.IsNullOrWhiteSpace(divisionalSecretariat))
        //    {
        //        return BadRequest("Divisional Secretariat is required.");
        //    }
        //    var reports = await this._symptomServices.GetPendingReportsByDivisionAsync(divisionalSecretariat);


        //    return Ok(reports);
        //}

        [HttpGet("pendingReportsByDistrict")]
        public async Task<IActionResult> GetPendingReportsByDistrict([FromQuery] string district)
        {
            if (string.IsNullOrWhiteSpace(district))
            {
                return BadRequest("District is required.");
            }

            var reports = await _symptomServices.GetPendingReportsByDistrictAsync(district);
            return Ok(reports);
        }


        //[HttpGet("pendingByDistrict/{district}")]
        //public async Task<IActionResult> GetPendingReportsByDistrict(string district)
        //{
        //    var reports = await _symptomServices.GetPendingReportsByDistrictAsync(district);
        //    return Ok(reports);
        //}




        [HttpPost("updateStatus")]
        public IActionResult UpdateSymptomStatus([FromBody] StatusUpdateModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Status))
                return BadRequest("Invalid request data");

            var updated = _symptomServices.UpdateSymptomStatus(model.ReportId, model.Status);

            if (updated)
                return Ok(new { success = true, message = "Status updated successfully" });

            return NotFound(new { success = false, message = "Report not found" });
        }

         [HttpGet("approvedByDs/{district}")]
        public async Task<IActionResult> GetDsApprovedSymptomsByDistrict(string district)
        {
            var reports = await _symptomServices.GetDsApprovedSymptomsByDistrictAsync(district);
            return Ok(reports);
        }




    }
}
