using LeaveApplication.Context;
using LeaveApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;

namespace LeaveApplication.Controllers
{
    public class ReportsController : Controller
    {
        private LeaveAppDbContext db = new LeaveAppDbContext();
        //
        // GET: /Charts/
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult Report()
        {
            Report report = new Report();

            LoadDdlLTypes();

            return View(report);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Report(Report report)
        {
            if (ModelState.IsValid)
            {
                LoadDdlLTypes(Convert.ToInt32(report.Criteria.LTypeID));

                string LTypeName = "%";

                if (report.Criteria.LTypeID >= 1)
                {
                    LTypeName = db.LTypes
                                        .Where(x => x.ID == report.Criteria.LTypeID)
                                        .Select(y => y.Name).First();
                }

                var data = db.Database.SqlQuery<ReportResult>("SpGenReport @startdate, @enddate, @typename, @email",
                                                    new SqlParameter("@startdate", report.Criteria.StartDate),
                                                    new SqlParameter("@enddate", report.Criteria.EndDate.AddDays(1)),
                                                    new SqlParameter("@typename", LTypeName),
                                                    new SqlParameter("@email", "%"))
                                                    .OrderBy(a => a.LTypName)
                                                    .ThenByDescending(b => b.NoOfDays)
                                                    .ThenBy(c => c.EmployeeName);
                
                report.ReportResults = data.ToArray();
            }

            return View(report);
        }

        public FileResult GetChart(ReportCriteria criteria)
        {
            Report report = new Report();
            report.Criteria = criteria;

            string LTypeName = "%";

            if (report.Criteria.LTypeID >= 1)
            {
                LTypeName = db.LTypes
                                    .Where(x => x.ID == report.Criteria.LTypeID)
                                    .Select(y => y.Name).First();
            }

            LoadDdlLTypes(Convert.ToInt32(report.Criteria.LTypeID));

            var data = db.Database.SqlQuery<ReportResult>("SpGenReport @startdate, @enddate, @typename, @email",
                                                new SqlParameter("@startdate", report.Criteria.StartDate),
                                                new SqlParameter("@enddate", report.Criteria.EndDate),
                                                new SqlParameter("@typename", LTypeName),
                                                new SqlParameter("@email", "%"))
                                                .OrderBy(a => a.EmployeeName)
                                                .ThenBy(b => b.LTypName);

            report.ReportResults = data.ToArray();

            List<string> EmployeeNames = new List<string>();
            List<int> LvWFHCounts = new List<int>();
            List<int> LvALCounts = new List<int>();
            List<int> LvELCounts = new List<int>();
            List<int> LvSLCounts = new List<int>();
            List<int> LvCTCounts = new List<int>();
            List<int> LvMLCounts = new List<int>();
            List<int> LvPLCounts = new List<int>();
            List<int> LvCLCounts = new List<int>();
            List<int> LvOOOCounts = new List<int>();
            List<int> LvHLCounts = new List<int>();
            
            foreach (var item in report.ReportResults)
            {
                if (!EmployeeNames.Contains(item.EmployeeName))
                {
                    EmployeeNames.Add(item.EmployeeName);
                }

                switch (item.LTypName)
                {
                    case "Work From Home":
                        LvWFHCounts.Add(item.NoOfDays);
                        break;
                    case "Annual Leave":
                        LvALCounts.Add(item.NoOfDays);
                        break;
                    case "Emergency Leave":
                        LvELCounts.Add(item.NoOfDays);
                        break;
                    case "Sick Leave":
                        LvSLCounts.Add(item.NoOfDays);
                        break;
                    case "Comp Time Off":
                        LvCTCounts.Add(item.NoOfDays);
                        break;
                    case "Marriage Leave":
                        LvMLCounts.Add(item.NoOfDays);
                        break;
                    case "Paternity Leave":
                        LvPLCounts.Add(item.NoOfDays);
                        break;
                    case "Compassionate Leave":
                        LvCLCounts.Add(item.NoOfDays);
                        break;
                    case "Out of Office":
                        LvOOOCounts.Add(item.NoOfDays);
                        break;
                    case "Hospitalization Leave":
                        LvHLCounts.Add(item.NoOfDays);
                        break;
                }
            }

            string themeChart = @"<Chart>
                      <ChartAreas>
                        <ChartArea Name=""Default"" _Template_=""All"">
                          <AxisY>
                            <LabelStyle Font=""Verdana, 12px"" />
                          </AxisY>
                          <AxisX LineColor=""64, 64, 64, 64"" Interval=""1"">
                            <LabelStyle Font=""Verdana, 12px"" />
                          </AxisX>
                        </ChartArea>
                      </ChartAreas>
                    </Chart>";

            var chart = new System.Web.Helpers.Chart(width: 1500, height: 800, theme: themeChart)
                .AddTitle("Leave Data from " + report.Criteria.StartDate.ToString("yyyy/MM/dd") + " till " + report.Criteria.EndDate.ToString("yyyy/MM/dd"))
                .AddLegend("Legend")
                .SetYAxis("Number of days")
                .SetXAxis("Employees");

            if (LvWFHCounts.Sum() > 0)
            {
                chart.AddSeries(name: "Work from home",
                            xValue: EmployeeNames,
                            yValues: LvWFHCounts);
            }

            if (LvALCounts.Sum() > 0)
            {
                chart.AddSeries(name: "Annual Leave",
                            xValue: EmployeeNames,
                            yValues: LvALCounts);
            }

            if (LvELCounts.Sum() > 0)
            {
                chart.AddSeries(name: "Emergency Leave",
                            xValue: EmployeeNames,
                            yValues: LvELCounts);
            }

            if (LvSLCounts.Sum() > 0)
            {
                chart.AddSeries(name: "Sick Leave",
                            xValue: EmployeeNames,
                            yValues: LvSLCounts);
            }

            if (LvCTCounts.Sum() > 0)
            {
                chart.AddSeries(name: "Comp Time Off",
                            xValue: EmployeeNames,
                            yValues: LvCTCounts);
            }

            if (LvMLCounts.Sum() > 0)
            {
                chart.AddSeries(name: "Marriage Leave",
                            xValue: EmployeeNames,
                            yValues: LvMLCounts);
            }

            if (LvPLCounts.Sum() > 0)
            {
                chart.AddSeries(name: "Paternity Leave",
                            xValue: EmployeeNames,
                            yValues: LvPLCounts);
            }

            if (LvCLCounts.Sum() > 0)
            {
                chart.AddSeries(name: "Compassionate Leave",
                            xValue: EmployeeNames,
                            yValues: LvCLCounts);
            }

            if (LvOOOCounts.Sum() > 0)
            {
                chart.AddSeries(name: "Out of Office",
                            xValue: EmployeeNames,
                            yValues: LvOOOCounts);
            }

            if (LvHLCounts.Sum() > 0)
            {
                chart.AddSeries(name: "Hospitalization Leave",
                            xValue: EmployeeNames,
                            yValues: LvHLCounts);
            }

            return File(chart.ToWebImage().GetBytes(), "image/jpeg", "chart.jpg");
        }

        private void LoadDdlLTypes(int SelectedValue = 0)
        {
            var query = db.LTypes.Select(c => new { c.ID, c.Name });
            ViewBag.LTypes = new SelectList(query.AsEnumerable(), "ID", "Name", SelectedValue);
        }
	}
}