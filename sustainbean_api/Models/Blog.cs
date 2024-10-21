using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace sustainbean_api.Models
{
    public class Blog
    {
        public int blog_id { get; set; } // blog_id
        public int? category_id { get; set; } // category_id
        public int? tag_id { get; set; } // tag_id
        public string? slug { get; set; } // slug
        public string? auther { get; set; } // auther
        public string? img_url { get; set; } // img_url
        public string? seo_title { get; set; } // seo_title
        public string? seo_key_word { get; set; } // seo_key_word
        public string? description { get; set; } // description
        public string? html { get; set; } // html
        public bool? is_active { get; set; } // is_active
        public DateTime created_on { get; set; } // created_on
        public string? created_by { get; set; } // created_by
        public DateTime? updated_on { get; set; } // updated_on (nullable)
        public string? updated_by { get; set; } // updated_by (nullable)
        public string? tag_name { get; set; } // updated_by (nullable)
        public string? category { get; set; } // updated_by (nullable)

    }

}
