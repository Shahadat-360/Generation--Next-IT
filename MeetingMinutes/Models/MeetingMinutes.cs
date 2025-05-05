using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MeetingMinutes.Models
{
    public class MeetingMinutesMaster
    {
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Customer Type")]
        public string CustomerType { get; set; } // "Corporate" or "Individual"
        
        [Required]
        [Display(Name = "Customer Name")]
        public int CustomerId { get; set; }
        
        public string CustomerName { get; set; } // For display purposes
        
        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        
        [Required]
        [Display(Name = "Time")]
        [DataType(DataType.Time)]
        public TimeSpan Time { get; set; }
        
        [Required]
        [Display(Name = "Meeting Place")]
        public string MeetingPlace { get; set; }
        
        [Display(Name = "Attends From Client Side")]
        public string? AttendsFromClientSide { get; set; }
        
        [Display(Name = "Attends From Host Side")]
        public string? AttendsFromHostSide { get; set; }
        
        [Required]
        [Display(Name = "Meeting Agenda")]
        public string MeetingAgenda { get; set; }
        
        [Display(Name = "Meeting Discussion")]
        public string? MeetingDiscussion { get; set; }
        
        [Display(Name = "Meeting Decision")]
        public string? MeetingDecision { get; set; }
        
        public List<MeetingMinutesDetail> Details { get; set; } = new List<MeetingMinutesDetail>();
    }

    public class MeetingMinutesDetail
    {
        public int Id { get; set; }
        
        public int MeetingMinutesMasterId { get; set; }
        
        public int ProductServiceId { get; set; }
        
        [Display(Name = "Product/Service Name")]
        public string ProductServiceName { get; set; }
        
        [Required]
        [Display(Name = "Quantity")]
        public decimal Quantity { get; set; }
        
        [Display(Name = "Unit")]
        public string? Unit { get; set; }
    }
} 