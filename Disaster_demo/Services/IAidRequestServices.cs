using Disaster_demo.Models.Entities;

namespace Disaster_demo.Services
{
    public interface IAidRequestServices
    {
        Task<bool> CreateAidRequestAsync(AidRequests request);
        Task<int> GetContributionCountAsync(int aidId);
        Task<List<AidRequests>> GetDsApprovedAidRequests();
        Task<List<AidRequests>> GetOngoingAidRequestsAsync(string divisionalSecretariat);
        Task<bool> MarkAidRequestAsResolvedAsync(int aidId);
        Task<List<AidRequests>> GetPendingEmergencyAidRequestsAsync();
        Task<List<AidRequests>> GetPendingPostDisasterAidRequestsAsync(string divisionalSecretariat);
        Task<List<AidRequests>> GetDeliveredAidRequestsAsync();
        bool UpdateStatus(StatusUpdateModel model);
    }
}