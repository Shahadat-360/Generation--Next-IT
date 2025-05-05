using System.ComponentModel.DataAnnotations;

namespace MeetingMinutes.Models
{
    public class ProductService
    {
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Product/Service Name")]
        public string Name { get; set; }
        
        public string? Unit { get; set; }
        
        [Display(Name = "Description")]
        public string? Description { get; set; }
    }
} 