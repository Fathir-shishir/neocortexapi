using NeoCortexApi;
using NeoCortexApi.Encoders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static NeoCortexApiSample.MultiSequenceLearning;

namespace NeoCortexApiSample
{
    class Program
    {

        private static readonly object _logLock = new object();


        /// <summary>
        /// This sample shows a typical experiment code for SP and TM.
        /// You must start this code in debugger to follow the trace.
        /// and TM.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //
            // Starts experiment that demonstrates how to learn spatial patterns.
            //SpatialPatternLearning experiment = new SpatialPatternLearning();
            //experiment.Run();

            //
            // Starts experiment that demonstrates how to learn spatial patterns.
            //SequenceLearning experiment = new SequenceLearning();
            //experiment.Run();

            //GridCellSamples gridCells = new GridCellSamples();
            //gridCells.Run();

            // RunMultiSimpleSequenceLearningExperiment();

            RunMultiSequenceLearningExperiment(20);
        }

        /// <summary>
        /// This example demonstrates how to learn multiple sequences concurrently
        /// and how to use the prediction mechanism.
        /// </summary>
        private static void RunMultiSequenceLearningExperiment(int MaxNewSynapseCount)
        {
            Dictionary<string, List<double>> sequences = new Dictionary<string, List<double>>
            {
                { "S1", new List<double> { 0.0, 1.0, 0.0, 2.0, 3.0, 4.0, 5.0, 6.0, 5.0, 4.0, 3.0, 7.0, 1.0, 9.0, 12.0, 11.0, 12.0, 13.0, 14.0, 11.0, 12.0, 14.0, 5.0, 7.0, 6.0, 9.0, 3.0, 4.0, 3.0, 4.0, 3.0, 4.0 } },
                { "S2", new List<double> { 0.8, 2.0, 0.0, 3.0, 3.0, 4.0, 5.0, 6.0, 5.0, 7.0, 2.0, 7.0, 1.0, 9.0, 11.0, 11.0, 10.0, 13.0, 14.0, 11.0, 7.0, 6.0, 5.0, 7.0, 6.0, 5.0, 3.0, 2.0, 3.0, 4.0, 3.0, 4.0 } }
            };

            // Parallel processing for sequences
            Console.WriteLine("Starting multi-sequence learning experiment...");

            Parallel.ForEach(sequences, sequence =>
            {
                Log($"Processing {sequence.Key}...");

                // Each sequence gets its own Predictor
                MultiSequenceLearning experiment = new MultiSequenceLearning(MaxNewSynapseCount);
                var predictor = experiment.Run(new Dictionary<string, List<double>> { { sequence.Key, sequence.Value } });

                // Predict for test lists
                var testLists = GenerateTestLists();
                foreach (var testList in testLists)
                {
                    predictor.Reset();
                    PredictNextElement(predictor, testList);
                }

                Log($"Finished processing {sequence.Key}.");
            });

            Console.WriteLine("Experiment completed.");
        }

        /// <summary>
        /// Generates test lists for predictions.
        /// </summary>
        private static List<double[]> GenerateTestLists()
        {
            return new List<double[]>
            {
                new double[] { 1.0, 0.0, 2.0, 3.0, 4.0, 5.0, 6.0, 5.0, 4.0, 3.0, 7.0, 1.0, 9.0 },
                new double[] { 0.8, 2.0, 0.0, 3.0, 3.0, 4.0 },
                new double[] { 0.8, 2.0, 0.0 }
            };
        }

        /// <summary>
        /// Predicts the next element for a given list using the Predictor.
        /// </summary>
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
                {
                    Debug.WriteLine("Nothing predicted :(");
                }
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
