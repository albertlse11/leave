using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveApplication.Models
{
    public class LeaveResult
    {
        // This class is use to populate the data in order to display on Leave/Index page's Grid result.

        public int ID { get; set; }

        [Display(Name = "Leave Date"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LeaveDate { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Display(Name = "Leave Type")]
        public string LTypeName { get; set; }

        [Display(Name = "Reason")]
        public string ReasonDesc { get; set; }
    }
}