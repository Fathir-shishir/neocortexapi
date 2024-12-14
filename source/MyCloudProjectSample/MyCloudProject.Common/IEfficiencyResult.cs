
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCloudProject.Common
{
    public interface IEfficiencyResult
    {
        /// <summary>
        /// Gets or sets the unique identifier for the experiment.
        /// This ID is used to track and distinguish experiments in the system.
        /// </summary>
        string ExperimentId { get; set; }

        /// <summary>
        /// Gets or sets the start time of the experiment in UTC format.
        /// This value indicates when the experiment execution began.
        /// </summary>
        DateTime? StartTimeUtc { get; set; }

        /// <summary>
        /// Gets or sets the end time of the experiment in UTC format.
        /// This value indicates when the experiment execution was completed.
        /// </summary>
        DateTime? EndTimeUtc { get; set; }

        /// <summary>
        /// Represents the average duration of processing sequences during the experiment.
        /// </summary>
        TimeSpan AverageDuration { get; set; }

        /// <summary>
        /// Represents the average sequence count achieved across all experiments.
        /// </summary>
        float AverageSequenceCount { get; set; }

        /// <summary>
        /// Represents the rate of failed experiments as a percentage of total experiments.
        /// </summary>
        float FailureRate { get; set; }

        /// <summary>
        /// Represents the average accuracy percentage achieved across all experiments.
        /// </summary>
        double AverageAccuracy { get; set; }

        /// <summary>
        /// Represents the configuration parameter indicating the maximum number of new synapses allowed per segment.
        /// </summary>
        int MaxNewSynapseCount { get; set; }
    }

}
