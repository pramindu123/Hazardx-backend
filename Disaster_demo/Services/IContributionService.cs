// Services/ContributionService.cs
using Disaster_demo.Models.Entities;

namespace Disaster_demo.Services
{
    public interface IContributionService
    {
        Task<bool> AddContributionAsync(ContributionDTO dto);
        Task<bool> ApproveContributionAsync(int contributionId);
        Task<List<Contribution>> GetContributionsByAidIdAsync(int aidId);
        Task<List<Contribution>> GetContributionsByVolunteerIdAsync(int volunteerId);
        Task<Contribution> GetLatestContributionAsync(int volunteerId);
        Task<List<VolunteerContributionDTO>> GetPendingContributionsAsync(string divisional_secretariat);
        Task<int> GetTotalContributionsCountAsync(int volunteerId);
        Task<bool> RejectContributionAsync(int contributionId);
        Task<bool> VerifyContributionAsync(int contributionId);
    }
}