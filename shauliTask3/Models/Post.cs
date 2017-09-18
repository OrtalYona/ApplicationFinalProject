using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace shauliTask3.Models
{
    public class Post
    {
        
        public int PostID { get; set; }
        public string PostTitle { get; set; }
        public string postWriter { get; set; }
        public string postWebSiteLink { get; set; }
        public DateTime date { get; set; }
        public string text { get; set; }
        public string video { get; set; }
        public string image { get; set; }
       // public Comment comment { get; set; }
        public virtual int counter { get; set; }
        public virtual ICollection<Comment> comments { get; set; }
    }
}