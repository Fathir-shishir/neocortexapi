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

        public async Task<IExerimentRequest> ReceiveExperimentRequestAsync(CancellationToken token)
        {
            // Receive the message and make sure that it is serialized to IExperimentResult.
            QueueClient queueClient = new QueueClient(this._config.StorageConnectionString, this._config.Queue);
            while (token.IsCancellationRequested == false)
            {

                QueueMessage message = await queueClient.ReceiveMessageAsync();
                if (message != null)
                {

                    try {
                        string msgTxt = Encoding.UTF8.GetString(message.Body.ToArray());
                        this.logger?.LogInformation($"Received the message {msgTxt}");

                        ExerimentRequestMessage request = JsonSerializer.Deserialize<ExerimentRequestMessage>(msgTxt);

                        var fileOne = request.file;

                        this.logger?.LogInformation($"Received file {fileOne}");
                    } catch (Exception)
                    {
                        throw new ApplicationException();
                    }
                
                }

            }
            throw new NotImplementedException();
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
