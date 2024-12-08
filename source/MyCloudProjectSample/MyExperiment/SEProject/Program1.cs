using NeoCortexApi;
using NeoCortexApi.Encoders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SEProject
{
    class Program1
    {
        private static readonly object _logLock = new object();
        /// <summary>
        /// This sample shows a typical experiment code for SP and TM.
        /// You must start this code in debugger to follow the trace.
        /// and TM.
        /// </summary>
        /// <param name="args"></param>
        //static void Main12(string[] args)
        //{
        //    //
        //    // Starts experiment that demonstrates how to learn spatial patterns.
        //    //SpatialPatternLearning experiment = new SpatialPatternLearning();
        //    //experiment.Run();

        //    //
        //    // Starts experiment that demonstrates how to learn spatial patterns.
        //    //SequenceLearning experiment = new SequenceLearning();
        //    //experiment.Run();

        //    //GridCellSamples gridCells = new GridCellSamples();
        //    //gridCells.Run();

        //    // RunMultiSimpleSequenceLearningExperiment();

        //    RunMultiSequenceLearningExperiment(60);
        //}


        /// <summary>
        /// Demonstrates how to learn multiple sequences and use the prediction mechanism.
        /// Captures results like cycle count, accuracy, duration, and status for each sequence.
        /// </summary>
        /// <param name="MaxNewSynapseCount">The maximum number of new synapses per segment.</param>
        /// <param name="sequence">A dictionary containing sequences to learn.</param>
        /// <param name="testList">A list of test sequences used for prediction validation.</param>
        /// <param name="sequenceResults">Outputs results for each sequence, including cycle count, accuracy, duration, and status.</param>
        public void RunMultiSequenceLearningExperiment(
            int MaxNewSynapseCount,
            Dictionary<string, List<double>> sequence,
            List<List<double>> testList,
            out Dictionary<string, (string CycleID, int CycleCount, double Accuracy, TimeSpan Duration, string Status)> sequenceResults)
        {
            // Local dictionary to store results
            var localResults = new Dictionary<string, (string CycleID, int CycleCount, double Accuracy, TimeSpan Duration, string Status)>();
            Dictionary<string, List<double>> sequences = sequence;

            Console.WriteLine("Starting multi-sequence learning experiment...");

            // Use a thread-safe structure for updating results in parallel
            var resultsLock = new object();

            // Parallel execution for each sequence
            Parallel.ForEach(sequences, sequence =>
            {
                string sequenceKey = sequence.Key; // This will be used as CycleID
                Log($"Processing {sequenceKey}...");

                try
                {
                    Log($"Just inside the parrallel loop and the value is{MaxNewSynapseCount}");
                    // Each sequence gets its own Predictor
                    MultiSequenceLearning experiment = new MultiSequenceLearning(MaxNewSynapseCount);
                    var predictor = experiment.Run(
                        new Dictionary<string, List<double>> { { sequenceKey, sequence.Value } },
                        out int cycleCount,
                        out double accuracy,
                        out TimeSpan duration,
                        out string status
                    );

                    // Predict for test lists
                    var testLists = GenerateTestLists(testList);
                    foreach (var testList in testLists)
                    {
                        predictor.Reset();
                        PredictNextElement(predictor, testList);
                    }

                    // Log results for this sequence
                    lock (resultsLock)
                    {
                        localResults[sequenceKey] = (sequenceKey, cycleCount, accuracy, duration, status);
                    }

                    Log($"Finished processing {sequenceKey} with Cycle Count: {cycleCount}, Accuracy: {accuracy:F2}%, Duration: {duration}, Status: {status}.");
                }
                catch (Exception ex)
                {
                    // In case of failure, log the result as failed
                    lock (resultsLock)
                    {
                        localResults[sequenceKey] = (sequenceKey, 0, 0.0, TimeSpan.Zero, "Failed");
                    }

                    Log($"Error processing {sequenceKey}: {ex.Message}");
                }
            });

            Console.WriteLine("Experiment completed.");

            // Assign local results to the out parameter
            sequenceResults = localResults;
        }



        /// <summary>
        /// Processes and returns test lists for predictions based on the provided data. 
        /// </summary>
        /// <param name="testLists">A list of test sequences (List<List<double>>) to be used for predictions.</param>
        /// <returns>A list of test sequences converted to double arrays.</returns>
        private static List<double[]> GenerateTestLists(List<List<double>> testLists)
        {
            // Convert each List<double> from testLists into a double[] and return as a List<double[]>
            return testLists.Select(list => list.ToArray()).ToList();
        }



        /// <summary>
        /// Demonstrates how to use the Predictor object for making predictions based on a given list of input values. 
        /// For each input in the list, this method retrieves the predictions from the Predictor, logs the predicted 
        /// values along with their similarity scores, and extracts specific parts of the predicted input for further 
        /// analysis or display.
        /// </summary>
        /// <param name="predictor">The Predictor object used to make predictions based on the learned model.</param>
        /// <param name="list">An array of double values representing the inputs for which predictions are to be made.</param>
        private static void PredictNextElement(Predictor predictor, double[] list)
        {
            Debug.WriteLine("------------------------------");

            foreach (var item in list)
            {
                var res = predictor.Predict(item);

                if (res.Count > 0)
                {
                    foreach (var pred in res)
                    {
                        Debug.WriteLine($"{pred.PredictedInput} - {pred.Similarity}");
                    }

                    var tokens = res.First().PredictedInput.Split('_');
                    var tokens2 = res.First().PredictedInput.Split('-');
                    Debug.WriteLine($"Predicted Sequence: {tokens[0]}, predicted next element {tokens2.Last()}");
                }
                else
                    Debug.WriteLine("Nothing predicted :(");
            }

            Debug.WriteLine("------------------------------");
        }

        /// <summary>
        /// Thread-safe logging method.
        /// </summary>
        private static void Log(string message)
        {
            lock (_logLock)
            {
                Console.WriteLine(message);
            }
        }
    }
}
