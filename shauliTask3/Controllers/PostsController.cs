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
    public class PostsController : Controller
    {
        private PostContext db = new PostContext();


        // GET: Posts
        ///
        public ActionResult Index()
        {
            var posts = from s in db.Posts select s;

            return View(posts.ToList());
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
            if (!String.IsNullOrEmpty(SearchName))// should insert to here
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
                query = query.Substring(0, query.Length - 10);// empty query
            }
            query = String.Format(query, where);
            posts = (List<Post>)db.Posts.SqlQuery(query).ToList();
            return View(posts.ToList());
        }

        //group by query
        public ViewResult Index2()
        {
            var query =
                from i in db.Posts
                group i by i.postWriter into g
                select new { PostWriter = g.Key, num = g};

            return View(query.ToList());
        }






        public ActionResult Home()
        { 
            return View(db.Posts.ToList());
        }
        // GET: Posts/Details/5
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

        // GET: Posts/Create
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
               // ViewBag.PostID = post.comments;
                //////////////////////////////////////////
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
               // db.Entry(post).Property("counter").IsModified = false;//////////////////////////////////////////////

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Delete/5
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

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bool isAdmin = (Boolean)Session["isAmdin"];

            

            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        /// <summary>
        /// ///////////////////////
     
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


        public ActionResult Statistics()////////////////////////////////////
        {
            List<Post> posts;
            string a = "ortal";
            String query = "select * from posts groupby {0}";
            string select = "postWriter";
            string groupby = "postTitle";

            query = String.Format(query, groupby);
            posts = (List<Post>)db.Posts.SqlQuery(query).ToList();
            return View(posts.ToList());

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
