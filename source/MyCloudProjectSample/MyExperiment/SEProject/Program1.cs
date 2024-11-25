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
        /// This example demonstrates how to learn two sequences and how to use the prediction mechanism.
        /// First, two sequences are learned.
        /// Second, three short sequences with three elements each are created und used for prediction. The predictor used by experiment privides to the HTM every element of every predicting sequence.
        /// The predictor tries to predict the next element.
        /// </summary>
        private static void RunMultiSequenceLearningExperiment(int MaxNewSynapseCount, Dictionary<string, List<double>> sequence, List<List<double>> testList)
        {
            Dictionary<string, List<double>> sequences = sequence ;

            // Prototype for building the prediction engine.
            MultiSequenceLearning experiment = new MultiSequenceLearning(MaxNewSynapseCount);
            var predictor = experiment.Run(sequences);

            //
            // These list are used to see how the prediction works.
            // Predictor is traversing the list element by element. 
            // By providing more elements to the prediction, the predictor delivers more precise result.
            Console.WriteLine("Starting multi-sequence learning experiment...");

            Parallel.ForEach(sequences, sequence =>
            {
                Log($"Processing {sequence.Key}...");

                // Each sequence gets its own Predictor
                MultiSequenceLearning experiment = new MultiSequenceLearning(MaxNewSynapseCount);
                var predictor = experiment.Run(new Dictionary<string, List<double>> { { sequence.Key, sequence.Value } });

                // Predict for test lists
                var testLists = GenerateTestLists(testList);
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
