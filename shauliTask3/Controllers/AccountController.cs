using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using shauliTask3.Models;
using System.Net;
using System.Data.Entity;
using System.Collections;
using static shauliTask3.Models.UsetAccount;

namespace shauliTask3.Controllers
{
    
    public class AccountController : Controller
    {
        public ActionResult Index()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }
            else if (((shauliTask3.Models.UsetAccount)Session["User"]).IsAdmin)
            {
                using (AccountDbContext db = new AccountDbContext())
                {
                    var accounts = from s in db.userAccounts select s;

                    return View(accounts.ToList());
               }
            }

            else
            {


                return RedirectToAction("Home", "Posts");
                
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

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UsetAccount user)
        {
            using (AccountDbContext db = new AccountDbContext())
            {
                if ((user.UserName)=="admin" && (user.Password)=="1234")
                {

                    user.IsAdmin = true;
                    user.UserId = 0;
                    user.UserName = "admin";
                    Session["IsAdmin"] = user.IsAdmin.ToString();
                    Session["UserID"] = user.UserId.ToString();
                    Session["UserName"] = user.UserName.ToString();
                    Session["User"] = user;
                    return RedirectToAction("LoggedIn");
                }
                var usr = db.userAccounts.SingleOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);

                if (usr != null)

                {
                    Session["UserID"] = usr.UserId.ToString();
                    Session["UserName"] = usr.UserName.ToString();
                    Session["User"] = usr;

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
        public ActionResult Edit([Bind(Include = "UserID,FirstName,LastName,Email,UserName,Password,ComfirmPassword,IsAdmin")] UsetAccount user)
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

        public ActionResult LogOut()
        {
            Session["UserID"] = null;
            Session.Clear();
            return RedirectToAction("Index","Home");
        }

    }
}
