namespace MyCloudProject.Common
{
    /// <summary>
    /// Defines the contract for the message request that will trigger and configure an experiment execution.
    /// </summary>
    public interface IExerimentRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier for the experiment.
        /// </summary>
        public string ExperimentId { get; set; }

        /// <summary>
        /// Gets or sets the name of the experiment.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the experiment, providing additional details.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of new synapses that can be formed during the experiment.
        /// </summary>
        public int MaxNewSynapseCount { get; set; }

        /// <summary>
        /// Gets or sets the file path or name containing the input data for the experiment.
        /// </summary>
        public string file { get; set; }

        /// <summary>
        /// Gets or sets the operation type wheather to calculate the efficiency or the the sequence operation 
        /// </summary>
        public string OperationType { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the queue message associated with the experiment request.
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// Gets or sets the pop receipt acknowledging the retrieval of the queue message.
        /// </summary>
        public string PopReceipt { get; set; }
    }
}