// Services/ContributionService.cs
using Disaster_demo.Models;
using Disaster_demo.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Disaster_demo.Services
{
    public class ContributionService : IContributionService
    {
        private readonly DisasterDBContext _dbContext;

        public ContributionService(DisasterDBContext context)
        {
            _dbContext = context;
        }


        public async Task<bool> AddContributionAsync(ContributionDTO dto)
        {
            var contribution = new Contribution
            {
                volunteer_id = dto.volunteer_id,
                aid_id = dto.aid_id,
                district = dto.district,
                image = dto.image,
                type_support = dto.type_support,
                description = dto.description,
                status = "Pending"
            };

            _dbContext.Contribution.Add(contribution);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<List<Contribution>> GetContributionsByVolunteerIdAsync(int volunteerId)
        {
            return await _dbContext.Contribution
                .Where(c => c.volunteer_id == volunteerId && c.status == "Approved")
                .OrderByDescending(c => c.created_at)
                .ToListAsync();
        }



        //public async Task<List<VolunteerContributionDTO>> GetPendingContributionsAsync(string divisional_secretariat)
        //{
        //    return await _dbContext.Contribution
        //        .Where(c => c.status == "Pending")
        //        .Join(
        //            _dbContext.Volunteers,
        //            c => c.volunteer_id,
        //            v => v.user_id,
        //            (c, v) => new { Contribution = c, Volunteer = v }
        //        )
        //        .Join(
        //            _dbContext.AidRequests,
        //            cv => cv.Contribution.aid_id,
        //            a => a.aid_id,
        //            (cv, a) => new { cv.Contribution, cv.Volunteer, AidRequest = a }
        //        )
        //        .Where(x => x.AidRequest.divisional_secretariat == divisional_secretariat)
        //        .Select(x => new VolunteerContributionDTO
        //        {
        //            contribution_id = x.Contribution.contribution_id,
        //            volunteer_id = x.Volunteer.user_id,
        //            volunteer_name = x.Volunteer.name,
        //            volunteer_contact = x.Volunteer.contact_number,
        //            district = x.Contribution.district,
        //            type_support = x.Contribution.type_support,
        //            description = x.Contribution.description,
        //            image = x.Contribution.image,
        //            status = x.Contribution.status,

        //            // ✅ NEW: get NIC from AidRequest table
        //            requester_nic = x.AidRequest.nic_number
        //        })
        //        .OrderByDescending(x => x.contribution_id)
        //        .ToListAsync();
        //}

        public async Task<List<VolunteerContributionDTO>> GetPendingContributionsAsync(string divisional_secretariat, string? aidCategory = null)
        {
            var query = _dbContext.Contribution
                .Where(c => c.status == "Pending")
                .Join(
                    _dbContext.Volunteers,
                    c => c.volunteer_id,
                    v => v.user_id,
                    (c, v) => new { Contribution = c, Volunteer = v }
                )
                .Join(
                    _dbContext.AidRequests,
                    cv => cv.Contribution.aid_id,
                    a => a.aid_id,
                    (cv, a) => new { cv.Contribution, cv.Volunteer, AidRequest = a }
                )
                .Where(x => x.AidRequest.divisional_secretariat == divisional_secretariat);

            if (!string.IsNullOrEmpty(aidCategory))
            {
                if (Enum.TryParse<AidRequestType>(aidCategory.Replace(" ", ""), ignoreCase: true, out var parsedType))
                {
                    query = query.Where(x => x.AidRequest.request_type == parsedType);
                }
            }

            return await query
                .Select(x => new VolunteerContributionDTO
                {
                    contribution_id = x.Contribution.contribution_id,
                    volunteer_id = x.Volunteer.user_id,
                    volunteer_name = x.Volunteer.name,
                    volunteer_contact = x.Volunteer.contact_number,
                    district = x.Contribution.district,
                    AidRequestTypeSupport = x.AidRequest.type_support,
                    VolunteerTypeSupport = x.Contribution.type_support,
                    description = x.Contribution.description,
                    image = x.Contribution.image,
                    status = x.Contribution.status,
                    requester_nic = x.AidRequest.nic_number
                })
                .OrderByDescending(x => x.contribution_id)
                .ToListAsync();
        }





        public async Task<bool> ApproveContributionAsync(int contributionId)
        {
            var contribution = await _dbContext.Contribution.FindAsync(contributionId);
            if (contribution == null) return false;

            contribution.status = "Approved";
            return await _dbContext.SaveChangesAsync() > 0;
        }


        public async Task<bool> RejectContributionAsync(int contributionId)
        {
            var contribution = await _dbContext.Contribution.FindAsync(contributionId);
            if (contribution == null) return false;

            contribution.status = "Rejected";
            return await _dbContext.SaveChangesAsync() > 0;
        }



        public async Task<int> GetTotalContributionsCountAsync(int volunteerId)
        {
            return await _dbContext.Contribution
                .CountAsync(c => c.volunteer_id == volunteerId && c.status == "Approved");
        }

        public async Task<Contribution> GetLatestContributionAsync(int volunteerId)
        {
            return await _dbContext.Contribution
                .Where(c => c.volunteer_id == volunteerId && c.status == "Approved")
                .OrderByDescending(c => c.created_at)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetPendingContributionsCountAsync(string divisional_secretariat)
        {
            return await _dbContext.Contribution
                .Where(c => c.status == "Pending")
                .Join(
                    _dbContext.AidRequests,
                    c => c.aid_id,
                    a => a.aid_id,
                    (c, a) => new { Contribution = c, AidRequest = a }
                )
                .CountAsync(x => x.AidRequest.divisional_secretariat == divisional_secretariat);
        }

    }

}