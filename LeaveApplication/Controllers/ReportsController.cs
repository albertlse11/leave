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
            LoadDdlLTypes();

            Report report = new Report();

            report.StartDate = DateTime.Now;
            report.EndDate = DateTime.Now;

            string LTypeName = "%";

            var data = db.Database.SqlQuery<ReportResult>("SpGenReport @startdate, @enddate, @typename, @email",
                                                new SqlParameter("@startdate", report.StartDate),
                                                new SqlParameter("@enddate", report.EndDate),
                                                new SqlParameter("@typename", LTypeName),
                                                new SqlParameter("@email", "%"));

            report.ReportResults = data.ToArray();

            return View(report);            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Report(Report report)
        {
            string LTypeName = "%";

            if (report.LTypeID >= 1)
            {
                LTypeName = db.LTypes
                                    .Where(x => x.ID == report.LTypeID)
                                    .Select(y => y.Name).First();
            }

            LoadDdlLTypes(Convert.ToInt32(report.LTypeID));
            
            var data = db.Database.SqlQuery<ReportResult>("SpGenReport @startdate, @enddate, @typename, @email",
                                                new SqlParameter("@startdate", report.StartDate),
                                                new SqlParameter("@enddate", report.EndDate.AddDays(1)),
                                                new SqlParameter("@typename", LTypeName),
                                                new SqlParameter("@email", "%"));

            report.ReportResults = data.ToArray();
            
            return View(report);


            //Chart chart = new Chart();
            //chart.Titles.Add("Leave System");
            //chart.ChartAreas.Add(new ChartArea());
            //chart.ChartAreas[0].Position.Height = 100;
            //chart.ChartAreas[0].Position.Width = 100;
            //chart.ChartAreas[0].AxisX.Interval = 1;
            
            //chart.Series.Add(new Series("WFH"));
            //chart.Series["WFH"].ChartType = SeriesChartType.Column;
            //chart.Series["WFH"].Points.DataBindXY(
            //    data.Where(data1 => data1.Name == "Work from home").Select(data1 => data1.EmpName.ToString()).ToArray(),
            //    data.Where(data1 => data1.Name == "Work from home").Select(data1 => data1.NoOfDays).ToArray());
            //chart.Series["WFH"].Label = "#VALY";
            //chart.Legends.Add(new Legend("WFH"));
            //chart.Series["WFH"].Legend = "WFH";
            //chart.Series["WFH"].IsVisibleInLegend = true;
                        
            //MemoryStream ms = new MemoryStream();
            //chart.SaveImage(ms, ChartImageFormat.Png);
            //return File(ms.ToArray(), "image/png");
        }

        public FileResult GetChart(Report report)
        {
            string LTypeName = "%";

            if (report.LTypeID >= 1)
            {
                LTypeName = db.LTypes
                                    .Where(x => x.ID == report.LTypeID)
                                    .Select(y => y.Name).First();
            }

            LoadDdlLTypes(Convert.ToInt32(report.LTypeID));

            var data = db.Database.SqlQuery<ReportResult>("SpGenReport @startdate, @enddate, @typename, @email",
                                                new SqlParameter("@startdate", report.StartDate),
                                                new SqlParameter("@enddate", report.EndDate),
                                                new SqlParameter("@typename", LTypeName),
                                                new SqlParameter("@email", "%"));

            report.ReportResults = data.ToArray();

            List<string> EmpNames = new List<string>();
            List<int> LvWFHCounts = new List<int>();
            List<int> LvALCounts = new List<int>();
            List<int> LvELCounts = new List<int>();
            List<int> LvSLCounts = new List<int>();
            List<int> LvCTCounts = new List<int>();
            List<int> LvMLCounts = new List<int>();
            List<int> LvPLCounts = new List<int>();
            List<int> LvCLCounts = new List<int>();
            List<int> LvOOOCounts = new List<int>();
            
            foreach (var item in report.ReportResults)
            {
                if (!EmpNames.Contains(item.EmpName))
                {
                    EmpNames.Add(item.EmpName);
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
                .AddTitle("Leave Data from " + report.StartDate.ToString("yyyy/MM/dd") + " till " + report.EndDate.ToString("yyyy/MM/dd"))
                .AddLegend("Legend")
                .SetYAxis("Number of days")
                .SetXAxis("Employees");

            if (LvWFHCounts.Sum() > 0)
            {
                chart.AddSeries(name: "Work from home",
                            xValue: EmpNames,
                            yValues: LvWFHCounts);
            }

            if (LvALCounts.Sum() > 0)
            {
                chart.AddSeries(name: "Annual Leave",
                            xValue: EmpNames,
                            yValues: LvALCounts);
            }

            if (LvELCounts.Sum() > 0)
            {
                chart.AddSeries(name: "Emergency Leave",
                            xValue: EmpNames,
                            yValues: LvELCounts);
            }

            if (LvSLCounts.Sum() > 0)
            {
                chart.AddSeries(name: "Sick Leave",
                            xValue: EmpNames,
                            yValues: LvSLCounts);
            }

            if (LvCTCounts.Sum() > 0)
            {
                chart.AddSeries(name: "Comp Time Off",
                            xValue: EmpNames,
                            yValues: LvCTCounts);
            }

            if (LvMLCounts.Sum() > 0)
            {
                chart.AddSeries(name: "Marriage Leave",
                            xValue: EmpNames,
                            yValues: LvMLCounts);
            }

            if (LvPLCounts.Sum() > 0)
            {
                chart.AddSeries(name: "Paternity Leave",
                            xValue: EmpNames,
                            yValues: LvPLCounts);
            }

            if (LvCLCounts.Sum() > 0)
            {
                chart.AddSeries(name: "Compassionate Leave",
                            xValue: EmpNames,
                            yValues: LvCLCounts);
            }

            if (LvOOOCounts.Sum() > 0)
            {
                chart.AddSeries(name: "Out of Office",
                            xValue: EmpNames,
                            yValues: LvOOOCounts);
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