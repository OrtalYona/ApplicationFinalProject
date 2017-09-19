using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using shauliTask3.Models;
using System.Net;
using System.Data.Entity;
using static shauliTask3.Models.UsetAccount;

namespace shauliTask3.Controllers
{
    
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                using (AccountDbContext db = new AccountDbContext())
                {
                    return View(db.userAccounts.ToList());
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UsetAccount account)
        {
            if (ModelState.IsValid)
            {
                using (AccountDbContext db = new AccountDbContext())
                {
                    db.userAccounts.Add(account);

                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = account.FirstName + " " + account.LastName + " successfully registered ";
            }
            return View();
        }
        //login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UsetAccount user)
        {
            using (AccountDbContext db = new AccountDbContext())
            {

                var usr = db.userAccounts.SingleOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);

                if (usr != null)

                {
                    Session["UserID"] = usr.UserID.ToString();
                    Session["UserName"] = usr.UserName.ToString();


                    return RedirectToAction("LoggedIn");
                }
                else
                {
                    ModelState.AddModelError("", "UserName or Password is wrong");
                }
            }
            return View();
        }

        public ActionResult LoggedIn()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (AccountDbContext db = new AccountDbContext())
            {
                UsetAccount user = db.userAccounts.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (AccountDbContext db = new AccountDbContext())
            {
                UsetAccount user = db.userAccounts.Find(id);
                db.userAccounts.Remove(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (AccountDbContext db = new AccountDbContext())
            {
                UsetAccount user = db.userAccounts.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (AccountDbContext db = new AccountDbContext())
            {
                UsetAccount user = db.userAccounts.Find(id);

                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
        }








        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,FirstName,LastName,Email,UserName,Password,ComfirmPassword")] UsetAccount user)
        {
            if (ModelState.IsValid)
            {
                using (AccountDbContext db = new AccountDbContext())
                {
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(user);
        }

        /*public ActionResult Search(string term)
        {

            AcountModel am = new AcountModel();
            return Json(am.Search(term), JsonRequestBehavior.AllowGet);
        }*/

        [ActionName("Search")]
        public ActionResult Search()
        {

            return View();

        }
        [HttpPost]
       // [ActionName("StartSearch")]
        public ActionResult Search(string FirstName,
    string LastName,
    string Email,
    string UserName)
        {
            using (AccountDbContext db = new AccountDbContext())
            {


                var accounts = from a in db.userAccounts
                               select a;
        
                if (FirstName != null)
                {
                    accounts = accounts.Where(x => x.FirstName == FirstName);
                }

                if (LastName != null)
                {
                    accounts = accounts.Where(x => x.LastName == LastName);
                }

                if (Email != null)
                {
                    accounts = accounts.Where(x => x.Email == Email);
                }

                if (UserName != null)
                {
                    accounts = accounts.Where(x => x.UserName == UserName);
                }


                return View(accounts.OrderBy(x => x.UserID));//<----------
            }
        }
    }
}