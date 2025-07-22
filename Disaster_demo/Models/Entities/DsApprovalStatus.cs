﻿using System.Text.Json.Serialization;

namespace Disaster_demo.Models.Entities
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DsApprovalStatus
    {
        
            Pending,
            Approved,
            Rejected
        
    }
}
