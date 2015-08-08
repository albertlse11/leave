using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveApplication.Models
{
    public class Reason : Auditable
    {
        public int ID { get; set; }

        [Display(Name = "Reason")]
        [Required]
        [StringLength(200)]
        public string Desc { get; set; }
    }
}