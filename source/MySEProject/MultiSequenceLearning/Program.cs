using MultiSequenceLearning;
using MySEProject;
using NeoCortexApi;
using NeoCortexApi.Encoders;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using static MySEProject.MultiSequenceLearning;

namespace MySEProject
{
    class Program
    {
        /// <summary>
        /// SE Project: ML22/23-13	Investigate Influence of parameter MaxNewSynapseCount
        /// Issue: https://github.com/UniversityOfAppliedSciencesFrankfurt/se-cloud-2022-2023/issues/67
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("Running RunMultiSequenceLearningExperiment()");
            List<Report> reports = new List<Report>();
            List<Analysis> analyses = new List<Analysis>();
            RunMultiSimpleSequenceLearningExperiment(reports, analyses);
            Console.WriteLine($"Reports: {reports.Count}");
            Console.WriteLine($"Reports: {analyses.Count}");
            String ticks = $"{DateTime.Now.Ticks}";
            generateReport(reports, ticks);
            generateAnalysis(analyses, ticks);
            //RunMultiSequenceLearningExperiment(reports, analyses);
        }

        private static void RunMultiSimpleSequenceLearningExperiment(List<Report> reports, List<Analysis> analyses)
        {
            Dictionary<string, List<double>> sequences = new Dictionary<string, List<double>>();

            sequences.Add("S1", new List<double>(new double[] { 0.0, 1.0, 2.0, 3.0, 4.0, 5.0, 7.0, 8.0, 11.0 }));
            sequences.Add("S2", new List<double>(new double[] { 0.0, 1.0, 2.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0 }));
            sequences.Add("S3", new List<double>(new double[] { 1.0, 2.0, 3.0, 5.0, 7.0, 8.0, 9.0, 10.0, 11.0 }));

            //
            // Prototype for building the prediction engine.
            MultiSequenceLearning experiment = new MultiSequenceLearning();
            var predictor = experiment.Run(sequences, reports, analyses);
        }


        /// <summary>
        /// This example demonstrates how to learn two sequences and how to use the prediction mechanism.
        /// First, two sequences are learned.
        /// Second, three short sequences with three elements each are created und used for prediction. The predictor used by experiment privides to the HTM every element of every predicting sequence.
        /// The predictor tries to predict the next element.
        /// </summary>
        private static void RunMultiSequenceLearningExperiment(List<Report> reports, List<Analysis> analyses)
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
            var predictor = experiment.Run(sequences, reports, analyses);

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

        public static string generateReport(List<Report> reports, string ticks)
        {
            string BasePath = AppDomain.CurrentDomain.BaseDirectory;
            string reportFolder = Path.Combine(BasePath, "reports");
            if (!Directory.Exists(reportFolder))
                Directory.CreateDirectory(reportFolder);
            string reportPath = Path.Combine(reportFolder, $"report_{ticks}.txt");

            if (!File.Exists(reportPath))
            {
                using (StreamWriter sw = File.CreateText(reportPath))
                {
                    foreach(Report report in reports)
                    {
                        sw.WriteLine($"----------------------------- Start of Cycle: {report.cycle} -----------------------------");
                        sw.WriteLine($"Cycle: {report.cycle}, Sequence: {report.sequenceName}, Accuracy: {report.accuracy}");
                        foreach(String log in report.logs)
                        {
                            sw.WriteLine($"\t{log}");
                        }
                        sw.WriteLine($"----------------------------- End of Cycle: {report.cycle} -----------------------------");
                    }
                }
            }

             return reportPath;

        }

        public static string generateAnalysis(List<Analysis> analyses, string ticks)
        {
            string BasePath = AppDomain.CurrentDomain.BaseDirectory;
            string reportFolder = Path.Combine(BasePath, "analysis");
            if (!Directory.Exists(reportFolder))
                Directory.CreateDirectory(reportFolder);
            string reportPath = Path.Combine(reportFolder, $"analysis_{ticks}.csv");

            if (!File.Exists(reportPath))
            {
                using (StreamWriter sw = File.CreateText(reportPath))
                {
                    //sw.WriteLine("SequenceName,Cycle,Accuracy,ActivatePredictedColumnCalls,ActivatePredictedColumnNewSynapseCount,BurstColumnWithMatchingSegmentsCalls ,BurstColumnWithMatchingSegmentsNewSynapseCount,BurstColumnWithoutMatchingSegmentsCalls,BurstColumnWithoutMatchingSegmentsNewSynapseCount");
                    string previousSequenceName = "";
                    foreach (Analysis analysis in analyses)
                    {
                        if (!analysis.SequenceName.Equals(previousSequenceName))
                            sw.WriteLine("SequenceName,Cycle,Accuracy,ActivatePredictedColumnCalls,ActivatePredictedColumnNewSynapseCount,BurstColumnWithMatchingSegmentsCalls ,BurstColumnWithMatchingSegmentsNewSynapseCount,BurstColumnWithoutMatchingSegmentsCalls,BurstColumnWithoutMatchingSegmentsNewSynapseCount");
                        previousSequenceName = analysis.SequenceName;
                        sw.WriteLine($"{analysis.SequenceName},{analysis.Cycle},{analysis.Accuracy},{analysis.ActivatePredictedColumnCalls},{analysis.ActivatePredictedColumnNewSynapseCount},{analysis.BurstColumnWithMatchingSegmentsCalls},{analysis.BurstColumnWithMatchingSegmentsNewSynapseCount},{analysis.BurstColumnWithoutMatchingSegmentsCalls},{analysis.BurstColumnWithoutMatchingSegmentsNewSynapseCount}");
                    }
                }
            }

            return reportPath;
        }
    }
}
