using Azure;
using Azure.Data.Tables;
using MyCloudProject.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyExperiment
{

    public class EfficiencyResult : ITableEntity, IEfficiencyResult
    {
        public EfficiencyResult(string partitionKey, string rowKey)
        {
            this.PartitionKey = partitionKey;
            this.RowKey = rowKey;
        }

        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        public DateTimeOffset? Timestamp { get; set; }

        public ETag ETag { get; set; }

        public string ExperimentId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime? StartTimeUtc { get; set; }

        public DateTime? EndTimeUtc { get; set; }

        /// <summary>
        /// Represents the average duration of processing sequences during the experiment.
        /// </summary>
        public TimeSpan AverageDuration { get; set; }

        /// <summary>
        /// Represents the average sequence count achieved across all experiments.
        /// </summary>
        public float AverageSequenceCount { get; set; }

        /// <summary>
        /// Represents the rate of failed experiments as a percentage of total experiments.
        /// </summary>
        public float FailureRate { get; set; }

        /// <summary>
        /// Represents the average accuracy percentage achieved across all experiments.
        /// </summary>
        public double AverageAccuracy { get; set; }

        /// <summary>
        /// Represents the configuration parameter indicating the maximum number of new synapses allowed per segment.
        /// </summary>
        public int MaxNewSynapseCount { get; set; }
    }
}
