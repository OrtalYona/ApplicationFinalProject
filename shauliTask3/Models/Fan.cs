using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace shauliTask3.Models
{
    public class Fan
    {
        [Key]
        public int FanID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string sex { get; set; }
        public DateTime birthDay { get; set; }
        public int seniority { get; set; }
    }
    public class FanDBContext : DbContext
    {
        public DbSet<Fan> Fan { get; set; }
    }
}
