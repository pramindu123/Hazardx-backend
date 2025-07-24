using Disaster_demo.Models.Entities;

namespace Disaster_demo.Services
{
    public interface IAidRequestServices
    {
        Task<bool> CreateAidRequestAsync(AidRequests request);
        Task<int> GetContributionCountAsync(int aidId);
        Task<List<AidRequests>> GetDeliveredAidRequestsAsync();
        Task<List<AidRequests>> GetDsApprovedAidRequests();
        Task<(List<AidRequests> dsApprovedPostDisaster, List<AidRequests> emergency)> GetDsApprovedPostDisasterAndEmergencyAidRequestsByDistrictAsync(string district, bool? isFulfilled = null);
        Task<List<AidRequests>> GetOngoingAidRequestsAsync(string? divisionalSecretariat = null);
        Task<List<AidRequests>> GetPendingEmergencyAidRequestsAsync();
        Task<List<AidRequests>> GetPendingPostDisasterAidRequestsAsync(string divisionalSecretariat);
        Task<int> GetPendingPostDisasterAidRequestsCountAsync(string divisionalSecretariat);
        Task<bool> MarkAidRequestAsResolvedAsync(int aidId);
        bool UpdateStatus(StatusUpdateModel model);
    }
}