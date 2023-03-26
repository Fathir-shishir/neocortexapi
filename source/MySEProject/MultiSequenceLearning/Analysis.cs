using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiSequenceLearning
{
    public class Analysis
    {
        public int Cycle { get; set; }
        public string SequenceName { get; set; }
        public double Accuracy { get; set; }
        public int ActivatePredictedColumnCalls { get; set; }
        public int ActivatePredictedColumnNewSynapseCount { get; set; }
        public int BurstColumnWithMatchingSegmentsCalls { get; set; }
        public int BurstColumnWithMatchingSegmentsNewSynapseCount { get; set; }
        public int BurstColumnWithoutMatchingSegmentsCalls { get; set; }
        public int BurstColumnWithoutMatchingSegmentsNewSynapseCount { get; set; }

        public Analysis() 
        {
            this.Cycle = 0;
            this.Accuracy = 0;
            this.ActivatePredictedColumnCalls = 0;
            this.ActivatePredictedColumnNewSynapseCount = 0;
            this.BurstColumnWithMatchingSegmentsCalls = 0;
            this.BurstColumnWithMatchingSegmentsNewSynapseCount = 0;
            this.BurstColumnWithoutMatchingSegmentsCalls = 0;
            this.BurstColumnWithoutMatchingSegmentsNewSynapseCount = 0;
        }
    }
}
