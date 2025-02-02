using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using MyCloudProject.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExperiment
{
    public class AzureStorageProvider : IStorageProvider
    {
        private MyConfig config;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureStorageProvider"/> class.
        /// The constructor binds the configuration settings passed via the configSection.
        /// </summary>
        /// <param name="configSection">The configuration section containing storage connection details.</param>
        public AzureStorageProvider(IConfigurationSection configSection)
        {
            config = new MyConfig();
            configSection.Bind(config);
        }

        /// <summary>
        /// Downloads a file from the Azure Blob Storage based on the given file name.
        /// It checks if the blob exists in the specified container and returns its content as a string.
        /// </summary>
        /// <param name="fileName">The name of the file to download from the blob storage.</param>
        /// <returns>A string containing the file's content.</returns>
        /// <exception cref="FileNotFoundException">Thrown when the file does not exist in the blob container.</exception>
        public async Task<string> DownloadInputFile(string fileName)
        {
            BlobContainerClient container = new BlobContainerClient(this.config.StorageConnectionString, this.config.TrainingContainer);
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
                    return await reader.ReadToEndAsync();
                }
            }
            else
            {
                throw new FileNotFoundException($"The file '{fileName}' does not exist in the blob container.");
            }
        }

        /// <summary>
        /// Uploads an experiment result to Azure Table Storage.
        /// This stores experiment metadata such as start/end times, test data, and description.
        /// </summary>
        /// <param name="result">An object implementing <see cref="IExperimentResult"/> containing the experiment data to upload.</param>
        public async Task UploadExperimentResult(IExperimentResult result)
        {
            Random rnd = new Random();
            int rowKeyNumber = rnd.Next(0, 1000);
            string rowKey = "team-as-" + rowKeyNumber.ToString();
            string partitionKey = "cc-proj-" + rowKey;

            var testResult = new ExperimentResult(partitionKey, rowKey)
            {
                ExperimentId = result.ExperimentId,
                Name = result.Name,
                Description = result.Description,
                StartTimeUtc = result.StartTimeUtc,
                EndTimeUtc = result.EndTimeUtc,
                TestData = result.TestData,
                MaxNewSynapseCount1 = result.MaxNewSynapseCount1,
                MaxNewSynapseCount2 = result.MaxNewSynapseCount2,
            };

            Console.WriteLine($"Upload ExperimentResult to table: {this.config.ResultTable}");
            var client = new TableClient(this.config.StorageConnectionString, this.config.ResultTable);

            await client.CreateIfNotExistsAsync();
            try
            {
                await client.AddEntityAsync<ExperimentResult>(testResult);
                Console.WriteLine("Uploaded to Table Storage completed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to upload to Table Storage: {ex.ToString()}");
            }
        }

        /// <summary>
        /// Uploads a result file (as a byte array) to Azure Blob Storage.
        /// The file is stored with the specified file name in the configured blob container.
        /// </summary>
        /// <param name="fileName">The name of the file to upload to the blob storage.</param>
        /// <param name="data">The byte array data representing the file content.</param>
        public async Task UploadResultFile(string fileName, byte[] data)
        {
            var experimentLabel = fileName;

            BlobServiceClient blobServiceClient = new BlobServiceClient(this.config.StorageConnectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(this.config.ResultContainer);

            string blobName = experimentLabel;

            // Upload the data as a blob
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                await blobClient.UploadAsync(memoryStream);
            }
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

            BlobServiceClient blobServiceClient = new BlobServiceClient(this.config.StorageConnectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(this.config.ResultContainer);

            string blobName = experimentLabel;

            // Upload the data as a blob
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(memoryStream);
        }
    }



}
