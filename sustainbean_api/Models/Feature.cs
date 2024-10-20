namespace sustainbean_api.Models
{
    public class Feature
    {
        public int feature_id { get; set; } // feature_id
        public string? title { get; set; } // title
        public string? alt_text { get; set; } // alt_text
        public string? caption { get; set; } // caption
        public string? description { get; set; } // description
        public bool? is_active { get; set; } // is_active
        public DateTime created_on { get; set; } // created_on
        public string? created_by { get; set; } // created_by
        public DateTime? updated_on { get; set; } // updated_on (nullable)
        public string? updated_by { get; set; } // updated_by (nullable)
    }

}
