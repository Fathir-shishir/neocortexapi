using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExperiment
{
    public class ExperimentData
    {
        /// <summary>
        /// A dictionary containing named sequences for training.
        /// Each key represents a sequence identifier, and its value is a list of doubles constituting the sequence.
        /// </summary>
        public Dictionary<string, List<double>> Sequences { get; set; }

        /// <summary>
        /// A list of test sequences for predictions.
        /// Each item in the list represents a separate test sequence.
        /// </summary>
        public List<List<double>> TestLists { get; set; }

        /// <summary>
        /// The first MaxNewSynapseCount value for configuration.
        /// </summary>
        public int MaxNewSynapseCount { get; set; }
    }
}
