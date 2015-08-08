using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveApplication.Models
{
    public class Report
    {
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Leave Type")]
        public int? LTypeID { get; set; }

        [Display(Name = "Employee Name")]
        public string EmpName { get; set; }

        [Display(Name = "Leave Type")]
        public string LTypName { get; set; }

        [Display(Name = "Number of Days")]
        public int NoOfDays { get; set; }

        public ReportResult[] ReportResults { get; set; }
    }
}