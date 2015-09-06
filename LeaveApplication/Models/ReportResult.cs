using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveApplication.Models
{
    public class ReportResult
    {
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Display(Name = "Leave Type")]
        public string LTypName { get; set; }

        [Display(Name = "Number of Days")]
        public int NoOfDays { get; set; }
    }
}