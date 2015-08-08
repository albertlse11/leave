using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveApplication.Models
{
    public class Leave : Auditable
    {
        public int ID { get; set; }

        [Display(Name = "Leave Date"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime LeaveDate { get; set; }

        [Display(Name = "Employee Name")]
        [Required]
        public int EmployeeID { get; set; }

        [Display(Name = "Leave Type")]
        [Required]
        public int LTypeID { get; set; }

        [Display(Name = "Reason")]
        [Required]
        public int ReasonID { get; set; }
    }
}