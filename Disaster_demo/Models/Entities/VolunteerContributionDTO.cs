namespace Disaster_demo.Models.Entities
{
    public class VolunteerContributionDTO
    {
        public int contribution_id { get; set; }
        public int volunteer_id { get; set; }
        public string volunteer_name { get; set; }
        public string volunteer_contact { get; set; }
        public string district { get; set; }
        public string VolunteerTypeSupport { get; set; }
        public string AidRequestTypeSupport { get; set; }
        public string description { get; set; }
        public string? image { get; set; }
        public string status { get; set; }
        public string requester_nic { get; set; }
    }
}