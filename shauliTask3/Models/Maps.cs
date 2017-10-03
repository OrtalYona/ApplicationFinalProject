using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace shauliTask3.Models
{
    public class Maps
    {
        public int MapsID { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
    public class MapsDbContext : DbContext
    {
        public DbSet<Maps> Map { get; set; }
    }
}