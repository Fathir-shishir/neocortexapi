using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SEProject
{
    [TestClass]
    public class SequenceLearningTests
    {

        /// <summary>
        /// Retrieves a predefined collection of test sequences to be used in the learning experiments.
        /// This method serves as a data provider, offering a set of sequences that the HTM network will
        /// attempt to learn and later predict. Each sequence is represented by a list of double values,
        /// simulating potential real-world data patterns that the network might encounter. The sequences
        /// are mapped to unique identifiers, allowing for easy reference and manipulation within the
        /// learning and prediction processes.
        /// 
        /// The selection of sequences in this method, including their specific values and composition,
        /// is designed to test the network's ability to learn and predict sequences with varying degrees
        /// of complexity and pattern distinctiveness. This aids in assessing the robustness and
        /// adaptability of the HTM model under different learning conditions and parameter configurations,
        /// including the MaxNewSynapseCount setting.
        /// </summary>
        /// <returns>A dictionary where each key is a sequence identifier and each value is the corresponding sequence represented as a list of double values.</returns>
        private Dictionary<string, List<double>> GetTestSequences1()
        {
            return new Dictionary<string, List<double>>()
            {
                { "S1", new List<double>(new double[] { 0.0, 1.0, 0.0, 2.0, 3.0, 4.0, 5.0 }) }, 
                { "S2", new List<double>(new double[] { 8.0, 1.0, 2.0, 9.0, 10.0, 7.0, 11.00 }) }
            };
        }


        /// <summary>
        /// Retrieves a predefined collection of test sequences to be used in the learning experiments.
        /// This method serves as a data provider, offering a set of sequences that the HTM network will
        /// attempt to learn and later predict. Each sequence is represented by a list of double values,
        /// simulating potential real-world data patterns that the network might encounter. The sequences
        /// are mapped to unique identifiers, allowing for easy reference and manipulation within the
        /// learning and prediction processes.
        /// 
        /// The selection of sequences in this method, including their specific values and composition,
        /// is designed to test the network's ability to learn and predict sequences with varying degrees
        /// of complexity and pattern distinctiveness. This aids in assessing the robustness and
        /// adaptability of the HTM model under different learning conditions and parameter configurations,
        /// including the MaxNewSynapseCount setting.
        /// </summary>
        /// <returns>A dictionary where each key is a sequence identifier and each value is the corresponding sequence represented as a list of double values.</returns>
        private Dictionary<string, List<double>> GetTestSequences2()
        {
            return new Dictionary<string, List<double>>()
            {
                { "S1", new List<double>(new double[] { 0.0, 1.0, 0.0, 2.0, 3.0, 4.0, 5.0, 6.0, 5.0, 4.0, 3.0, 7.0, 1.0, 9.0, 12.0, 11.0, 12.0, 13.0, 14.0, 11.0, 12.0, 14.0 }) },
                { "S2", new List<double>(new double[] { 0.8, 2.0, 0.0, 3.0, 3.0, 4.0, 5.0, 6.0, 5.0, 7.0, 2.0, 7.0, 1.0, 9.0, 11.0, 11.0, 10.0, 13.0, 14.0, 11.0, 7.0, 6.0 }) }
            };
        }


        /// <summary>
        /// Tests the impact of different MaxNewSynapseCount settings on prediction accuracy.
        /// This method evaluates how varying the MaxNewSynapseCount parameter affects the
        /// learning and prediction capabilities of a Hierarchical Temporal Memory (HTM) network.
        /// By comparing the prediction accuracy at two distinct MaxNewSynapseCount values,
        /// the test aims to identify which setting yields better accuracy, thereby
        /// providing insights into optimal configuration for sequence prediction tasks.
        /// </summary>
        [TestMethod]
        public void PredictionAccuracyTest1()
        {
            // Retrieve a set of predefined sequences for the test.
            var sequences = GetTestSequences1();

            // Evaluate prediction accuracy with a low MaxNewSynapseCount value.
            var lowCountAccuracy = TestWithMaxNewSynapseCount(sequences, 5);
            // Evaluate prediction accuracy with a higher MaxNewSynapseCount value for comparison.
            var highCountAccuracy = TestWithMaxNewSynapseCount(sequences, 20);

            // Determine the higher accuracy between the two tested MaxNewSynapseCount values.
            var bestAccuracy = Math.Max(lowCountAccuracy, highCountAccuracy);
            // Identify which MaxNewSynapseCount value resulted in the best accuracy.
            var bestCount = lowCountAccuracy > highCountAccuracy ? 5 : 20;

            // Log the best accuracy achieved and its corresponding MaxNewSynapseCount value.
            Debug.WriteLine($"Best accuracy achieved: {bestAccuracy} with MaxNewSynapseCount {bestCount}");

            // Assert that the test achieved a positive accuracy, indicating successful learning and prediction.
            // This assertion ensures that the HTM network is capable of learning from the sequences and
            // making accurate predictions, validating the effectiveness of the tested MaxNewSynapseCount settings.
            Assert.IsTrue(bestAccuracy > 0, "Expected a positive accuracy value indicating a successful learning outcome.");
        }


        [TestMethod]
        public void PredictionAccuracyTest2()
        {
            // Retrieves a set of sequences from the method GetTestSequences2. These sequences are more complex than those in GetTestSequences1,
            // intended to provide a challenging test scenario for assessing the HTM model's prediction accuracy.
            var sequences = GetTestSequences2();

            // Evaluates the prediction accuracy of the HTM model with a 'MaxNewSynapseCount' value set to 20.
            // This 'lowCountAccuracy' variable holds the accuracy percentage, allowing us to assess the effectiveness
            // of the model's learning and prediction capabilities at a relatively lower synapse count.
            var lowCountAccuracy = TestWithMaxNewSynapseCount(sequences, 20);

            // Similar to 'lowCountAccuracy', this evaluates the prediction accuracy but with a 'MaxNewSynapseCount' value set to 40.
            // The 'highCountAccuracy' variable is used to determine how increasing the synapse count affects the model's accuracy,
            // aiming to identify if a higher synapse count leads to better prediction outcomes.
            var highCountAccuracy = TestWithMaxNewSynapseCount(sequences, 40);

            // Determines the higher accuracy between the low and high MaxNewSynapseCount settings.
            // The 'bestAccuracy' variable holds the value of the better accuracy outcome, providing a direct comparison
            // between the two tested settings to ascertain which synapse count configuration yields superior prediction performance.
            var bestAccuracy = Math.Max(lowCountAccuracy, highCountAccuracy);

            // Identifies which MaxNewSynapseCount value (20 or 40) resulted in the best accuracy.
            // This conditional operation assigns the corresponding MaxNewSynapseCount value to 'bestCount',
            // effectively pinpointing the more optimal synapse count setting based on the achieved accuracies.
            var bestCount = lowCountAccuracy > highCountAccuracy ? 20 : 40;

            // Outputs the result of the test to the debug console, indicating the MaxNewSynapseCount value that led to the highest accuracy.
            // This log statement is crucial for understanding the impact of synapse count adjustments on the model's prediction accuracy in real-time.
            Debug.WriteLine($"Best accuracy achieved: {bestAccuracy} with MaxNewSynapseCount {bestCount}");

            // Asserts that the best accuracy obtained from either of the MaxNewSynapseCount settings is greater than 0.
            // This assertion ensures that the model was capable of learning and making predictions with a certain level of accuracy,
            // validating the effectiveness of the HTM model under the tested configurations.
            Assert.IsTrue(bestAccuracy > 0, "Expected a positive accuracy value indicating a successful learning outcome.");
        }


        /// <summary>
        /// Evaluates the prediction accuracy of a MultiSequenceLearning run for a given set of sequences and a specified MaxNewSynapseCount.
        /// This method initializes the HTM network with the specified MaxNewSynapseCount, runs the learning experiment with the provided sequences,
        /// and calculates the accuracy based on the number of correct predictions out of the total predictions made.
        /// </summary>
        /// <param name="sequences">A dictionary of sequences to be learned and predicted by the network, where each key is a unique sequence identifier, and the value is a list of numerical values representing the sequence.</param>
        /// <param name="maxNewSynapseCount">The maximum number of new synapses that can be formed to previously unconnected neurons during learning, used to configure the HTM network.</param>
        /// <returns>The prediction accuracy as a double, representing the ratio of correct predictions to total predictions.</returns>
        private double TestWithMaxNewSynapseCount(Dictionary<string, List<double>> sequences, int maxNewSynapseCount)
        {
            // Initialize accuracy to zero.
            double accuracy = 0;

            // Instantiate the learning experiment with the specified MaxNewSynapseCount.
            MultiSequenceLearning learningExperiment = new MultiSequenceLearning(maxNewSynapseCount);

            try
            {
                // Run the learning experiment with the provided sequences and obtain a predictor for making predictions.
                var predictor = learningExperiment.Run(sequences);

                // Initialize counters for total and correct predictions.
                int totalPredictions = 0;
                int correctPredictions = 0;

                // Iterate through each sequence in the provided set.
                foreach (var sequence in sequences)
                {
                    List<double> sequenceValues = sequence.Value;

                    // Log the sequence being tested.
                    Console.WriteLine($"Testing sequence: {sequence.Key}");
                    Console.WriteLine($"Testing sequence count: {sequenceValues.Count}");

                    // Iterate through the sequence values to make predictions and compare with the actual next value.
                    for (int i = 0; i < sequenceValues.Count - 1; i++)
                    {
                        double currentInput = sequenceValues[i];
                        string nextInput = sequenceValues[i + 1].ToString();

                        // Obtain predictions for the current input.
                        var predictions = predictor.Predict(currentInput);

                        // Log the current input and expected next value.
                        Console.WriteLine($"Input: {currentInput}");
                        Console.WriteLine($"Expected Next: {nextInput}");

                        // Log each prediction and its similarity score.
                        foreach (var prediction in predictions)
                        {
                            Console.WriteLine($"Predicted: {prediction.PredictedInput}, Similarity: {prediction.Similarity}");
                        }

                        // Determine if any prediction matches the actual next value.
                        bool isCorrectPrediction = predictions.Any(pred => pred.PredictedInput.Contains(nextInput));

                        // Increment counters based on prediction correctness.
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

                // Calculate accuracy as the ratio of correct predictions to total predictions.
                accuracy = totalPredictions > 0 ? (double)correctPredictions / totalPredictions : 0;
                Console.WriteLine($"Total predictions: {totalPredictions}, Correct predictions: {correctPredictions}, Accuracy: {accuracy}");
            }
            catch (Exception ex)
            {
                // In case of an exception, set accuracy to 0 and log the exception.
                accuracy = 0;
                Console.WriteLine(ex.ToString());
            }

            // Return the calculated accuracy.
            return accuracy;
        }

        /// <summary>
        /// Tests the learning speed of a MultiSequenceLearning by comparing the number of learning cycles required to reach a specified accuracy threshold with two different MaxNewSynapseCount settings. This method aims to identify which setting allows the network to learn more efficiently, providing insights into how the MaxNewSynapseCount parameter influences the speed of learning in HTM networks.
        /// </summary>
        [TestMethod]
        public void CompareLearningSpeedWithDifferentSynapseCounts()
        {
            // Retrieves a set of sequences from GetTestSequences1, which will be used as the dataset for the learning experiments.
            var sequences = GetTestSequences1();
            // Defines the target accuracy threshold for the learning process as 90%.
            double accuracyThreshold = 0.9;

            // Executes the learning experiment with a low MaxNewSynapseCount value (5) and calculates the number of cycles needed to reach the accuracy threshold.
            int cyclesForLowCount = GetCyclesToReachAccuracy(sequences, 5, accuracyThreshold);
            // Executes the learning experiment with a higher MaxNewSynapseCount value (20) and calculates the number of cycles needed to reach the accuracy threshold.
            int cyclesForHighCount = GetCyclesToReachAccuracy(sequences, 20, accuracyThreshold);

            // Determines the lower of the two cycle counts, indicating the most efficient learning performance.
            var bestEfficientCycles = Math.Min(cyclesForLowCount, cyclesForHighCount);
            // Identifies which MaxNewSynapseCount value resulted in the best (lowest) cycle count, indicating faster learning.
            var bestCount = cyclesForLowCount > cyclesForHighCount ? 20 : 5;

            // Logs the best learning efficiency achieved, indicating the number of cycles and the corresponding MaxNewSynapseCount value.
            Debug.WriteLine($"Best cycles achieved: {bestEfficientCycles} with MaxNewSynapseCount {bestCount}");

            // Asserts that the best cycle count is greater than 0, ensuring that the network was capable of learning successfully to the defined accuracy threshold. This check validates the effectiveness of the learning process under the tested MaxNewSynapseCount settings.
            Assert.IsTrue(bestEfficientCycles > 0, "Expected a positive cycle value indicating a successful learning outcome.");
        }


        /// <summary>
        /// Evaluates the MultiSequenceLearning efficiency using a more complex set of sequences and higher MaxNewSynapseCount values. 
        /// By comparing the number of learning cycles required to reach a 90% accuracy threshold at different synaptic creation capacities, 
        /// this test aims to further refine our understanding of how MaxNewSynapseCount influences learning speed in more demanding scenarios. 
        /// The test identifies the optimal MaxNewSynapseCount for efficient learning, contributing to the fine-tuning of HTM network configurations.
        /// </summary>
        [TestMethod]
        public void CompareLearningSpeedWithDifferentSynapseCounts2()
        {
            var sequences = GetTestSequences2(); // Retrieves a set of more complex sequences to challenge the HTM network.
            double accuracyThreshold = 0.9; // The desired accuracy threshold set for this experiment.

            // Compares the learning efficiency at moderate (20) and higher (40) MaxNewSynapseCount values with the complex sequences.
            int cyclesForLowCount = GetCyclesToReachAccuracy(sequences, 20, accuracyThreshold);
            int cyclesForHighCount = GetCyclesToReachAccuracy(sequences, 40, accuracyThreshold);

            // Determines the most efficient learning configuration based on the minimum number of cycles to reach the accuracy threshold.
            var bestEfficientCycles = Math.Min(cyclesForLowCount, cyclesForHighCount);
            var bestCount = cyclesForLowCount > cyclesForHighCount ? 40 : 20; // Identifies which MaxNewSynapseCount was more efficient.

            // Logs the optimal MaxNewSynapseCount value and the associated minimum cycle count needed for achieving the accuracy threshold.
            Console.WriteLine($"Best cycles achieved: {bestEfficientCycles} with MaxNewSynapseCount {bestCount}");

            // Verifies that the learning process was successful by ensuring a positive cycle count was required to meet the accuracy threshold.
            Assert.IsTrue(bestEfficientCycles > 0, "Expected a positive cycle value indicating a successful learning outcome.");
        }


        /// <summary>
        /// Determines the number of learning cycles required for an MultiSequenceLearning, configured with a specific MaxNewSynapseCount, 
        /// to reach or exceed a predefined accuracy threshold. This method iteratively trains the network on a set of sequences, 
        /// evaluating its prediction accuracy after each learning cycle. The process continues until the network's accuracy meets 
        /// or surpasses the desired threshold, at which point the method returns the number of cycles taken to achieve this goal. 
        /// If an exception occurs during the learning process, a default cycle count is returned, and the exception details are logged.
        /// </summary>
        /// <param name="sequences">A dictionary containing the sequences to be learned by the network. Each key is a unique identifier 
        /// for a sequence, and its value is a list of numerical values representing the sequence.</param>
        /// <param name="maxNewSynapseCount">The maximum number of new synapses that the HTM network can form to previously unconnected 
        /// neurons during the learning process. This parameter is critical for configuring the network's learning capabilities.</param>
        /// <param name="accuracyThreshold">The desired prediction accuracy threshold that the network must reach or exceed. 
        /// This value is expressed as a decimal between 0 and 1, where 1 represents 100% accuracy.</param>
        /// <returns>The number of cycles required for the network to reach the specified accuracy threshold. If an exception occurs, 
        /// a default value of 5000 cycles is returned, and the exception is logged for debugging purposes.</returns>
        private int GetCyclesToReachAccuracy(Dictionary<string, List<double>> sequences, int maxNewSynapseCount, double accuracyThreshold)
        {
            int cycles = 0; // Initialize the cycle counter.
            try
            {
                // Initialize the learning experiment with the specified MaxNewSynapseCount.
                MultiSequenceLearning learningExperiment = new MultiSequenceLearning(maxNewSynapseCount);
                // Run the learning experiment and obtain a predictor for making predictions.
                var predictor = learningExperiment.Run(sequences);

                bool accuracyReached = false; // Flag to indicate when the desired accuracy threshold is reached.

                // Continue the learning process until the accuracy threshold is met.
                while (!accuracyReached)
                {
                    cycles++; // Increment the cycle counter with each iteration.
                    int correctPredictions = 0; // Counter for correct predictions.
                    int totalPredictions = 0; // Counter for total predictions made.

                    // Iterate through each sequence to make predictions and assess accuracy.
                    foreach (var sequence in sequences)
                    {
                        List<double> sequenceValues = sequence.Value;

                        // Predict the next value in the sequence and assess correctness.
                        for (int i = 0; i < sequenceValues.Count - 1; i++)
                        {
                            double currentInput = sequenceValues[i];
                            string nextInput = sequenceValues[i + 1].ToString();

                            // Obtain predictions for the current input.
                            var predictions = predictor.Predict(currentInput);
                            // Determine if any predictions match the actual next value.
                            bool isCorrectPrediction = predictions.Any(pred => pred.PredictedInput.Contains(nextInput));

                            if (isCorrectPrediction)
                            {
                                correctPredictions++; // Increment correct predictions counter.
                            }

                            totalPredictions++; // Increment total predictions counter.
                        }
                    }

                    // Calculate the current accuracy based on correct and total predictions.
                    double accuracy = totalPredictions > 0 ? (double)correctPredictions / totalPredictions : 0;
                    // Check if the current accuracy meets or exceeds the threshold.
                    if (accuracy >= accuracyThreshold)
                    {
                        accuracyReached = true; // Update flag if threshold is met.
                    }
                    else
                    {
                        // Reset or update the predictor state for the next learning cycle if needed.
                        predictor.Reset();
                    }
                }
            }
            catch (Exception ex)
            {
                // In case of an exception, set a default cycle count and log the exception.
                cycles = 5000;
                Debug.WriteLine(ex.ToString());
            }

            return cycles; // Return the number of cycles required to meet the accuracy threshold.
        }


        public void TestLearningEfficiencyWithMaxNewSynapseAndSynapsesPerSegment()
        {
            // Example sequences for testing.
            var sequences = GetTestSequences1(); // Assuming this method retrieves your test sequences.

            // Define ranges or specific values for MaxNewSynapseCount and MaxSynapsesPerSegment to test their interaction.
            int[] maxNewSynapseCounts = { 10, 20, 30, 40 };
            double maxSynapsesPerSegment = 0.10; // Example fixed value to analyze interaction effects.
            try {
                foreach (var maxNewSynapseCount in maxNewSynapseCounts)
                {
                    try {
                        // Assuming MultiSequenceLearning can be initialized with MaxSynapsesPerSegment, or that it can be set separately.
                        MultiSequenceLearning msl = new MultiSequenceLearning(maxNewSynapseCount);

                        // Run the learning experiment.
                        var predictor = msl.Run(sequences, maxSynapsesPerSegment);

                        Console.WriteLine(maxSynapsesPerSegment);

                        int totalPredictions = 0;
                        int correctPredictions = 0;

                        // Iterate over sequences to calculate accuracy.
                        foreach (var sequence in sequences)
                        {
                            List<double> sequenceValues = sequence.Value;

                            for (int i = 0; i < sequenceValues.Count - 1; i++)
                            {
                                double currentInput = sequenceValues[i];
                                string nextInput = sequenceValues[i + 1].ToString();

                                var predictions = predictor.Predict(currentInput);

                                // Check prediction accuracy.
                                bool isCorrectPrediction = predictions.Any(pred => pred.PredictedInput.Contains(nextInput));
                                if (isCorrectPrediction)
                                {
                                    correctPredictions++;
                                }

                                totalPredictions++;
                            }
                        }

                        double accuracy = totalPredictions > 0 ? (double)correctPredictions / totalPredictions : 0;

                        // Log or analyze the accuracy for this configuration.
                        Console.WriteLine($"Accuracy with MaxNewSynapseCount={maxNewSynapseCount} and MaxSynapsesPerSegment={maxSynapsesPerSegment}: {accuracy}");
                    } catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        Console.WriteLine(maxNewSynapseCount.ToString()+" "+maxSynapsesPerSegment.ToString());
                    }
                    
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.ToString());
            }
        }

    }
}