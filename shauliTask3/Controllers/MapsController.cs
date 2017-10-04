using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using shauliTask3.Models;

namespace shauliTask3.Controllers
{
    public class MapsController : Controller
    {
        private MapsDbContext db = new MapsDbContext();

        // GET: Maps
        public ActionResult Index()
        {
            return View(db.Map.ToList());
        }

        // GET: Maps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maps maps = db.Map.Find(id);
            if (maps == null)
            {
                return HttpNotFound();
            }
            return View(maps);
        }

        // GET: Maps/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Maps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MapsID,Latitude,Longitude")] Maps maps)
        {
            if (ModelState.IsValid)
            {
                db.Map.Add(maps);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(maps);
        }

        // GET: Maps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maps maps = db.Map.Find(id);
            if (maps == null)
            {
                return HttpNotFound();
            }
            return View(maps);
        }

        // POST: Maps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MapsID,Latitude,Longitude")] Maps maps)
        {
            if (ModelState.IsValid)
            {
                db.Entry(maps).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(maps);
        }

        // GET: Maps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maps maps = db.Map.Find(id);
            if (maps == null)
            {
                return HttpNotFound();
            }
            return View(maps);
        }

        // POST: Maps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Maps maps = db.Map.Find(id);
            db.Map.Remove(maps);
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

        [HttpGet]
        public ActionResult GetCordinates()
        {
             MapsDbContext maps = new MapsDbContext();

            return Json(maps.Map.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}
