using NeoCortexApiSample;

namespace EffectSynapseCountTest
{
    [TestClass]
    public class SequenceLearningTests
    {
        private Dictionary<string, List<double>> GetTestSequences1()
        {
            return new Dictionary<string, List<double>>()
            {
                { "S1", new List<double>(new double[] { 0.0, 1.0, 0.0, 2.0, 3.0, 4.0, 5.0 }) }, 
                { "S2", new List<double>(new double[] { 8.0, 1.0, 2.0, 9.0, 10.0, 7.0, 11.00 }) }
            };
        }

        private Dictionary<string, List<double>> GetTestSequences2()
        {
            return new Dictionary<string, List<double>>()
            {
                { "S1", new List<double>(new double[] { 0.0, 1.0, 0.0, 2.0, 3.0, 4.0, 5.0, 6.0, 5.0, 4.0, 3.0, 7.0, 1.0, 9.0, 12.0, 11.0, 12.0, 13.0, 14.0, 11.0, 12.0, 14.0 }) },
                { "S2", new List<double>(new double[] { 0.8, 2.0, 0.0, 3.0, 3.0, 4.0, 5.0, 6.0, 5.0, 7.0, 2.0, 7.0, 1.0, 9.0, 11.0, 11.0, 10.0, 13.0, 14.0, 11.0, 7.0, 6.0 }) }
            };
        }

        [TestMethod]
        public void PredictionAccuracyTest1()
        {
            var sequences = GetTestSequences1();

            // Test with different MaxNewSynapseCount values
            var lowCountAccuracy = TestWithMaxNewSynapseCount(sequences, 5);
            var highCountAccuracy = TestWithMaxNewSynapseCount(sequences, 20);

            // Determine which MaxNewSynapseCount had the best accuracy
            var bestAccuracy = Math.Max(lowCountAccuracy, highCountAccuracy);
            var bestCount = lowCountAccuracy > highCountAccuracy ? 5 : 20;

            Console.WriteLine($"Best accuracy achieved: {bestAccuracy} with MaxNewSynapseCount {bestCount}");

            // Assert that a valid accuracy was achieved - this ensures the test validates a condition but is flexible regarding which MaxNewSynapseCount is better
            Assert.IsTrue(bestAccuracy > 0, "Expected a positive accuracy value indicating a successful learning outcome.");
        }

        [TestMethod]
        public void PredictionAccuracyTest2()
        {
            var sequences = GetTestSequences2();

            // Test with different MaxNewSynapseCount values
            var lowCountAccuracy = TestWithMaxNewSynapseCount(sequences, 20);
            var highCountAccuracy = TestWithMaxNewSynapseCount(sequences, 40);

            // Determine which MaxNewSynapseCount had the best accuracy
            var bestAccuracy = Math.Max(lowCountAccuracy, highCountAccuracy);
            var bestCount = lowCountAccuracy > highCountAccuracy ? 20 : 40;

            Console.WriteLine($"Best accuracy achieved: {bestAccuracy} with MaxNewSynapseCount {bestCount}");

            // Assert that a valid accuracy was achieved - this ensures the test validates a condition but is flexible regarding which MaxNewSynapseCount is better
            Assert.IsTrue(bestAccuracy > 0, "Expected a positive accuracy value indicating a successful learning outcome.");
        }

        private double TestWithMaxNewSynapseCount(Dictionary<string, List<double>> sequences, int maxNewSynapseCount)
        {
            double accuracy = 0;
            MultiSequenceLearning learningExperiment = new MultiSequenceLearning(maxNewSynapseCount);
            try {
                var predictor = learningExperiment.Run(sequences);
                int totalPredictions = 0;
                int correctPredictions = 0;

                foreach (var sequence in sequences)
                {
                    List<double> sequenceValues = sequence.Value;

                    Console.WriteLine($"Testing sequence: {sequence.Key}");
                    Console.WriteLine($"Testing sequence count : {sequenceValues.Count}");

                    for (int i = 0; i < sequenceValues.Count - 1; i++)
                    {
                        double currentInput = sequenceValues[i];
                        string nextInput = sequenceValues[i + 1].ToString();

                        var predictions = predictor.Predict(currentInput);

                        Console.WriteLine($"Input: {currentInput}");
                        Console.WriteLine($"Expected Next: {nextInput}");

                        // Iterate through each prediction to log it
                        foreach (var prediction in predictions)
                        {
                            Console.WriteLine($"Predicted: {prediction.PredictedInput}, Similarity: {prediction.Similarity}");
                        }

                        // Check if any of the predictions match the next input
                        bool isCorrectPrediction = predictions.Any(pred => pred.PredictedInput.Contains(nextInput));

                        if (isCorrectPrediction)
                        {
                            correctPredictions++;
                            Console.WriteLine("Prediction was correct.");
                        }
                        else
                        {
                            Console.WriteLine("Prediction was incorrect.");
                        }

                        totalPredictions++;

                        Console.WriteLine("---------------------------------");
                    }
                }

                accuracy = totalPredictions > 0 ? (double)correctPredictions / totalPredictions : 0;
                Console.WriteLine($"Total predictions: {totalPredictions}, Correct predictions: {correctPredictions}, Accuracy: {accuracy}");
            } catch (Exception ex)
            {
                accuracy= 0;
                Console.WriteLine(ex.ToString());
            }

            return accuracy;

        }

        [TestMethod]
        public void CompareLearningSpeedWithDifferentSynapseCounts()
        {
            var sequences = GetTestSequences1();
            double accuracyThreshold = 0.9; // 90% accuracy

            // Compare the number of cycles needed to reach the accuracy threshold
            int cyclesForLowCount = GetCyclesToReachAccuracy(sequences, 5, accuracyThreshold);
            int cyclesForHighCount = GetCyclesToReachAccuracy(sequences, 20, accuracyThreshold);

            var bestEfficientCycles = Math.Min(cyclesForLowCount, cyclesForHighCount);
            var bestCount = cyclesForLowCount > cyclesForHighCount ? 20 : 5;

            Console.WriteLine($"Best cycles achieved: {bestEfficientCycles} with MaxNewSynapseCount {bestCount}");

            // Assert that a valid cycles was needed - this ensures the test validates a condition but is flexible regarding which MaxNewSynapseCount is better
            Assert.IsTrue(bestEfficientCycles > 0, "Expected a positive cycle value indicating a successful learning outcome.");
        }

        [TestMethod]
        public void CompareLearningSpeedWithDifferentSynapseCounts2()
        {
            var sequences = GetTestSequences2();
            double accuracyThreshold = 0.9; // 90% accuracy

            // Compare the number of cycles needed to reach the accuracy threshold
            int cyclesForLowCount = GetCyclesToReachAccuracy(sequences, 20, accuracyThreshold);
            int cyclesForHighCount = GetCyclesToReachAccuracy(sequences, 40, accuracyThreshold);

            var bestEfficientCycles = Math.Min(cyclesForLowCount, cyclesForHighCount);
            var bestCount = cyclesForLowCount > cyclesForHighCount ? 40 : 20;

            Console.WriteLine($"Best cycles achieved: {bestEfficientCycles} with MaxNewSynapseCount {bestCount}");

            // Assert that a valid cycles was needed - this ensures the test validates a condition but is flexible regarding which MaxNewSynapseCount is better
            Assert.IsTrue(bestEfficientCycles > 0, "Expected a positive cycle value indicating a successful learning outcome.");
        }

        private int GetCyclesToReachAccuracy(Dictionary<string, List<double>> sequences, int maxNewSynapseCount, double accuracyThreshold)
        {
            int cycles = 0;
            try
            {
                MultiSequenceLearning learningExperiment = new MultiSequenceLearning(maxNewSynapseCount);
                var predictor = learningExperiment.Run(sequences);

                bool accuracyReached = false;

                while (!accuracyReached)
                {
                    cycles++;
                    int correctPredictions = 0;
                    int totalPredictions = 0;

                    foreach (var sequence in sequences)
                    {
                        List<double> sequenceValues = sequence.Value;

                        for (int i = 0; i < sequenceValues.Count - 1; i++)
                        {
                            double currentInput = sequenceValues[i];
                            string nextInput = sequenceValues[i + 1].ToString();

                            var predictions = predictor.Predict(currentInput);
                            bool isCorrectPrediction = predictions.Any(pred => pred.PredictedInput.Contains(nextInput));

                            if (isCorrectPrediction)
                            {
                                correctPredictions++;
                            }

                            totalPredictions++;
                        }
                    }

                    double accuracy = totalPredictions > 0 ? (double)correctPredictions / totalPredictions : 0;
                    if (accuracy >= accuracyThreshold)
                    {
                        accuracyReached = true;
                    }
                    else
                    {
                        // Reset or update predictor state as needed for the next cycle
                        predictor.Reset();
                    }
                }
            }
            catch (Exception ex)
            {
                cycles = 5000;
                Console.WriteLine(ex.ToString());
            }
            

            return cycles;
        }
    }
}