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
    [Authorize(Roles = "Admin")]
    public class LTypesController : Controller
    {
        private LeaveAppDbContext db = new LeaveAppDbContext();

        // GET: /LTypes/
        public ActionResult Index()
        {
            //return View(db.LTypes.ToList());

            var query = from typ in db.LTypes
                        where typ.Deleted == false
                        select typ;

            return View(query.ToList());
        }

        // GET: /LTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LType ltype = db.LTypes.Find(id);
            if (ltype == null)
            {
                return HttpNotFound();
            }
            return View(ltype);
        }

        // GET: /LTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /LTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Name")] LType ltype)
        {
            if (ModelState.IsValid)
            {
                db.LTypes.Add(ltype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ltype);
        }

        // GET: /LTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LType ltype = db.LTypes.Find(id);
            if (ltype == null)
            {
                return HttpNotFound();
            }
            return View(ltype);
        }

        // POST: /LTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Deleted,UserCreated,DateCreated,UserModified,DateModified")] LType ltype)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ltype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ltype);
        }

        // GET: /LTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LType ltype = db.LTypes.Find(id);
            if (ltype == null)
            {
                return HttpNotFound();
            }
            return View(ltype);
        }

        // POST: /LTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LType ltype = db.LTypes.Find(id);
            ltype.Deleted = true;
            //db.LTypes.Remove(ltype);
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
