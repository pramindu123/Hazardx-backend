using Disaster_demo.Models.Entities;

namespace Disaster_demo.Services
{
    public interface ISymptomsServices
    {
        Task createSymptoms(Symptoms symptoms);
        Task<List<Symptoms>> GetDsApprovedSymptomsByDistrictAsync(string district);
        Task<List<Symptoms>> GetPendingReportsByDistrictAsync(string district);


        List<Symptoms> GetPendingSymptomsByDivisionalSecretariat(string divisionalSecretariat);
        bool UpdateSymptomStatus(int reportId, string status);

    }
}