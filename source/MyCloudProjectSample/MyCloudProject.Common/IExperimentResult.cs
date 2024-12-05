
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCloudProject.Common
{
    public interface IExperimentResult
    {
        string ExperimentId { get; set; }

        DateTime? StartTimeUtc { get; set; }

        DateTime? EndTimeUtc { get; set; }

        public TimeSpan Duration { get; set; }

        string SequenceID { get; set; }

        int sequence { get; set; }

        string status { get; set; }

        float Accuracy { get; set; }

        int MaxNewSynapseCount { get; set; }
    }

}
