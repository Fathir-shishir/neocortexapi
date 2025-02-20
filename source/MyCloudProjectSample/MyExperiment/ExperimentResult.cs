using Azure;
using Azure.Data.Tables;
using MyCloudProject.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyExperiment
{
    /// <summary>
    /// Represents the result of an experiment stored as a table entity in Azure Table Storage.
    /// Implements both <see cref="ITableEntity"/> for Azure Table operations and <see cref="IExperimentResult"/> for experiment result contracts.
    /// </summary>
    public class ExperimentResult : ITableEntity, IExperimentResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExperimentResult"/> class.
        /// </summary>
        public ExperimentResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExperimentResult"/> class with the specified partition and row keys.
        /// </summary>
        /// <param name="partitionKey">The partition key for the table entity.</param>
        /// <param name="rowKey">The row key for the table entity.</param>
        public ExperimentResult(string partitionKey, string rowKey)
        {
            this.PartitionKey = partitionKey;
            this.RowKey = rowKey;
        }

        /// <summary>
        /// Gets or sets the partition key of the table entity.
        /// </summary>
        public string PartitionKey { get; set; }

        /// <summary>
        /// Gets or sets the row key of the table entity.
        /// </summary>
        public string RowKey { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the entity.
        /// This value is maintained by Azure Table Storage to track entity modifications.
        /// </summary>
        public DateTimeOffset? Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the entity tag (ETag) used for optimistic concurrency checks.
        /// </summary>
        public ETag ETag { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the experiment.
        /// </summary>
        public string ExperimentId { get; set; }

        /// <summary>
        /// Gets or sets the name of the experiment.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the experiment.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the UTC start time of the experiment.
        /// </summary>
        public DateTime? StartTimeUtc { get; set; }

        /// <summary>
        /// Gets or sets the UTC end time of the experiment.
        /// </summary>
        public DateTime? EndTimeUtc { get; set; }

        /// <summary>
        /// Gets or sets the sequence identifier associated with this experiment result.
        /// </summary>
        public string SequenceID { get; set; }

        /// <summary>
        /// Gets or sets the sequence number indicating the order of the experiment result.
        /// </summary>
        public int sequence { get; set; }

        /// <summary>
        /// Gets or sets the current status of the experiment.
        /// This could represent states such as "Running", "Completed", or "Failed".
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// Gets or sets the accuracy score of the experiment result.
        /// </summary>
        public float Accuracy { get; set; }

        /// <summary>
        /// Gets or sets the duration of the experiment.
        /// Represents the time span for which the experiment ran.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of new synapses recorded during the experiment.
        /// </summary>
        public int MaxNewSynapseCount { get; set; }
    }
}
