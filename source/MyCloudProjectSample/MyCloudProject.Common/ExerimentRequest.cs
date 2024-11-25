using System;
using System.Collections.Generic;
using System.Text;

namespace MyCloudProject.Common
{
    /// <summary>
    /// Defines the contract for the message request that will run your experiment.
    /// </summary>
    public interface IExerimentRequest
    {
        public string ExperimentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int maxNewSynapseCount { get; set; }
        public string file { get; set; }
    }
}
