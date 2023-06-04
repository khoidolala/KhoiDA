using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyNhanSu.Models;

namespace QuanLyNhanSu.Areas.Admin.Controllers
{
    public class tblQuyensController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Admin/tblQuyens
        public ActionResult Index()
        {
            return View(db.tblQuyens.ToList());
        }

        // GET: Admin/tblQuyens/Details/5
        /*public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblQuyen tblQuyen = db.tblQuyens.Find(id);
            if (tblQuyen == null)
            {
                return HttpNotFound();
            }
            return View(tblQuyen);
        }*/

        // GET: Admin/tblQuyens/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/tblQuyens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Quyen,TenQuyen,MoTa")] tblQuyen tblQuyen)
        {
            if (ModelState.IsValid)
            {
                db.tblQuyens.Add(tblQuyen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblQuyen);
        }

        // GET: Admin/tblQuyens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblQuyen tblQuyen = db.tblQuyens.Find(id);
            if (tblQuyen == null)
            {
                return HttpNotFound();
            }
            return View(tblQuyen);
        }

        // POST: Admin/tblQuyens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Quyen,TenQuyen,MoTa")] tblQuyen tblQuyen)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblQuyen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblQuyen);
        }

        // GET: Admin/tblQuyens/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblQuyen tblQuyen = db.tblQuyens.Find(id);
            if (tblQuyen == null)
            {
                return HttpNotFound();
            }
            return View(tblQuyen);
        }

        // POST: Admin/tblQuyens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblQuyen tblQuyen = db.tblQuyens.Find(id);
            db.tblQuyens.Remove(tblQuyen);
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
