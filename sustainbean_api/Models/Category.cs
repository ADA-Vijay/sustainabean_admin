namespace sustainbean_api.Models
{
    public class Category
    {
        public int category_id { get; set; } // category_id
        public string? category { get; set; } // category
        public string? slug { get; set; } // slug
        public string? parent_category { get; set; } // parent_category
        public string? description { get; set; } // description
        public bool? is_active { get; set; } // is_active
        public DateTime created_on { get; set; } // created_on
        public string? created_by { get; set; } // created_by
        public DateTime? updated_on { get; set; } // updated_on (nullable)
        public string? updated_by { get; set; } // updated_by (nullable)
    }

}
