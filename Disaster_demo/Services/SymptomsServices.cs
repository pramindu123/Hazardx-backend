using Disaster_demo.Models;
using Disaster_demo.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Disaster_demo.Services;
using System.Numerics;

namespace Disaster_demo.Services
{
    public class SymptomsServices : ISymptomsServices
    {
        private readonly DisasterDBContext _dbContext;

        public SymptomsServices(DisasterDBContext dbContext)
        {
            this._dbContext = dbContext;
        }



        //public async Task<List<Symptoms>> GetPendingReportsByDivisionAsync(string divisionalSecretariat)
        //{
        //    return await _dbContext.Symptoms
        //        .Where(r => r.action == "Pending" && r.divisional_secretariat == divisionalSecretariat)
        //        .ToListAsync();
        //}

        public async Task<List<Symptoms>> GetPendingReportsByDistrictAsync(string district)
        {
            return await _dbContext.Symptoms
                .Where(r => r.action == "Pending" && r.district.Trim().ToLower() == district.Trim().ToLower())
                .ToListAsync();
        }





        public async Task createSymptoms(Symptoms symptoms)
        {
            this._dbContext.Symptoms.Add(symptoms);
            await this._dbContext.SaveChangesAsync();


        }




        public List<Symptoms> GetPendingSymptomsByDivisionalSecretariat(string divisionalSecretariat)
        {
            return _dbContext.Symptoms
                .Where(s => s.action == "Pending" && s.divisional_secretariat == divisionalSecretariat)
                .OrderByDescending(s => s.date_time)
                .ToList();
        }









        public bool UpdateSymptomStatus(int reportId, string status)
        {
            Console.WriteLine($"Updating Report ID {reportId} to status: {status}");

            var symptom = _dbContext.Symptoms.FirstOrDefault(s => s.report_id == reportId);

            if (symptom == null)
            {
                Console.WriteLine("Symptom not found.");
                return false;
            }

            symptom.action = status;
            Console.WriteLine($"Before Save: {symptom.action}");

            _dbContext.SaveChanges();  // Make sure this is hit

            Console.WriteLine("SaveChanges called");
            return true;
        }

        


        public async Task<List<Symptoms>> GetDsApprovedSymptomsByDistrictAsync(string district)
        {
            return await _dbContext.Symptoms
                .Where(s => s.action == "Approved" && s.district == district)
                .ToListAsync();
        }



    }
}

 
