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
        public int maxNewSynapseCount { get; set; }

        /// <summary>
        /// Gets or sets the file path or name containing the input data for the experiment.
        /// </summary>
        public string file { get; set; }

        /// <summary>
        /// Gets or sets a unique identifier for the queue message itself, used for message tracking and deletion.
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// Gets or sets the receipt acknowledging that the message has been successfully received and processed.
        /// This is required to confirm and delete the message from the queue.
        /// </summary>
        public string MessageReceipt { get; set; }
    }
}