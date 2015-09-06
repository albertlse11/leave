using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveApplication.Models
{
    public class LeaveSearch
    {
        public LeaveCriteria Criteria { get; set; }
        
        //Use for Result column name
        public Leave LeaveResultColName { get; set; }
        
        public LeaveResult[] LeaveResults { get; set; }
    }
}