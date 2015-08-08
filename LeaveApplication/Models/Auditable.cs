using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApplication.Models
{
    public class Auditable
    {
        [DefaultValue(false)]
        public bool Deleted { get; set; }

        public string UserCreated { get; set; }

        public DateTime? DateCreated { get; set; }

        public string UserModified { get; set; }

        public DateTime? DateModified { get; set; }
    }
}
