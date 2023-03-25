using MySEProject;
using NeoCortexApi;
using NeoCortexApi.Encoders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static MySEProject.MultiSequenceLearning;

namespace MySEProject
{
    class Program
    {
        /// <summary>
        /// SE Project: ML22/23-13	Investigate Influence of parameter MaxNewSynapseCount
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            
            // RunMultiSimpleSequenceLearningExperiment();

            Console.WriteLine("Running RunMultiSequenceLearningExperiment()");
            RunMultiSequenceLearningExperiment();
        }

        private static void RunMultiSimpleSequenceLearningExperiment()
        {
            Dictionary<string, List<double>> sequences = new Dictionary<string, List<double>>();

            sequences.Add("S1", new List<double>(new double[] { 0.0, 1.0, 2.0, 3.0, 4.0, 5.0, 7.0, 8.0, 11.0 }));
            sequences.Add("S2", new List<double>(new double[] { 0.0, 1.0, 2.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0 }));
            //sequences.Add("S3", new List<double>(new double[] { 1.0, 2.0, 3.0, 5.0, 7.0, 8.0, 9.0, 10.0, 11.0 }));

            //
            // Prototype for building the prediction engine.
            MultiSequenceLearning experiment = new MultiSequenceLearning();
            var predictor = experiment.Run(sequences);
        }


        /// <summary>
        /// This example demonstrates how to learn two sequences and how to use the prediction mechanism.
        /// First, two sequences are learned.
        /// Second, three short sequences with three elements each are created und used for prediction. The predictor used by experiment privides to the HTM every element of every predicting sequence.
        /// The predictor tries to predict the next element.
        /// </summary>
        private static void RunMultiSequenceLearningExperiment()
        {
            Dictionary<string, List<double>> sequences = new Dictionary<string, List<double>>();

            //sequences.Add("S1", new List<double>(new double[] { 0.0, 1.0, 0.0, 2.0, 3.0, 4.0, 5.0, 6.0, 5.0, 4.0, 3.0, 7.0, 1.0, 9.0, 12.0, 11.0, 12.0, 13.0, 14.0, 11.0, 12.0, 14.0, 5.0, 7.0, 6.0, 9.0, 3.0, 4.0, 3.0, 4.0, 3.0, 4.0 }));
            //sequences.Add("S2", new List<double>(new double[] { 0.8, 2.0, 0.0, 3.0, 3.0, 4.0, 5.0, 6.0, 5.0, 7.0, 2.0, 7.0, 1.0, 9.0, 11.0, 11.0, 10.0, 13.0, 14.0, 11.0, 7.0, 6.0, 5.0, 7.0, 6.0, 5.0, 3.0, 2.0, 3.0, 4.0, 3.0, 4.0 }));

            sequences.Add("S1", new List<double>(new double[] { 0.0, 1.0, 2.0, 3.0, 4.0, 5.0, 7.0, 8.0, 11.0 }));
            sequences.Add("S2", new List<double>(new double[] { 0.0, 1.0, 2.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0 }));
            sequences.Add("S3", new List<double>(new double[] { 1.0, 2.0, 3.0, 5.0, 7.0, 8.0, 9.0, 10.0, 11.0 }));

            //
            // Prototype for building the prediction engine.
            MultiSequenceLearning experiment = new MultiSequenceLearning();
            var predictor = experiment.Run(sequences);

            //
            // These list are used to see how the prediction works.
            // Predictor is traversing the list element by element. 
            // By providing more elements to the prediction, the predictor delivers more precise result.
            var list1 = new double[] { 1.0, 2.0, 3.0 };
            var list2 = new double[] { 2.0, 3.0, 5.0 };
            var list3 = new double[] { 6.0, 7.0, 9.0 };

            predictor.Reset();
            PredictNextElement(predictor, list1);

            predictor.Reset();
            PredictNextElement(predictor, list2);

            predictor.Reset();
            PredictNextElement(predictor, list3);
        }

        private static void PredictNextElement(Predictor predictor, double[] list)
        {
            Console.WriteLine("------------------------------");
            Console.WriteLine($"Input Sequence: {list}");

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
                    Console.WriteLine($"Predicted Sequence: {tokens[0]}, predicted next element {tokens2.Last()}");
                }
                else
                    Console.WriteLine("Nothing predicted :(");
            }

            Console.WriteLine("------------------------------");
        }
    }
}
