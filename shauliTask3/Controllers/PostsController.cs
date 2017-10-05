using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using shauliTask3.Models;
using System.Dynamic;

namespace shauliTask3.Controllers
{
    public class PostsController : Controller
    {
        private PostContext db = new PostContext();
        private MapsDbContext maps = new MapsDbContext();

        public ActionResult Index()
        {

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login","Account");
            }

            else if (((shauliTask3.Models.UsetAccount)Session["User"]).IsAdmin)
            {

                var posts = from s in db.Posts select s;

                return View(posts.ToList());
            }
            else
            {

                return RedirectToAction("Index", "Home");

            }
        }
        

        [HttpPost]
        public ViewResult Index(string SearchTitle, string SearchName)
        {
            List<Post> posts;

            String query = "select * from posts where {0}";
            string select = "";
            string where = "";

            if (!String.IsNullOrEmpty(SearchTitle))
            {
                select += "PostTitle,";
                where += "PostTitle like '%" + SearchTitle + "%'";
            }
            if (!String.IsNullOrEmpty(SearchName))
            {
                select += "postWriter ,";

                if (!String.IsNullOrEmpty(where))
                {
                    where += "and ";
                }
                where += "postWriter like '%" + SearchName + "%'";
            }

            if (where == "")
            {
                query = query.Substring(0, query.Length - 10);
            }
            query = String.Format(query, where);
            posts = (List<Post>)db.Posts.SqlQuery(query).ToList();
            return View(posts.ToList());
        }


        public ActionResult Home()
        {
            using (PostContext db=new PostContext())
            {
                ViewBag.TotalPosts = db.Posts.Count();
                ViewBag.TotalComments = db.comments.Count();
            }
            using (AccountDbContext db1 = new AccountDbContext())
            {
                ViewBag.TotalAccounts = db1.userAccounts.Count();

            }
            using (FanDBContext db2 = new FanDBContext())
            {
                ViewBag.TotalFans = db2.Fan.Count();

            }

            return View(db.Posts.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostID,PostTitle,postWriter,postWebSiteLink,date,text,video,image")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.counter = 0;
                post.date = DateTime.Now;
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostID,PostTitle,postWriter,postWebSiteLink,date,text,video,image,counter")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
        //    bool isAdmin = (Boolean)Session["isAmdin"];
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Comments(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post.comments.ToList());
        }

        public ActionResult Statistics()
        {
            var query = from i in db.Posts
                        group i by i.postWriter into g
                        select new { PostWriter = g.Key, c=g.Count() };

            return View(query.ToList());
        }

        public ActionResult Join()
        {

            var query = from d in db.Posts
                         join j in db.comments on d.PostID equals j.PostID
                         select new { d.PostTitle, j.CommentTitle };

            return View(query.ToList());
        }

        public ActionResult JoinAccountPost()//fix!
        {
            AccountDbContext dbuser = new AccountDbContext();
            var query = from d in db.Posts
                        join u in dbuser.userAccounts on d.postWriter equals u.UserName
                        select new { d.PostTitle, u.UserName };

            return View(query.ToList());
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
