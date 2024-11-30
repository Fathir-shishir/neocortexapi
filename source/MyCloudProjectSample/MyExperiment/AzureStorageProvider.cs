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

        public Task CommitRequestAsync(IExerimentRequest request)
        {
            throw new NotImplementedException();
        }

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
                    request.ExperimentId = message.MessageId;
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


        public Task UploadExperimentResult(IExperimentResult result)
        {
            throw new NotImplementedException();
        }

        public Task UploadResultAsync(string experimentName, IExperimentResult result)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Uploads a result file (from a MemoryStream) to Azure Blob Storage.
        /// This method directly uploads data stored in the MemoryStream to the specified blob container.
        /// </summary>
        /// <param name="fileName">The name of the file to upload to the blob storage.</param>
        /// <param name="memoryStream">The MemoryStream containing the file data.</param>
        public async Task UploadResultFile(string fileName, MemoryStream memoryStream)
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
