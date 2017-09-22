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
     // private AccountDbContext db = new AccountDbContext();
        // GET: Account
        public ActionResult Index()
        {
            if (Session["UserID"] == null)
            {

                //shuld print into a text box--> " Admin only! login before"
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
                return RedirectToAction("Index");

            }
        }

        [HttpPost]
        public ViewResult Index(string SearchFirst, string SearchLast, string SearchUser, string SearchEmail)
        {
            using (AccountDbContext db = new AccountDbContext())
            {
                List<UsetAccount> userAccounts;


                String query = "select * from userAccounts where {0}";
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


                if (!String.IsNullOrEmpty(SearchUser))
                {
                    select += "UserName ,";
                    if (!String.IsNullOrEmpty(where))
                    {
                        where += "and ";
                    }
                    where += "UserName like '%" + SearchUser + "%'";
                }

                if (!String.IsNullOrEmpty(SearchEmail))
                {
                    select += "Eamil ,";
                    if (!String.IsNullOrEmpty(where))
                    {
                        where += "and ";
                    }
                    where += "Email like '%" + SearchEmail + "%'";
                }

                if (where == "")
                {
                    query = query.Substring(0, query.Length - 10);// empty query
                }

                query = String.Format(query, where);
                userAccounts = (List<UsetAccount>)db.userAccounts.SqlQuery(query).ToList();
                return View(userAccounts.ToList());
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

    }
}