using MyCloudProject.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyExperiment
{
    internal class ExerimentRequestMessage : IExerimentRequestMessage
    {
        public string ExperimentId { get; set; }
        public string InputFile { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<double> sequences1 { get; set; }
        public List<double> sequences2 { get; set; }
        public List<double> sequences3{ get; set; }
        public int maxNewSynapseCount { get; set; }


    }
}
