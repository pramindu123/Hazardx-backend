using Disaster_demo.Models.Entities;

namespace Disaster_demo.Services
{
    public interface IDSOfficerServices
    {
        Task<LoginResponseDTO?> GetDsOfficerDetailsAsync(string userId);
        Task<List<DS_Officer>> GetAllDsOfficersAsync();
    }
}