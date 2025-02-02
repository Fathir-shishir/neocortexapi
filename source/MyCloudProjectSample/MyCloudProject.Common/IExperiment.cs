using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyCloudProject.Common
{
    /// <summary>
    /// Defines the interface for experiment.
    /// </summary>
    public interface IExperiment
    {
        /// <summary>
        /// Runs the experiment.
        /// </summary>
        /// <param name="sequences">A dictionary of sequences for the experiment.</param>
        /// <param name="testList">A list of test lists for validation.</param>
        /// <param name="maxNewSynapseCount">The maximum number of new synapses allowed in the experiment.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of experiment results.</returns>
        Task<List<IExperimentResult>> RunAsync(Dictionary<string, List<double>> sequences, List<List<double>> testList, int maxNewSynapseCount);
    }
}