using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveApplication.Models
{
    public class Employee : Auditable
    {
        public int ID { get; set; }

        [Display(Name = "First Name")]
        [Required]
        [StringLength(200)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        [StringLength(200)]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        [Required]
        [StringLength(200)]
        public string Email { get; set; }
    }
}