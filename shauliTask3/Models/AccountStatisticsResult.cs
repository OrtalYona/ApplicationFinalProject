using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace shauliTask3.Models
{
    public class AccountStatisticsResult
    {
        public UsetAccount Accounte { get; set; }
        public int Count { get; set; }
        public string Com { get; set; }
        public freq freq { get; set; }

    }

    public class freq
    {
        public int MaxComments { get; set; }
        public int MinComments { get; set; }
        public double AvgCommencts { get; set; }
    }


}