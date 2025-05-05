using System.ComponentModel.DataAnnotations;

namespace MeetingMinutes.Models
{
    public class CorporateCustomer
    {
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Corporate Name")]
        public string Name { get; set; }
    }

    public class IndividualCustomer
    {
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Customer Name")]
        public string Name { get; set; }
    }
} 