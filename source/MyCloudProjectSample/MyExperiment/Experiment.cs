using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyCloudProject.Common;
using NeoCortexApi.Utility;
using SEProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MyExperiment
{
    /// <summary>
    /// This class implements the ML experiment that will run in the cloud.
    /// </summary>
    public class Experiment : IExperiment
    {
        private readonly IStorageProvider storageProvider;
        private readonly ILogger logger;
        private readonly MyConfig config;

        public Experiment(IConfigurationSection configSection, IStorageProvider storageProvider, ILogger log)
        {
            this.storageProvider = storageProvider;
            this.logger = log;
            config = new MyConfig();
            configSection.Bind(config);
        }

        /// <summary>
        /// Runs the experiment asynchronously.
        /// </summary>
        public async Task<List<IExperimentResult>> RunAsync(Dictionary<string, List<double>> sequences, List<List<double>> testList, int maxNewSynapseCountValue)
        {
            var overallResults = new List<IExperimentResult>();
            logger?.LogInformation("Experiment started...");
            logger?.LogInformation($"Test list: '{JsonSerializer.Serialize(testList)}'");
            logger?.LogInformation($"Sequences: '{JsonSerializer.Serialize(sequences)}'");

            try
            {
                Dictionary<string, (string CycleID, int CycleCount, double Accuracy, TimeSpan Duration, string Status)> sequenceResults;
                RunMultiSequenceLearningExperiment(maxNewSynapseCountValue, sequences, testList, out sequenceResults);

                foreach (var sequenceKey in sequenceResults.Keys)
                {
                    var (cycleID, cycleCount, accuracy, duration, status) = sequenceResults[sequenceKey];

                    var sequenceResult = new ExperimentResult(config.GroupId, Guid.NewGuid().ToString())
                    {
                        ExperimentId = sequenceKey,
                        StartTimeUtc = DateTime.UtcNow,
                        EndTimeUtc = DateTime.UtcNow.Add(duration), // Adjust as needed
                        Duration = duration,
                        SequenceID = sequenceKey,
                        sequence = cycleCount,
                        status = status,
                        Accuracy = (float)accuracy,
                        MaxNewSynapseCount = maxNewSynapseCountValue
                    };

                    logger?.LogInformation($"Processed Sequence: {sequenceKey}");
                    logger?.LogInformation($"  Cycle ID: {cycleID}");
                    logger?.LogInformation($"  Cycle Count: {cycleCount}");
                    logger?.LogInformation($"  Accuracy: {accuracy:F2}%");
                    logger?.LogInformation($"  Duration: {duration}");
                    logger?.LogInformation($"  Status: {status}");
                    logger?.LogInformation($"  Status: {maxNewSynapseCountValue}");

                    overallResults.Add(sequenceResult);
                }

                logger?.LogInformation("Experiment completed successfully.");
            }
            catch (Exception ex)
            {
                logger?.LogError($"Error running experiment: {ex.Message}");

                // Handle failure case
                overallResults.Add(new ExperimentResult(config.GroupId, Guid.NewGuid().ToString())
                {
                    ExperimentId = "Overall",
                    StartTimeUtc = DateTime.UtcNow,
                    EndTimeUtc = DateTime.UtcNow,
                    Duration = TimeSpan.Zero,
                    SequenceID = "Failed",
                    sequence = 0,
                    status = "Failed",
                    Accuracy = 0,
                    MaxNewSynapseCount = maxNewSynapseCountValue
                });
            }

            // Return the per-sequence results
            return await Task.FromResult(overallResults);
        }

        /// <summary>
        /// Runs the multi-sequence learning experiment.
        /// </summary>
        private void RunMultiSequenceLearningExperiment(
            int maxNewSynapseCountValue,
            Dictionary<string, List<double>> sequences,
            List<List<double>> testList,
            out Dictionary<string, (string CycleID, int CycleCount, double Accuracy, TimeSpan Duration, string Status)> sequenceResults)
        {
            Program1 program = new Program1();
            logger?.LogInformation($"1st call maxNewSynapseCount: {maxNewSynapseCountValue}");
            program.RunMultiSequenceLearningExperiment(maxNewSynapseCountValue, sequences, testList, out sequenceResults);
        }

        /// <summary>
        /// Processes and logs the results of the sequence learning experiment.
        /// </summary>
        private void ProcessSequenceResults(
            Dictionary<string, (string CycleID, int CycleCount, double Accuracy, TimeSpan Duration, string Status)> sequenceResults,
            ExperimentResult overallResult)
        {
            var aggregatedDescription = new StringBuilder();
            TimeSpan totalDuration = TimeSpan.Zero;
            int sequenceCount = sequenceResults.Count;

            foreach (var item in sequenceResults)
            {
                string sequenceKey = item.Key;
                var (cycleID, cycleCount, accuracy, duration, status) = item.Value;

                logger?.LogInformation($"Sequence Key: {sequenceKey}");
                logger?.LogInformation($"  Cycle ID: {cycleID}");
                logger?.LogInformation($"  Cycle Count: {cycleCount}");
                logger?.LogInformation($"  Accuracy: {accuracy:F2}%");
                logger?.LogInformation($"  Duration: {duration}");
                logger?.LogInformation($"  Status: {status}");

                overallResult.Description = aggregatedDescription.AppendLine($"Sequence {sequenceKey}: Status={status}, Accuracy={accuracy:F2}%, Duration={duration}").ToString();
                overallResult.Accuracy = (float)accuracy;
                overallResult.Duration = totalDuration;
            }

        }
    }

    public class SequenceResult
    {
        public string CycleID { get; set; }
        public int CycleCount { get; set; }
        public double Accuracy { get; set; }
        public TimeSpan Duration { get; set; }
        public string Status { get; set; }
        public int MaxNewSynapsCount { get; set; }
    }
}
