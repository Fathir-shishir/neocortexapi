using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExperiment
{
    /// <summary>
    /// Represents the sequence results derived from the MultiSequenceLearning process.
    /// Contains various metrics related to the execution of a cycle.
    /// </summary>
    public class SequenceResult
    {
        /// <summary>
        /// Gets or sets the unique identifier for the cycle.
        /// </summary>
        public string CycleID { get; set; }

        /// <summary>
        /// Gets or sets the total number of cycles processed.
        /// </summary>
        public int CycleCount { get; set; }

        /// <summary>
        /// Gets or sets the accuracy score of the sequence result.
        /// Typically represented as a double value.
        /// </summary>
        public double Accuracy { get; set; }

        /// <summary>
        /// Gets or sets the duration of the cycle.
        /// Represented as a TimeSpan indicating the elapsed time.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the current status of the sequence result.
        /// This could indicate states such as success, failure, or in-progress.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of new synapses created during the cycle.
        /// Reflects the peak connectivity formed in this sequence.
        /// </summary>
        public int MaxNewSynapsCount { get; set; }
    }

}
