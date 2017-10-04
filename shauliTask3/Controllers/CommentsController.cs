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
    public class CommentsController : Controller
    {
        private PostContext db = new PostContext();

        // GET: Comments
        public ActionResult Index()
        {
            var postComments = from s in db.comments.Include(c => c.post) select s;
            return View(postComments.ToList());
        }

        [HttpPost]
        public ViewResult Index(string SearchTitle, string SearchName)
        {
            List<Comment> Comments;

            String query = "select * from Comments where {0}";
            string select = "";
            string where = "";

            if (!String.IsNullOrEmpty(SearchTitle))
            {
                select += "CommentTitle,";
                where += "CommentTitle like '%" + SearchTitle + "%'";
            }

            if (!String.IsNullOrEmpty(SearchName))
            {
                select += "CommentWriter ,";

                if (!String.IsNullOrEmpty(where))
                {
                    where += "and ";
                }
                where += "CommentWriter like '%" + SearchName + "%'";
            }



            if (where == "")
            {
                query = query.Substring(0, query.Length - 10);
            }

            query = String.Format(query, where);
            Comments = (List<Comment>)db.comments.SqlQuery(query).ToList();
            return View(Comments.ToList());
        }


        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment([Bind(Include = "CommentID,CommentTitle,CommentWriter,commentWebSiteLink,text,PostID")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.comments.Add(comment);
                /// when there is a new comment - counter++ ///
                Post post = db.Posts.Find(comment.PostID);
                post.counter++;
                ViewBag.PostID = comment.CommentID;
                db.SaveChanges();
                return RedirectToAction("Home", "Posts");
            }

            ViewBag.PostID = new SelectList(db.Posts, "PostID", "Title", comment.PostID);
            return View(comment);
        }

        // GET: Comments/Create
        public ActionResult Create()
        {
            ViewBag.PostID = new SelectList(db.Posts, "PostID", "PostTitle");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CommentID,CommentTitle,CommentWriter,commentWebSiteLink,text,PostID,counter")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                Post post = db.Posts.Find(comment.PostID);
                post.counter++;
                db.comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PostID = new SelectList(db.Posts, "PostID", "PostTitle", comment.PostID);
            return View(comment);
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.PostID = new SelectList(db.Posts, "PostID", "PostTitle", comment.PostID);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommentID,CommentTitle,CommentWriter,commentWebSiteLink,text,PostID")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PostID = new SelectList(db.Posts, "PostID", "PostTitle", comment.PostID);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.comments.Find(id);
            /// when you delete a comment - counter-- ///
            Post post = db.Posts.Find(comment.PostID);
            if (post.counter > 0)
            post.counter--;
            db.comments.Remove(comment);
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
