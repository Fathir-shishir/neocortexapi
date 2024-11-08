using MyCloudProject.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyExperiment
{
    internal class ExerimentRequestMessage : IExerimentRequest
    {
        public string ExperimentId { get; set; }
        public string InputFile { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int maxNewSynapseCount { get; set; }
        public string file1 { get; set; }
        public string file2 { get; set; }
    }
}
