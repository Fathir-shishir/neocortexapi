using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExperiment
{
    public class ExperimentData
    {
        public Dictionary<string, List<double>> Sequences { get; set; }
        public int MaxNewSynapseCount1 { get; set; }
        public int MaxNewSynapseCount2 { get; set; }
    }
}
