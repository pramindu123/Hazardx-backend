// Services/ContributionService.cs
using Disaster_demo.Models.Entities;

namespace Disaster_demo.Services
{
    public interface IContributionService
    {
        Task<bool> AddContributionAsync(ContributionDTO dto);
        Task<bool> ApproveContributionAsync(int contributionId);
        Task<List<Contribution>> GetContributionsByVolunteerIdAsync(int volunteerId);
        Task<Contribution> GetLatestContributionAsync(int volunteerId);
        Task<List<VolunteerContributionDTO>> GetPendingContributionsAsync(string divisional_secretariat, string? aidCategory = null);
        Task<int> GetPendingContributionsCountAsync(string divisional_secretariat);
        Task<int> GetTotalContributionsCountAsync(int volunteerId);
        Task<bool> RejectContributionAsync(int contributionId);
    }
}