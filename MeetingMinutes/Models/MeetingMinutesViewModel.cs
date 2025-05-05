using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MeetingMinutes.Models
{
    public class MeetingMinutesViewModel
    {
        public MeetingMinutesMaster MeetingMinutesMaster { get; set; }
        public List<MeetingMinutesDetail> MeetingMinutesDetails { get; set; }
        
        // For dropdown lists
        public List<SelectListItem> CorporateCustomers { get; set; }
        public List<SelectListItem> IndividualCustomers { get; set; }
        public List<SelectListItem> ProductServices { get; set; }
        
        // For adding a new product/service
        public int SelectedProductServiceId { get; set; }
        public decimal ProductServiceQuantity { get; set; }
        public string ProductServiceUnit { get; set; }
        
        public MeetingMinutesViewModel()
        {
            MeetingMinutesMaster = new MeetingMinutesMaster
            {
                Date = DateTime.Today,
                Time = new TimeSpan(9, 0, 0) // Default 9:00 AM
            };
            MeetingMinutesDetails = new List<MeetingMinutesDetail>();
            CorporateCustomers = new List<SelectListItem>();
            IndividualCustomers = new List<SelectListItem>();
            ProductServices = new List<SelectListItem>();
        }
    }
} 