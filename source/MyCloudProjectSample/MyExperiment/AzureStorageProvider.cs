using Azure;
using Azure.Core;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyCloudProject.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MyExperiment
{
    public class AzureStorageProvider : IStorageProvider
    {
        private MyConfig _config;
        private ILogger logger;

        public AzureStorageProvider(IConfigurationSection configSection)
        {
            _config = new MyConfig();
            configSection.Bind(_config);
        }

        /// <summary>
        /// Commits the experiment request by deleting the message from the queue.
        /// </summary>
        /// <param name="request">The experiment request to commit.</param>
        public async Task CommitRequestAsync(IExerimentRequest request)
        {
            var queueClient = new QueueClient(_config.StorageConnectionString, _config.Queue);
            await queueClient.DeleteMessageAsync(request.MessageId, request.MessageReceipt);
        }

        /// <summary>
        /// Downloads the content of a file (blob) from an Azure Blob Storage container as a string.
        /// </summary>
        /// <param name="fileName">The name of the file (blob) to be downloaded.</param>
        /// <returns>
        /// A <see cref="Task{string}"/> representing the asynchronous operation, 
        /// which resolves to the file content as a string if the download is successful.
        /// </returns>
        /// <exception cref="FileNotFoundException">
        /// Thrown when the specified file does not exist in the Azure Blob Storage container.
        /// </exception>
        /// <remarks>
        /// This method ensures the specified blob container exists, retrieves the blob (file),
        /// and downloads its content as a stream, converting it to a string.
        /// </remarks>
        /// <example>
        /// Example usage:
        /// <code>
        /// string fileName = "inputData.txt";
        /// string content = await storageProvider.DownloadInputAsync(fileName);
        /// Console.WriteLine(content);
        /// </code>
        /// </example>
        public async Task<string> DownloadInputAsync(string fileName)
        {
            BlobContainerClient container = new BlobContainerClient(this._config.StorageConnectionString, this._config.TrainingContainer);
            await container.CreateIfNotExistsAsync();

            // Get a reference to the blob
            BlobClient blob = container.GetBlobClient(fileName);

            // Check if the blob exists
            if (await blob.ExistsAsync())
            {
                // Download the blob content as a stream
                BlobDownloadInfo download = await blob.DownloadAsync();

                // Read the content from the stream and return it as a string
                using (StreamReader reader = new StreamReader(download.Content))
                {
                    string fileContent = await reader.ReadToEndAsync();
                    return fileContent;
                }
            }
            else
            {
                throw new FileNotFoundException($"The file '{fileName}' does not exist in the blob container.");
            }
        }

        /// <summary>
        /// Receives an experiment request from the Azure queue asynchronously.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation, returning the experiment request received from the queue.</returns>
        public async Task<IExerimentRequest> ReceiveExperimentRequestAsync(CancellationToken token)
        {
            QueueClient queueClient = new QueueClient(_config.StorageConnectionString, _config.Queue);

            // Receive a message from the queue
            QueueMessage message = await queueClient.ReceiveMessageAsync();

            if (message != null)
            {
                try
                {
                    // Process the received message
                    string msgTxt = Encoding.UTF8.GetString(message.Body.ToArray());
                    ExerimentRequestMessage request = JsonSerializer.Deserialize<ExerimentRequestMessage>(msgTxt);
                    return request;
                }
                catch (JsonException jsonEx)
                {
                    logger?.LogError(jsonEx, "JSON deserialization failed for the message");
                    Console.Error.WriteLine("The message sent is not correctly formatted. Please send another message.");
                }
            }
            else
            {
                logger?.LogInformation("The message is null");
            }

            return null;
        }

        /// <summary>
        /// Uploads an experiment result to Azure Table Storage.
        /// </summary>
        /// <param name="result">An object implementing the <see cref="IExperimentResult"/> interface, containing the details of the experiment result.</param>
        /// <remarks>
        /// This method serializes the provided experiment result into an <see cref="ExperimentResult"/> object and uploads it to a specified Azure Table Storage table.
        /// A unique row key is generated for each entry to ensure uniqueness. The method also creates the table if it does not exist.
        /// </remarks>
        /// <example>
        /// Example usage:
        /// <code>
        /// var result = new ExperimentResult
        /// {
        ///     ExperimentId = "1234",
        ///     SequenceID = "S1_1",
        ///     StartTimeUtc = DateTime.UtcNow,
        ///     EndTimeUtc = DateTime.UtcNow.AddMinutes(5),
        ///     MaxNewSynapseCount = 20,
        ///     sequence = 100,
        ///     status = "Completed",
        ///     Accuracy = 95.0f,
        ///     Duration = TimeSpan.FromMinutes(5)
        /// };
        /// await storageProvider.UploadExperimentResult(result);
        /// </code>
        /// </example>
        /// <exception cref="System.Exception">
        /// Throws an exception if the upload to Azure Table Storage fails.
        /// </exception>
        public async Task UploadExperimentResult(IExperimentResult result)
        {
            string rowKey = $"{Guid.NewGuid()}";
            string partitionKey = "team-as-" + rowKey;

            var testResult = new ExperimentResult(partitionKey, rowKey)
            {
                ExperimentId = result.ExperimentId,
                Name = result.SequenceID,
                Description = result.SequenceID,
                StartTimeUtc = result.StartTimeUtc,
                EndTimeUtc = result.EndTimeUtc,
                MaxNewSynapseCount = result.MaxNewSynapseCount,
                sequence = result.sequence,
                status = result.status,
                Accuracy = result.Accuracy,
                Duration = result.Duration,
            };

            Console.WriteLine($"Upload ExperimentResult to table: {this._config.ResultTable}");
            var client = new TableClient(_config.StorageConnectionString, _config.ResultTable);

            await client.CreateIfNotExistsAsync();
            try
            {
                await client.UpsertEntityAsync<ExperimentResult>(testResult);
                Console.WriteLine("Uploaded to Table Storage completed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to upload to Table Storage: {ex.ToString()}");
            }
        }

        /// <summary>
        /// Uploads a result file (from a MemoryStream) to Azure Blob Storage.
        /// This method directly uploads data stored in the MemoryStream to the specified blob container.
        /// </summary>
        /// <param name="fileName">The name of the file to upload to the blob storage.</param>
        /// <param name="memoryStream">The MemoryStream containing the file data.</param>
        public async Task UploadResultAsync(string fileName, MemoryStream memoryStream)
        {
            var experimentLabel = fileName;

            BlobServiceClient blobServiceClient = new BlobServiceClient(this._config.StorageConnectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(this._config.ResultContainer);

            string blobName = experimentLabel;

            // Upload the data as a blob
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(memoryStream);
        }


    }


}
