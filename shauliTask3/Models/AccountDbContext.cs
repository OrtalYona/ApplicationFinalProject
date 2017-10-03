using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace shauliTask3.Models
{
    public class AccountDbContext :DbContext
    {
        public DbSet<UsetAccount> userAccounts { get; set; }

        public System.Data.Entity.DbSet<shauliTask3.Models.Post> Posts { get; set; }
    }
}