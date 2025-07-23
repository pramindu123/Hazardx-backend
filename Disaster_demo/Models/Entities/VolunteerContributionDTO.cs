namespace Disaster_demo.Models.Entities
{
    public class VolunteerContributionDTO
    {
        public int contribution_id { get; set; }
        public int volunteer_id { get; set; }
        public string volunteer_name { get; set; }
        public string volunteer_contact { get; set; }
        public string district { get; set; }
        public string type_support { get; set; }
        public string description { get; set; }
        public string? image { get; set; }
        public string status { get; set; }
    }
}