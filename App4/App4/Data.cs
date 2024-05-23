using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace App4
{
    public class Data
    {

        [AutoIncrement, PrimaryKey]
        public int EMPID { get; set; }
        public string EMPNAME { get; set; }
        public double HOURSWORK { get; set; }
        public double GROSSINCOME { get; set; }
        public double BONUS { get; set; }

        // SOLVING AND OUTPUTS

        public double DEDUCTION { get; set; }
        public double NETINCOME { get; set; }
       



    }
}
