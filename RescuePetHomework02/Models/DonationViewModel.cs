using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RescuePetHomework02.Models
{
    public class DonationViewModel
    {
        public string SelectedUserName { get; set; }
        public decimal Amount { get; set; }
        public string SelectedNumber { get; set; }
        public SelectList PhoneList { get; set; }
        public IEnumerable<SelectListItem> Users { get; set; }
        public decimal GoalAmount { get; set; } = 750000;
        public decimal TotalRaised { get; set; }
        public string Status { get; set; }
        public bool GoalReached => TotalRaised >= 750000M;
        public decimal Percentage { get; set; }
    }
}