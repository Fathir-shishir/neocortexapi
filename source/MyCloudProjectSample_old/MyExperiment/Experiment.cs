using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyCloudProject.Common;
using SEProject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
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

        private string expectedProjectName;
        /// <summary>
        /// construct the class
        /// </summary>
        /// <param name="configSection"></param>
        /// <param name="storageProvider"></param>
        /// <param name="expectedPrjName"></param>
        /// <param name="log"></param>
        public Experiment(IConfigurationSection configSection, IStorageProvider storageProvider, string expectedPrjName, ILogger log)
        {
            this.storageProvider = storageProvider;
            this.logger = log;
            this.expectedProjectName = expectedPrjName;
            config = new MyConfig();
            configSection.Bind(config);
        }

        /// <summary>
        /// Run Software Engineering project method
        /// </summary>
        /// <param name="inputFile">The input file.</param>
        /// <returns>experiment result object</returns>
        public Task<IExperimentResult> Run(string inputFile, string result, int mnsc1, int mnsc2)
        {
            Random rnd = new Random();
            int rowKeyNumber = rnd.Next(0, 1000);
            string rowKey = "team-as-" + rowKeyNumber.ToString();

            ExperimentResult res = new ExperimentResult(this.config.GroupId, rowKey);

            res.StartTimeUtc = DateTime.UtcNow;
            res.ExperimentId = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
            res.RowKey = rowKey;
            res.PartitionKey = "cc-proj-" + rowKey;

            if (inputFile == "run-team-as-project")
            {
                res.TestName = "Investigate Influence of parameter MaxNewSynapseCount";

                res.Description = "[Team AS Cloud Computing Implementation]";
                this.logger?.LogInformation($"The file result we got {"[Team AS Cloud Computing Implementation]"}");

                res.TestData = result;
                res.MaxNewSynapseCount1 = mnsc1;
                res.MaxNewSynapseCount2 = mnsc2;
            }
            res.EndTimeUtc = DateTime.UtcNow;

            this.logger?.LogInformation("The process successfully completed");
            return Task.FromResult<IExperimentResult>(res);
        }



        /// <summary>
        /// This method listens for incoming messages from the Azure Queue and processes each message by 
        /// running SE Project tests, uploading the test results and experiment data to Azure Blob Storage, 
        /// and updating the experiment result in Table Storage. It continuously checks the queue for new messages
        /// and stops processing when a cancellation request is triggered via the CancellationToken.
        /// </summary>
        /// <param name="cancelToken">The CancellationToken that signals when to stop listening to the queue.</param>
        /// <returns>A Task representing the asynchronous operation of processing messages from the queue.</returns>
        public async Task RunQueueListener(CancellationToken cancelToken)
        {
            //ExperimentResult res = new ExperimentResult("damir", "123")
            //{
            //    //Timestamp = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
                
            //    Accuracy = (float)0.5,
            //};

            //await storageProvider.UploadExperimentResult(res);


            QueueClient queueClient = new QueueClient(this.config.StorageConnectionString, this.config.Queue);

            //
            // Implements the step 3 in the architecture picture.
            while (cancelToken.IsCancellationRequested == false)
            {
                QueueMessage message = await queueClient.ReceiveMessageAsync();

                if (message != null)
                {
                    try
                    {

                        string msgTxt = Encoding.UTF8.GetString(message.Body.ToArray());

                        this.logger?.LogInformation($"Received the message {msgTxt}");

                        // The message in the step 3 on architecture picture.
                        ExerimentRequestMessage request = JsonSerializer.Deserialize<ExerimentRequestMessage>(msgTxt); 

                        // Step 4.
                        //var inputFile = await this.storageProvider.DownloadInputFile(request.InputFile);
                        var inputFile = request.InputFile;
                        var fileOne = request.file1;
                        var fileTwo = request.file2;



                        // Here is your SE Project code started.(Between steps 4 and 5).

                        List<string> resultOfTests = await this.runSEProject(fileOne);
                        ExperimentData experimentData = await this.getAndDeserializeDataFromBlobContainerAsync(fileOne);
                        string resultJsonString = JsonSerializer.Serialize(resultOfTests);

                        byte[] resultByteArray = Encoding.UTF8.GetBytes(resultJsonString);

                        // Step 4 (oposite direction)
                        //TODO. do serialization of the result.
                        //await storageProvider.UploadResultFile("outputfile.txt", null);

                        // Step 5.
                        //this.logger?.LogInformation($"{DateTime.Now} -  UploadExperimentResultFile...");
                        await storageProvider.UploadResultFile($"Test_data_{DateTime.UtcNow.ToString("yyyyMMddHHmmssfff")}.txt", resultByteArray);
                        IExperimentResult result = await this.Run(
                            inputFile,
                            resultJsonString, 
                            experimentData.MaxNewSynapseCount1, 
                            experimentData.MaxNewSynapseCount2
                        );

                        this.logger?.LogInformation($"{DateTime.Now} -  UploadExperimentResult...");
                        await storageProvider.UploadExperimentResult(result);
                        this.logger?.LogInformation($"{DateTime.Now} -  Experiment Completed Successfully...");

                        await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
                    }
                    catch (Exception ex)
                    {
                        this.logger?.LogError(ex, "TODO...");
                    }
                }
                else
                {
                    await Task.Delay(500);
                    logger?.LogTrace("Queue empty...");
                }
            }

            this.logger?.LogInformation("Cancel pressed. Exiting the listener loop.");
        }


        #region Private Methods

        /// <summary>
        /// Runs a series of SE Project tests using the provided file name.
        /// Extracts sequence learning results for different synapse counts.
        /// </summary>
        /// <param name="fileName">The name of the file containing experiment data.</param>
        /// <returns>A list of test result strings.</returns>
        private async Task<List<string>> runSEProject(string fileName)
        {
            // Create a new instance of SequenceLearningTests to run tests.
            SequenceLearningTests sequenceLearningTests = new SequenceLearningTests();

            // Retrieve experiment data from the file in blob storage.
            ExperimentData experimentData = await this.getAndDeserializeDataFromBlobContainerAsync(fileName);

            // Run prediction accuracy test.
            string test1 = sequenceLearningTests.PredictionAccuracyTest(
                experimentData.Sequences,
                experimentData.MaxNewSynapseCount1,
                experimentData.MaxNewSynapseCount2
            );

            // Run synapse count comparison test.
            string test2 = sequenceLearningTests.CompareLearningSpeedWithDifferentSynapseCounts(
                experimentData.Sequences,
                experimentData.MaxNewSynapseCount1,
                experimentData.MaxNewSynapseCount2
            );

            // Collect test results into a list.
            List<string> result = new List<string> { test1, test2 };

            // Simulate a delay before returning the results.
            await Task.Delay(500);
            return result;
        }

        /// <summary>
        /// Downloads and deserializes the experiment data from a blob container.
        /// </summary>
        /// <param name="fileName">The name of the file in the blob container.</param>
        /// <returns>An ExperimentData object containing the deserialized data.</returns>
        private async Task<ExperimentData> getAndDeserializeDataFromBlobContainerAsync(string fileName)
        {
            // Download the JSON file from blob storage.
            string jsonString = await storageProvider.DownloadInputFile(fileName);

            // Deserialize the JSON content into an ExperimentData object.
            return await DeserializeExperimentData(jsonString);
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


        #endregion
    }
}
