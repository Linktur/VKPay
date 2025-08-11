using System.ComponentModel.DataAnnotations;

namespace VkPayTest.Models
{
    public class VkItem
    {
        [Key]
        public string Id { get; set; } = string.Empty;
        
        [Required]
        public decimal Price { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string TitleEn { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(255)]
        public string TitleRu { get; set; } = string.Empty;
        
        public string? DescriptionEn { get; set; }
        
        public string? DescriptionRu { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public string? PhotoUrl { get; set; }
    }
}
