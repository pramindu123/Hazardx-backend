using Disaster_demo.Models.Entities;

namespace Disaster_demo.Services
{
    public interface IVolunteerServices
    {
        Task<IEnumerable<AidRequests>> GetEmergencyAidRequestsAsync();
        Task<IEnumerable<AidRequests>> GetNonEmergencyAidRequestsAsync();
        Task<LoginResponseDTO?> GetVolunteerDetailsAsync(string userId);
        Task<List<Volunteer>> GetVolunteersByDistrictAsync(string district);
        Task<List<Volunteer>> GetVolunteersByDsDivisionAsync(string divisionalSecretariat);
        Task<int> SignupAsync(VolunteerSignupDTO dto);
        Task<bool> UpdateAvailabilityAsync(int userId, AvailabilityStatus newStatus);
    }
}
