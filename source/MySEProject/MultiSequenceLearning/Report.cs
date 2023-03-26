using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySEProject
{
    public class Report
    {

        public int cycle;
        public string sequenceName;
        public List<String> logs;
        public double accuracy;

        public Report() 
        {
            logs = new List<String>();
        }
    }
}
