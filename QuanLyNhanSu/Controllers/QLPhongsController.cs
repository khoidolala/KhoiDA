using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyNhanSu.Models;

namespace QuanLyNhanSu.Controllers
{
    public class QLPhongsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: QLPhongs
        public ActionResult Index()
        {
            return View(db.tblPhongs.ToList());
        }

        // GET: QLPhongs/Details/5
        public ActionResult Details(string id)
        {
           if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPhong tblPhong = db.tblPhongs.Find(id);
            if (tblPhong == null)
            {
                return HttpNotFound();
            }
            /*var NVTP = db.tblThongTinNVs.Where(x => x.MaPhong == id).ToList();
            if (NVTP == null)
            {
                return HttpNotFound();
            }*/
            return View(tblPhong);
        }

        // GET: QLPhongs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QLPhongs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaPhong,TenPhong,TenTruongPhong,DienThoaiPhong")] tblPhong tblPhong)
        {
            if (ModelState.IsValid)
            {
                db.tblPhongs.Add(tblPhong);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblPhong);
        }

        // GET: QLPhongs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPhong tblPhong = db.tblPhongs.Find(id);
            if (tblPhong == null)
            {
                return HttpNotFound();
            }
            return View(tblPhong);
        }

        // POST: QLPhongs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaPhong,TenPhong,TenTruongPhong,DienThoaiPhong")] tblPhong tblPhong)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblPhong).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblPhong);
        }

        // GET: QLPhongs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPhong tblPhong = db.tblPhongs.Find(id);
            if (tblPhong == null)
            {
                return HttpNotFound();
            }
            return View(tblPhong);
        }

        // POST: QLPhongs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            tblPhong tblPhong = db.tblPhongs.Find(id);
            db.tblPhongs.Remove(tblPhong);
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
