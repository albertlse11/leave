using LeaveApplication.Context;
using LeaveApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaveApplication.Controllers
{
    public class StatsController : Controller
    {
        private LeaveAppDbContext db = new LeaveAppDbContext();

        // GET: Stats
        public string Index()
        {
            int LeaveTotal = db.Leaves.Count();
            int ReasonTotal = db.Reasons.Count();

            string result = String.Format("Total of {0} Leave(s) and {1} Reason(s) recorded since September 2012. ", LeaveTotal, ReasonTotal);

            var data = db.Database.SqlQuery<ReportResult>("SpStats");

            Report report = new Report();
            report.ReportResults = data.ToArray();

            result += "TOP SCORERS OF " + DateTime.Now.Year + ": ";
            string prevLTypName = "";

            foreach (var item in report.ReportResults)
            {
                if (!prevLTypName.Equals(item.LTypName))
                {
                    prevLTypName = item.LTypName;                    
                }
                else
                {
                    continue;
                }

                result += string.Format("{0}: {1} ({2}) {3} ", item.LTypName, item.EmployeeName, item.NoOfDays, "*****");
            }

            return result;
        }
    }
}