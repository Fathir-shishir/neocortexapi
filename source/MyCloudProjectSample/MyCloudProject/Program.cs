using MyCloudProject.Common;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Threading;
using MyExperiment;
using System.Threading.Tasks;
using Azure.Storage.Queues.Models;
using Azure.Storage.Queues;
using System.Text.Json;
using System.Text;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.IO;

namespace MyCloudProject
{
    class Program
    {
        private ILogger logger;
        /// <summary>
        /// Your project ID from the last semester.
        /// </summary>
        private static string _projectName = "ML22/23-13 Investigate Influence of parameter MaxNewSynapseCount";

        static async Task Main(string[] args)
        {

            CancellationTokenSource tokeSrc = new CancellationTokenSource();

            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true;
                tokeSrc.Cancel();
            };

            Console.WriteLine($"Started experiment: {_projectName}");

            // Init configuration
            var cfgRoot = Common.InitHelpers.InitConfiguration(args);

            var cfgSec = cfgRoot.GetSection("MyConfig");

            // InitLogging
            var logFactory = InitHelpers.InitLogging(cfgRoot);
          
            var logger = logFactory.CreateLogger("Train.Console");

            logger?.LogInformation($"{DateTime.Now} -  Started experiment: {_projectName}");

            IStorageProvider storageProvider = new AzureStorageProvider(cfgSec);

            IExperiment experiment = new Experiment(cfgSec, storageProvider, logger);

            //
            // Implements the step 3 in the architecture picture.
            while (!tokeSrc.Token.IsCancellationRequested)
            {
                // Step 3
                Task<IExerimentRequest> requestTask = storageProvider.ReceiveExperimentRequestAsync(tokeSrc.Token);
                IExerimentRequest request = await requestTask;

                if (request != null)
                {
                    try
                    {
                        logger?.LogInformation($"{DateTime.Now} -  In to the experiment...");

                        // Step 4.
                        string fileCFontent = await storageProvider.DownloadInputAsync(request.file);

                        logger?.LogInformation($"{fileCFontent} -  File content log");

                        ExperimentData eData = await getAndDeserializeDataFromBlobContainerAsync(fileCFontent);

                        logger?.LogInformation($"1st 1st MaxNewSynapseCount value '{eData.MaxNewSynapseCount}'");

                        // Here is your SE Project code started.(Between steps 4 and 5).
                        List<IExperimentResult> results = await experiment.RunAsync(eData.Sequences, eData.TestLists, eData.MaxNewSynapseCount);

                        // Step 5.
                        foreach (var result in results)
                        {
                            await storageProvider.UploadExperimentResult(result);
                        }

                        foreach (var result in results)
                        {
                            string fileName = GenerateFileName(result);

                            // Step 6.2: Convert the result to MemoryStream
                            using (MemoryStream memoryStream = ConvertResultToMemoryStream(result))
                            {
                                await storageProvider.UploadResultAsync(fileName, memoryStream);
                            }
                        }


                        await storageProvider.CommitRequestAsync(request);
                    }
                    catch (Exception ex)
                    {
                        logger?.LogError($"This happened: '{ex.Message}'");
                    }
                }
                else
                {
                    await Task.Delay(500);
                    logger?.LogTrace("Queue empty...");
                }
            }

            logger?.LogInformation($"{DateTime.Now} -  Experiment exit: {_projectName}");
        }

        /// <summary>
        /// Downloads and deserializes the experiment data from a blob container.
        /// </summary>
        /// <param name="fileName">The name of the file in the blob container.</param>
        /// <returns>An ExperimentData object containing the deserialized data.</returns>
        public static async Task<ExperimentData> getAndDeserializeDataFromBlobContainerAsync(string fileContent)
        {
            // Deserialize the JSON content into an ExperimentData object.
            return await DeserializeExperimentData(fileContent);
        }

        /// <summary>
        /// Deserializes the experiment data from a JSON string.
        /// </summary>
        /// <param name="jsonString">The JSON string containing experiment data.</param>
        /// <returns>A deserialized ExperimentData object.</returns>
        private static async Task<ExperimentData> DeserializeExperimentData(string jsonString)
        {
            return JsonSerializer.Deserialize<ExperimentData>(jsonString);
        }

        /// <summary>
        /// Generates a unique and readable file name based on IExperimentResult values.
        /// </summary>
        private static string GenerateFileName(IExperimentResult result)
        {
            string timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
            string experimentId = result.ExperimentId ?? "Experiment";
            string sequenceId = result.SequenceID ?? "UnknownSequence";
            string status = result.status ?? "UnknownStatus";

            return $"{experimentId}_{sequenceId}_{status}_{timestamp}.json";
        }

        /// <summary>
        /// Converts IExperimentResult object into a MemoryStream.
        /// </summary>
        private static MemoryStream ConvertResultToMemoryStream(IExperimentResult result)
        {
            string jsonContent = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(memoryStream);

            writer.Write(jsonContent);
            writer.Flush();
            memoryStream.Position = 0;

            return memoryStream;
        }


    }
}
