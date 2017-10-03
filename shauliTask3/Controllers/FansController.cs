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
    public class FansController : Controller
    {
        private FanDBContext db = new FanDBContext();
        //testtefdsfvdsfv
        // GET: Fans
        public ActionResult Index()
        {
            var fans = from s in db.Fan select s;

            return View(fans.ToList());
         //   return View(db.Fan.ToList());
        }

        [HttpPost]
        public ViewResult Index(string SearchFirst, string SearchLast, string SearchGender)
        {
            List<Fan> fans;

            String query = "select * from fans where {0}";
            string select = "";
            string where = "";

            if (!String.IsNullOrEmpty(SearchFirst))
            {
                select += "FirstName,";
                where += "FirstName like '%" + SearchFirst + "%'";
            }

            if (!String.IsNullOrEmpty(SearchLast))// should insert to here
            {
                select += "LastName ,";

                if (!String.IsNullOrEmpty(where))
                {
                    where += "and ";
                }
                where += "LastName like '%" + SearchLast + "%'";
            }


            if (!String.IsNullOrEmpty(SearchGender))
            {
                select += "sex ,";
                if (!String.IsNullOrEmpty(where))
                {
                    where += "and ";
                }
                where += "sex like '%" + SearchGender + "%'";
            }
            if (where == "")
            {
                query = query.Substring(0, query.Length - 10);// empty query
            }

            query = String.Format(query, where);
            fans = (List<Fan>)db.Fan.SqlQuery(query).ToList();
            return View(fans.ToList());
        }


        // GET: Fans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fan fan = db.Fan.Find(id);
            if (fan == null)
            {
                return HttpNotFound();
            }
            return View(fan);
        }

        // GET: Fans/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Fans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FanID,firstName,lastName,sex,birthDay,seniority")] Fan fan)
        {
            if (ModelState.IsValid)
            {
                db.Fan.Add(fan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fan);
        }

        // GET: Fans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fan fan = db.Fan.Find(id);
            if (fan == null)
            {
                return HttpNotFound();
            }
            return View(fan);
        }

        // POST: Fans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FanID,firstName,lastName,sex,birthDay,seniority")] Fan fan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fan);
        }

        // GET: Fans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fan fan = db.Fan.Find(id);
            if (fan == null)
            {
                return HttpNotFound();
            }
            return View(fan);
        }

        // POST: Fans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fan fan = db.Fan.Find(id);
            db.Fan.Remove(fan);
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
