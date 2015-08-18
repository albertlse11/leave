using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LeaveApplication.Models;
using System.IO;
using LeaveApplication.Context;

namespace LeaveApplication.Controllers
{
    public class LeavesController : Controller
    {
        private LeaveAppDbContext db = new LeaveAppDbContext();

        // GET: /Leaves/
        public ActionResult Index()
        {
            //return View(db.Leaves.ToList());

            var query = from lvs in db.Leaves
                        join emp in db.Employees on lvs.EmployeeID equals emp.ID
                        join typ in db.LTypes on lvs.LTypeID equals typ.ID
                        join rsn in db.Reasons on lvs.ReasonID equals rsn.ID
                        where lvs.Deleted == false
                        orderby lvs.LeaveDate descending
                        select new LeaveList() 
                        { 
                            ID = lvs.ID, 
                            LeaveDate = lvs.LeaveDate, 
                            EmployeeName = emp.LastName + ", " + emp.FirstName,
                            LTypeName = typ.Name, 
                            ReasonDesc = rsn.Desc 
                        };

            return View(query.ToList());
        }

        [Authorize(Roles = "Admin,CanEdit")]
        // GET: /Leaves/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leave leave = db.Leaves.Find(id);
            if (leave == null)
            {
                return HttpNotFound();
            }

            DisplayEmployeeName(leave);
            DisplayLTypeName(leave);
            DisplayReasonDesc(leave);
                        
            return View(leave);
        }

        [Authorize(Roles = "Admin,CanCreate,CanEdit")]
        // GET: /Leaves/Create
        public ActionResult Create(int? id)
        {
            //if (TempData["UploadedFile"] != null)
            //{
            //    return View(ExtractMsgFile(TempData["UploadedFile"].ToString()));
            //}

            //LoadDdlEmployees();
            //LoadDdlLTypes();
            //LoadDdlReasons();                
            
            //return View();

            //Create brand new record
            if (id == null)
            {
                if (TempData["UploadedFile"] != null)
                {
                    return View(ExtractMsgFile(TempData["UploadedFile"].ToString()));
                }

                LoadDdlEmployees();
                LoadDdlLTypes();
                LoadDdlReasons();

                return View();
            }

            //Copy a record and populate on Create View
            Leave leave = db.Leaves.Find(id);
            if (leave == null)
            {
                return HttpNotFound();
            }

            LoadDdlEmployees(leave.EmployeeID);
            LoadDdlLTypes(leave.LTypeID);
            LoadDdlReasons(leave.ReasonID);

            return View(leave);
        }

        // POST: /Leaves/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,CanCreate,CanEdit")]
        public ActionResult Create([Bind(Include="ID,LeaveDate,EmployeeID,LTypeID,ReasonID")] Leave leave)
        {
            if (ModelState.IsValid)
            {
                db.Leaves.Add(leave);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(leave);
        }

        [Authorize(Roles = "Admin,CanEdit")]
        // GET: /Leaves/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leave leave = db.Leaves.Find(id);
            if (leave == null)
            {
                return HttpNotFound();
            }

            LoadDdlEmployees(leave.EmployeeID);
            LoadDdlLTypes(leave.LTypeID);
            LoadDdlReasons(leave.ReasonID); 
            
            return View(leave);
        }

        // POST: /Leaves/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,CanEdit")]
        public ActionResult Edit([Bind(Include = "ID,LeaveDate,EmployeeID,LTypeID,ReasonID,Deleted,UserCreated,DateCreated,UserModified,DateModified")] Leave leave)
        {
            if (ModelState.IsValid)
            {
                db.Entry(leave).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(leave);
        }

        [Authorize(Roles = "Admin,CanEdit")]
        // GET: /Leaves/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leave leave = db.Leaves.Find(id);
            if (leave == null)
            {
                return HttpNotFound();
            }

            DisplayEmployeeName(leave);
            DisplayLTypeName(leave);
            DisplayReasonDesc(leave);

            return View(leave);
        }

        // POST: /Leaves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,CanEdit")]
        public ActionResult DeleteConfirmed(int id)
        {
            Leave leave = db.Leaves.Find(id);
            leave.Deleted = true;
            //db.Leaves.Remove(leave);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize(Roles = "Admin,CanCreate,CanEdit")]
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,CanCreate,CanEdit")]
        public ActionResult Upload(HttpPostedFileBase UploadFile)
        {
            if (!Directory.Exists(Server.MapPath("~/Temp/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Temp/"));
            }

            string path = Server.MapPath("~/Temp/" + UploadFile.FileName.Substring(UploadFile.FileName.LastIndexOf('\\') + 1));
            
            UploadFile.SaveAs(path);

            TempData["UploadedFile"] = path;

            return RedirectToAction("Create");
        }

        private void LoadDdlEmployees(int SelectedValue = 0)
        {
            var query = db.Employees.OrderBy(c => c.LastName).Select(c => new { c.ID, Name = c.LastName + ", " + c.FirstName });
            ViewBag.Employees = new SelectList(query.AsEnumerable(), "ID", "Name", SelectedValue);
        }

        private void LoadDdlLTypes(int SelectedValue = 0)
        {
            var query = db.LTypes.Select(c => new { c.ID, c.Name });
            ViewBag.LTypes = new SelectList(query.AsEnumerable(), "ID", "Name", SelectedValue);
        }

        private void LoadDdlReasons(int SelectedValue = 0)
        {
            var query = db.Reasons.Select(c => new { c.ID, c.Desc });
            ViewBag.Reasons = new SelectList(query.AsEnumerable(), "ID", "Desc", SelectedValue);
        }

        private void DisplayEmployeeName(Leave leave)
        {
            ViewBag.EmployeeName = db.Employees
                                    .Where(x => x.ID == leave.EmployeeID)
                                    .Select(y => y.LastName + ", " + y.FirstName).First();
        }

        private void DisplayLTypeName(Leave leave)
        {
            ViewBag.LTypeName = db.LTypes
                                    .Where(x => x.ID == leave.LTypeID)
                                    .Select(y => y.Name).First();
        }

        private void DisplayReasonDesc(Leave leave)
        {
            ViewBag.ReasonDesc = db.Reasons
                                    .Where(x => x.ID == leave.ReasonID)
                                    .Select(y => y.Desc).First();
        }

        private Leave ExtractMsgFile(string UploadedFile)
        {
            string ExtractedFolder = Server.MapPath("~/Temp/" + Guid.NewGuid().ToString() + "/");

            if (!Directory.Exists(ExtractedFolder))
            {
                Directory.CreateDirectory(ExtractedFolder);
            }

            MsgReader.Reader reader = new MsgReader.Reader();
            reader.ExtractToFolder(UploadedFile, ExtractedFolder, false);

            string[] ExtractedFiles = Directory.GetFiles(ExtractedFolder, "*.htm");

            Leave leave = new Leave();

            if (System.IO.File.Exists(ExtractedFiles[0]))
            {
                string MsgContent = System.IO.File.ReadAllText(ExtractedFiles[0]);

                string BreakTag = "<br/>";

                string FromTag = "From:</td><td>";
                string From = MsgContent.Substring(MsgContent.IndexOf(FromTag) + FromTag.Length);
                From = From.Substring(From.IndexOf("&nbsp&lt;") + "&nbsp&lt;".Length);
                From = From.Substring(0, From.IndexOf("&gt;"));

                if (From.ToLower().Contains("cn=recipients"))
                {
                    var emailList = db.Employees.Select(e => e.Email);
                    foreach (var email in emailList)
                    {
                        if (From.ToLower().Contains(email))
                        {
                            From = email;
                            break;
                        }
                    }
                }

                string SentOnTag = "Sent on:</td><td>";
                string SentOn = MsgContent.Substring(MsgContent.IndexOf(SentOnTag) + SentOnTag.Length);
                SentOn = SentOn.Substring(0, SentOn.IndexOf(BreakTag));

                string SubjectTag = "Subject:</td><td>";
                string Subject = MsgContent.Substring(MsgContent.IndexOf(SubjectTag) + SubjectTag.Length);
                Subject = Subject.Substring(0, Subject.IndexOf(BreakTag));

                ViewBag.From = From;
                ViewBag.SentOn = SentOn;
                ViewBag.Subject = Subject;

                leave.LeaveDate = Convert.ToDateTime(SentOn);

                leave.EmployeeID = db.Employees
                                    .Where(x => x.Email == From)
                                    .Select(y => y.ID).FirstOrDefault();

                Directory.Delete(ExtractedFolder, true);
                System.IO.File.Delete(UploadedFile);
            }

            LoadDdlEmployees(leave.EmployeeID);
            LoadDdlLTypes();
            LoadDdlReasons();

            return leave;
        }
    }
}
