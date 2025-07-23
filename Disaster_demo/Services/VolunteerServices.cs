using Disaster_demo.Models;
using Disaster_demo.Models.Entities;
using Disaster_demo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Disaster_demo.Services
{
    public class VolunteerServices : IVolunteerServices
    {
        private readonly DisasterDBContext _dbContext;

        public VolunteerServices(DisasterDBContext dbContext)
        {
            this._dbContext = dbContext;
        }




        public async Task<int> SignupAsync(VolunteerSignupDTO dto)
        {
            if (_dbContext.Users.Any(u => u.email == dto.Email))
            {
                return -1; // Indicate email already registered
            }

            var volunteer = new Volunteer
            {
                email = dto.Email,
                password = dto.Password,
                role = UserRole.volunteer,
                status = UserStatus.active,
                name = dto.Name,
                district = dto.District,
                divisional_secretariat = dto.DivisionalSecretariat,
                contact_number = dto.ContactNumber,
                availability = AvailabilityStatus.Unavailable
            };

            _dbContext.Volunteers.Add(volunteer);
            await _dbContext.SaveChangesAsync();

            return volunteer.user_id;  // Return the generated user_id (assuming auto-increment)
        }




        public async Task<List<Volunteer>> GetVolunteersByDsDivisionAsync(string divisionalSecretariat)
        {
            return await _dbContext.Volunteers
                .Where(v => v.divisional_secretariat == divisionalSecretariat)
                .ToListAsync();
        }


        public async Task<LoginResponseDTO?> GetVolunteerDetailsAsync(string userId)
        {
            if (!int.TryParse(userId, out int parsedUserId))
                return null;

            var volunteer = await _dbContext.Volunteers
                .FirstOrDefaultAsync(v => v.user_id == parsedUserId);

            if (volunteer == null) return null;

            return new LoginResponseDTO
            {
                UserId = volunteer.user_id,
                FullName = volunteer.name,
                DivisionalSecretariat = volunteer.divisional_secretariat,
                Role = "Volunteer",
                Message = "Volunteer details fetched successfully",
                ContactNo = volunteer.contact_number,
                Email = volunteer.email,
                District = volunteer.district,
                Availability = volunteer.availability
            };
        }

        public async Task<List<Volunteer>> GetVolunteersByDistrictAsync(string district)
        {
            return await _dbContext.Volunteers
                .Where(v => v.district == district)
                .ToListAsync();
        }

        public async Task<bool> UpdateAvailabilityAsync(int userId, AvailabilityStatus newStatus)
        {
            var volunteer = await _dbContext.Volunteers.FirstOrDefaultAsync(v => v.user_id == userId);
            if (volunteer == null)
                return false;

            volunteer.availability = newStatus;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<AidRequests>> GetEmergencyAidRequestsAsync()
        {
            return await _dbContext.AidRequests
                .Where(r => r.request_type == AidRequestType.Emergency)
                .ToListAsync();
        }


        public async Task<IEnumerable<AidRequests>> GetNonEmergencyAidRequestsAsync()
        {
            return await _dbContext.AidRequests
                .Where(r => r.dsApprove == DsApprovalStatus.Approved
                            && r.request_type == AidRequestType.PostDisaster
                            && !r.IsFulfilled)
                .ToListAsync();
        }





    }
}