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
    }
}