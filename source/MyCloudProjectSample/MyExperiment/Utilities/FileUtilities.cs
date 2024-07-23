using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExperiment.Utilities
{
    public class FileUtilities
    {
        /// <summary>
        /// Retreive localstorage file path location
        /// </summary>
        /// <param name="localfilePath"></param>
        /// <returns></returns>
        public static string GetLocalStorageFilePath(string localFilePath)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                localFilePath);
        }
    }
}
