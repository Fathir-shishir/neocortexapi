using Azure;
using Azure.Core;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyCloudProject.Common;
using System;
using System.Collections.Generic;
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

        public Task<string> DownloadInputAsync(string fileName)
        {
            throw new NotImplementedException();
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

                        var inputFile = request.InputFile;
                        var fileOne = request.file1;
                        var fileTwo = request.file2;

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
    }


}
