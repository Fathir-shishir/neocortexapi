using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyCloudProject.Common;
using NeoCortexApi.Utility;
using SEProject;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MyExperiment
{
    /// <summary>
    /// This class implements the ML experiment that will run in the cloud. This is refactored code from my SE project.
    /// </summary>
    public class Experiment : IExperiment
    {
        private IStorageProvider storageProvider;

        private ILogger logger;

        private MyConfig config;

        public Experiment(IConfigurationSection configSection, IStorageProvider storageProvider, ILogger log)
        {
            this.storageProvider = storageProvider;
            this.logger = log;

            config = new MyConfig();
            configSection.Bind(config);
        }


        public Task<IExperimentResult> RunAsync(Dictionary<string, List<double>> sequences, List<List<double>> testList, int maxNewSynapseCount)
        {

            ExperimentResult res = new ExperimentResult(this.config.GroupId, null);

            logger?.LogInformation("cc started running........");
            logger?.LogInformation($"Variable testing '{testList}' sequence '{sequences}'");

            res.StartTimeUtc = DateTime.UtcNow;

            // Run your experiment code here.


            return Task.FromResult<IExperimentResult>(res);
        }

        public async Task<IExperimentResult> runSEProject(Dictionary<string, List<double>> sequences, List<List<double>> testList, int maxNewSynapseCount) {

            Program1 program = new Program1();
            program.RunMultiSequenceLearningExperiment( maxNewSynapseCount, sequences, testList);

        } 
    }
}
