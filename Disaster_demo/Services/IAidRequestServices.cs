using Disaster_demo.Models.Entities;

namespace Disaster_demo.Services
{
    public interface IAidRequestServices
    {
        Task<bool> CreateAidRequestAsync(AidRequests request);
        Task<List<AidRequests>> GetDsApprovedAidRequests();
        Task<List<AidRequests>> GetPendingEmergencyAidRequestsAsync();
        Task<List<AidRequests>> GetPendingPostDisasterAidRequestsAsync(string divisionalSecretariat);
        bool UpdateStatus(StatusUpdateModel model);
    }
}
