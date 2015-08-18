using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LeaveApplication.Models;
using LeaveApplication.Context;

namespace LeaveApplication.Controllers
{
    public class ReasonsController : Controller
    {
        private LeaveAppDbContext db = new LeaveAppDbContext();

        // GET: /Reasons/
        [Authorize(Roles = "Admin,CanCreate,CanEdit")]
        public ActionResult Index()
        {
            //return View(db.Reasons.ToList());

            var query = from rsn in db.Reasons
                        where rsn.Deleted == false
                        select rsn;

            return View(query.ToList());
        }

        // GET: /Reasons/Details/5
        [Authorize(Roles = "Admin,CanEdit")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reason reason = db.Reasons.Find(id);
            if (reason == null)
            {
                return HttpNotFound();
            }
            return View(reason);
        }

        // GET: /Reasons/Create
        [Authorize(Roles = "Admin,CanCreate,CanEdit")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Reasons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,CanCreate,CanEdit")]
        public ActionResult Create([Bind(Include="ID,Desc")] Reason reason)
        {
            if (ModelState.IsValid)
            {
                db.Reasons.Add(reason);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reason);
        }

        // GET: /Reasons/Edit/5
        [Authorize(Roles = "Admin,CanEdit")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reason reason = db.Reasons.Find(id);
            if (reason == null)
            {
                return HttpNotFound();
            }
            return View(reason);
        }

        // POST: /Reasons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,CanEdit")]
        public ActionResult Edit([Bind(Include = "ID,Desc,Deleted,UserCreated,DateCreated,UserModified,DateModified")] Reason reason)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reason).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reason);
        }

        // GET: /Reasons/Delete/5
        [Authorize(Roles = "Admin,CanEdit")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reason reason = db.Reasons.Find(id);
            if (reason == null)
            {
                return HttpNotFound();
            }
            return View(reason);
        }

        // POST: /Reasons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,CanEdit")]
        public ActionResult DeleteConfirmed(int id)
        {
            Reason reason = db.Reasons.Find(id);
            reason.Deleted = true;
            //db.Reasons.Remove(reason);
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
    }
}
