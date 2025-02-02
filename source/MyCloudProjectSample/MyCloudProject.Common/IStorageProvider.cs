using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyCloudProject.Common
{
    /// <summary>
    /// Defines the contract for all storage operations.
    /// </summary>
    public interface IStorageProvider
    {
        /// <summary>
        /// Receives the next message from the queue.
        /// </summary>
        /// <param name="token"></param>
        /// <returns>NULL if there are no messages in the queue.</returns>
        Task<IExerimentRequest> ReceiveExperimentRequestAsync(CancellationToken token);

        /// <summary>
        /// Downloads the input file for training. This file contains all required input for the experiment.
        /// The file is stored in the cloud or any other kind of store or database.
        /// </summary>
        /// <param name="fileName">The name of the file at some remote (cloud) location from where the file will be downloaded.</param>
        /// <returns>The fullpath name of the file as downloaded locally.</returns>
        /// <remarks>See step 4 in the architecture picture.</remarks>
        Task<string> DownloadInputAsync(string fileName);

        /// <summary>
        /// Uploads the result of the experiment to a table
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        Task UploadExperimentResult(IExperimentResult result);

        /// <summary>
        /// Makes sure that the message is deleted from the queue.
        /// </summary>
        /// <param name="request">The requests received by <see cref="nameof(IStorageProvider.ReceiveExperimentRequestAsync)"/>.</param>
        /// <returns></returns>
        Task CommitRequestAsync(IExerimentRequest request);

        /// <summary>
        /// Uploads the result of the experiment in the cloud or any other kind of store or database.
        /// </summary>
        /// <param name="result">The result of the experiment that should be uploaded to the table.</param>
        /// <remarks>See step 5 (oposite way) in the architecture picture.</remarks>
        Task UploadResultAsync(string fileName, MemoryStream memoryStream);

        /// <summary>
        /// Uploads the efficiency result of the experiment in the cloud or any other kind of store or database.
        /// </summary>
        /// <remarks>See step 5 (oposite way) in the architecture picture.</remarks>
        Task UploadEfficiencyResultAsync(IExerimentRequest request);

    }
}
