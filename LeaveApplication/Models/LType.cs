using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveApplication.Models
{
    public class LType : Auditable
    {
        public int ID { get; set; }

        [Display(Name = "Leave Type")]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}