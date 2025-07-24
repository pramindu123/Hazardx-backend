using Disaster_demo.Models;
using Disaster_demo.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Disaster_demo.Services;
using System.Numerics;

namespace Disaster_demo.Services
{
    public class AidRequestServices : IAidRequestServices
    {
        private readonly DisasterDBContext _dbContext;

        public AidRequestServices(DisasterDBContext dbContext)
        {
            this._dbContext = dbContext;
        }




        public async Task<List<AidRequests>> GetPendingPostDisasterAidRequestsAsync(string divisionalSecretariat)
        {
            divisionalSecretariat = divisionalSecretariat.Trim();

            var pending = await _dbContext.AidRequests
                .Where(s => s.dsApprove == DsApprovalStatus.Pending
                            && s.request_type == AidRequestType.PostDisaster
                            && s.divisional_secretariat == divisionalSecretariat)
                .OrderByDescending(s => s.date_time)
                .ToListAsync();

            return pending;
        }


        public async Task<List<AidRequests>> GetPendingEmergencyAidRequestsAsync()
        {
            var pendingEmergency = await _dbContext.AidRequests
                .Where(s => s.request_type == AidRequestType.Emergency)
                .OrderByDescending(s => s.date_time)
                .ToListAsync();

            return pendingEmergency;
        }







        public async Task<bool> CreateAidRequestAsync(AidRequests request)
        {
            if (!Enum.IsDefined(typeof(AidRequestType), request.request_type))
            {
                return false;  // Invalid enum value
            }

            request.date_time = DateTime.UtcNow;

            _dbContext.AidRequests.Add(request);
            await _dbContext.SaveChangesAsync();
            return true;
        }



        public bool UpdateStatus(StatusUpdateModel model)
        {
            var aidRequest = _dbContext.AidRequests.FirstOrDefault(a => a.aid_id == model.ReportId);
            if (aidRequest == null)
                return false;

            if (string.Equals(model.Actor, "DS", StringComparison.OrdinalIgnoreCase))
            {
                if (Enum.TryParse<DsApprovalStatus>(model.Status, true, out var parsedGnStatus))
                {
                    aidRequest.dsApprove = parsedGnStatus;
                    _dbContext.SaveChanges();
                    return true;
                }
            }


            return false;
        }

        public async Task<List<AidRequests>> GetDsApprovedAidRequests()
        {
            var approvedRequests = await _dbContext.AidRequests
                .Where(a => a.dsApprove == DsApprovalStatus.Approved)
                .OrderByDescending(a => a.date_time)
                .ToListAsync();

            return approvedRequests;
        }



        public async Task<List<AidRequests>> GetOngoingAidRequestsAsync(string? divisionalSecretariat = null)
        {
            var query = _dbContext.AidRequests.AsQueryable();

            if (!string.IsNullOrWhiteSpace(divisionalSecretariat))
            {
                divisionalSecretariat = divisionalSecretariat.Trim();
                query = query.Where(a => a.divisional_secretariat == divisionalSecretariat);
            }

            return await query
                .Where(a =>
                    (a.request_type == AidRequestType.Emergency ||
                     (a.request_type == AidRequestType.PostDisaster && a.dsApprove == DsApprovalStatus.Approved))
                    && !a.IsFulfilled)
                .OrderByDescending(a => a.date_time)
                .ToListAsync();
        }


        public async Task<bool> MarkAidRequestAsResolvedAsync(int aidId)
        {
            var aidRequest = await _dbContext.AidRequests.FirstOrDefaultAsync(a => a.aid_id == aidId);
            if (aidRequest == null) return false;

            aidRequest.IsFulfilled = true;
            await _dbContext.SaveChangesAsync();
            return true;
        }




        public async Task<int> GetContributionCountAsync(int aidId)
        {
            return await _dbContext.Contribution
                .Where(c => c.aid_id == aidId)
                .CountAsync();
        }

        public async Task<List<AidRequests>> GetDeliveredAidRequestsAsync()
        {
            return await _dbContext.AidRequests
                .Where(a => a.IsFulfilled == true)
                .OrderByDescending(a => a.date_time)
                .ToListAsync();
        }

        public async Task<int> GetPendingPostDisasterAidRequestsCountAsync(string divisionalSecretariat)
        {
            divisionalSecretariat = divisionalSecretariat.Trim();

            return await _dbContext.AidRequests
                .Where(s => s.dsApprove == DsApprovalStatus.Pending
                            && s.request_type == AidRequestType.PostDisaster
                            && s.divisional_secretariat == divisionalSecretariat)
                .CountAsync();
        }




        public async Task<(List<AidRequests> dsApprovedPostDisaster, List<AidRequests> emergency)>
    GetDsApprovedPostDisasterAndEmergencyAidRequestsByDistrictAsync(string district, bool? isFulfilled = null)
        {
            district = district.Trim();

            var dsQuery = _dbContext.AidRequests
                .Where(a =>
                    a.dsApprove == DsApprovalStatus.Approved &&
                    a.request_type == AidRequestType.PostDisaster &&
                    a.district == district);

            var emergencyQuery = _dbContext.AidRequests
                .Where(a =>
                    a.request_type == AidRequestType.Emergency &&
                    a.district == district);

            // Apply filter if isFulfilled parameter is provided
            if (isFulfilled.HasValue)
            {
                dsQuery = dsQuery.Where(a => a.IsFulfilled == isFulfilled.Value);
                emergencyQuery = emergencyQuery.Where(a => a.IsFulfilled == isFulfilled.Value);
            }

            var dsApprovedPostDisaster = await dsQuery
                .OrderByDescending(a => a.date_time)
                .ToListAsync();

            var emergency = await emergencyQuery
                .OrderByDescending(a => a.date_time)
                .ToListAsync();

            return (dsApprovedPostDisaster, emergency);
        }






    }
}