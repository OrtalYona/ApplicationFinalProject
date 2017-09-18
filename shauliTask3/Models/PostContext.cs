using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace shauliTask3.Models
{
    public class PostContext: DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> comments { get; set; }

    }
}