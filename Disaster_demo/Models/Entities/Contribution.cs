using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Disaster_demo.Models.Entities
{
    [Table("contributions")]
    public class Contribution
    {
        [Key]
        public int contribution_id { get; set; }

        public int volunteer_id { get; set; }

        public int aid_id { get; set; }   // ✅ NEW: link to AidRequests

        public string district { get; set; }

        public string type_support { get; set; }

        public string description { get; set; }
        public string? image { get; set; }

        public DateTime created_at { get; set; } = DateTime.UtcNow;

        public string status { get; set; } = "Pending";  // ✅ NEW: Pending / Verified
    }
}