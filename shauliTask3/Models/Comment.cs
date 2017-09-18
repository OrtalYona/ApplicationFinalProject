using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace shauliTask3.Models
{
    public class Comment
    {
        public int CommentID { get; set; }
        public string CommentTitle { get; set; }
        public string CommentWriter { get; set; }
        public string commentWebSiteLink { get; set; }
        public string text { get; set; }
        public int PostID { get; set; }
       // [ForeignKey("PostID")]
        public virtual Post post { get; set; }
    }

}