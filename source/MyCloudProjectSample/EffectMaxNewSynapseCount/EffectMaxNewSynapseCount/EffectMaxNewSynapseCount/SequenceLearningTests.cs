using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoCortexApi;
using NeoCortexApi.Entities;
using NeoCortexApiSample;
using System.Collections.Generic;
using System.Linq;

namespace EffectMaxNewSynapseCount
{
    [TestClass]
    public class SequenceLearningTests
    {
        private Dictionary<string, List<double>> GetTestSequences()
        {
            return new Dictionary<string, List<double>>()
            {
                { "S1", new List<double>(new double[] { 0.0, 1.0, 0.0, 2.0, 3.0, 4.0, 5.0, 6.0, 5.0, 4.0, 3.0, 7.0, 1.0, 9.0, 12.0, 11.0, 12.0, 13.0, 14.0, 11.0, 12.0, 14.0 }) },
                { "S2", new List<double>(new double[] { 0.8, 2.0, 0.0, 3.0, 3.0, 4.0, 5.0, 6.0, 5.0, 7.0, 2.0, 7.0, 1.0, 9.0, 11.0, 11.0, 10.0, 13.0, 14.0, 11.0, 7.0, 6.0 }) }
            };
        }

        [TestMethod]
        public void PredictionAccuracyTest()
        {
            var sequences = GetTestSequences();

            // Test with different MaxNewSynapseCount values
            var lowCountAccuracy = TestWithMaxNewSynapseCount(sequences, 15);
            var highCountAccuracy = TestWithMaxNewSynapseCount(sequences, 409);

            Assert.IsTrue(lowCountAccuracy < highCountAccuracy, "Expected higher accuracy with a higher MaxNewSynapseCount.");
        }

        private double TestWithMaxNewSynapseCount(Dictionary<string, List<double>> sequences, int maxNewSynapseCount)
        {
            // Assuming MultiSequenceLearning can now accept MaxNewSynapseCount as part of its configuration
            MultiSequenceLearning learningExperiment = new MultiSequenceLearning(maxNewSynapseCount);
            var predictor = learningExperiment.Run(sequences);

            // Logic to calculate prediction accuracy
            int totalPredictions = 0;
            int correctPredictions = 0;

            foreach (var sequence in sequences.Values)
            {
                for (int i = 0; i < sequence.Count - 1; i++)
                {
                    var currentInput = sequence[i];
                    var nextInput = sequence[i + 1];

                    var predictions = predictor.Predict(currentInput);

                    // Convert the next input to a string once, outside the loop, to improve efficiency.
                    string nextInputStr = nextInput.ToString();

                    // Check if any of the predictions match the next input.
                    bool isCorrectPrediction = predictions.Any(pred => pred.PredictedInput.Equals(nextInputStr));

                    if (isCorrectPrediction)
                    {
                        correctPredictions++;
                    }

                    totalPredictions++;
                }
            }

            return (double)correctPredictions / totalPredictions;
        }
    }
} 

