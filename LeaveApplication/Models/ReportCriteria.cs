using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveApplication.Models
{
    public class ReportCriteria
    {
        [Display(Name = "Start Date")]
        [Required]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [Required]
        public DateTime EndDate { get; set; }

        [Display(Name = "Leave Type")]
        public int? LTypeID { get; set; }
    }
}