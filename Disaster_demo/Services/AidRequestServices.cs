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


        //public async Task<List<AidRequests>> getPendingAidRequests(string divisionalSecretariat)
        //{
        //    divisionalSecretariat = divisionalSecretariat.Trim();

        //    var pending = await _dbContext.AidRequests
        //        .Where(s => s.dsApprove == DsApprovalStatus.Pending && s.divisional_secretariat == divisionalSecretariat)
        //        .OrderByDescending(s => s.date_time)
        //        .ToListAsync();

        //    return pending;
        //}

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
            //else if (string.Equals(model.Actor, "DMC", StringComparison.OrdinalIgnoreCase))
            //{
            //    if (Enum.TryParse<DmcApprovalStatus>(model.Status, true, out var parsedDmcStatus))
            //    {
            //        aidRequest.dmcApprove = parsedDmcStatus;


            //        if (parsedDmcStatus == DmcApprovalStatus.Approved)
            //        {
            //            var dsOfficer = _dbContext.DS_Officers
            //                .FirstOrDefault(g => g.divisional_secretariat.ToLower() == aidRequest.divisional_secretariat.ToLower());

            //            if (dsOfficer != null)
            //            {
            //                aidRequest.assign_ds = dsOfficer.user_id;
            //            }
            //        }

            //        _dbContext.SaveChanges();
            //        return true;
            //    }
            //}

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




        //public async Task<List<AidRequests>> GetAidRequestsByDistrict(string district)
        //{
        //    var requests = await _dbContext.AidRequests
        //        .Where(a => a.district.ToLower() == district.ToLower()
        //                    && a.dsApprove == DsApprovalStatus.Approved
        //                    && a.dmcApprove == DmcApprovalStatus.Pending)
        //        .OrderByDescending(a => a.date_time)
        //        .ToListAsync();

        //    return requests;
        //}



        //public async Task<List<AidRequests>> GetDmcApprovedAidRequests()
        //{
        //    var approvedRequests = await _dbContext.AidRequests

        //        .Where(a => a.dsApprove == DsApprovalStatus.Approved && a.dmcApprove == DmcApprovalStatus.Approved)
        //        .OrderByDescending(a => a.date_time)
        //        .ToListAsync();

        //    return approvedRequests;
        //}



        //public async Task<List<AidRequests>> GetAllDmcRelatedAidRequests(string district)
        //{
        //    var result = await _dbContext.AidRequests
        //        .Where(a =>
        //            a.district.ToLower() == district.ToLower() &&
        //            a.dsApprove == DsApprovalStatus.Approved &&
        //            (a.dmcApprove == DmcApprovalStatus.Pending || a.dmcApprove == DmcApprovalStatus.Approved))
        //        .OrderByDescending(a => a.date_time)
        //        .ToListAsync();

        //    return result;
        //}




    }
}
