using MyCloudProject.Common;
using NeoCortexApi;
using NeoCortexApi.Classifiers;
using NeoCortexApi.Encoders;
using NeoCortexApi.Entities;
using NeoCortexApi.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;


namespace SEProject
{
    /// <summary>
    /// Implements an experiment that demonstrates how to learn sequences.
    /// </summary>
    public class MultiSequenceLearning
    {
        private IStorageProvider storageProvider;
        private int maxNewSynapseCount;

        /// <summary>
        /// Initializes a new instance of the MultiSequenceLearning class with a specified MaxNewSynapseCount.
        /// This constructor allows dynamic adjustment of the MaxNewSynapseCount parameter, which dictates
        /// the maximum number of synapses that can be newly formed to unconnected cells during the learning process.
        /// Adjusting this parameter enables the exploration of its impact on the learning efficiency and
        /// prediction accuracy of Hierarchical Temporal Memory (HTM) networks.
        /// </summary>
        /// <param name="maxNewSynapseCount">The maximum number of new synapses that can be formed to previously unconnected cells.</param>
        public MultiSequenceLearning(int maxNewSynapseCount)
        {
            this.maxNewSynapseCount = maxNewSynapseCount;
        }

        /// <summary>
        /// Executes the sequence learning experiment using the configured HTM network. This method sets up
        /// the HTM network with the specified MaxNewSynapseCount and other necessary configurations. It then
        /// processes a series of input sequences, facilitating the network's learning. Upon completion, the method
        /// returns a Predictor object, which can be used to predict future sequence elements based on the learned patterns.
        /// This process is crucial for assessing the effects of MaxNewSynapseCount on the network's ability to
        /// learn and make accurate predictions.
        /// </summary>
        /// <param name="sequences">A dictionary containing the sequences to be learned. Each key represents a sequence identifier, and its corresponding value is a list of double values constituting the sequence.</param>
        /// <returns>A Predictor object configured for making predictions based on the learned sequences.</returns>
        public Predictor Run(Dictionary<string, List<double>> sequences, double defaultMaxSynapsesPerSegment = 0.40)
        {
            Console.WriteLine($"Hello NeocortexApi! Experiment {nameof(MultiSequenceLearning)}");

            int inputBits = 100;
            int numColumns = 1024;

            HtmConfig cfg = new HtmConfig(new int[] { inputBits }, new int[] { numColumns })
            {
                Random = new ThreadSafeRandom(42),

                CellsPerColumn = 25,
                GlobalInhibition = true,
                LocalAreaDensity = -1,
                NumActiveColumnsPerInhArea = 0.02 * numColumns,
                PotentialRadius = (int)(0.15 * inputBits),
                //InhibitionRadius = 15,

                MaxBoost = 10.0,
                DutyCyclePeriod = 25,
                MinPctOverlapDutyCycles = 0.75,
                MaxSynapsesPerSegment = (int)(defaultMaxSynapsesPerSegment * numColumns),
                MaxNewSynapseCount = this.maxNewSynapseCount,

                ActivationThreshold = 15,
                ConnectedPermanence = 0.5,

                // Learning is slower than forgetting in this case.
                PermanenceDecrement = 0.25,
                PermanenceIncrement = 0.15,

                // Used by punishing of segments.
                PredictedSegmentDecrement = 0.1
            };

            double max = 20;

            Dictionary<string, object> settings = new Dictionary<string, object>()
            {
                { "W", 15},
                { "N", inputBits},
                { "Radius", -1.0},
                { "MinVal", 0.0},
                { "Periodic", false},
                { "Name", "scalar"},
                { "ClipInput", false},
                { "MaxVal", max}
            };

            EncoderBase encoder = new ScalarEncoder(settings);

            return RunExperiment(inputBits, cfg, encoder, sequences);
        }

        /// <summary>
        /// Executes a comprehensive learning experiment with the specified Hierarchical Temporal Memory (HTM) configuration and sequences. 
        /// This method sets up an HTM network using a given configuration, processes a set of input sequences through the network, 
        /// and evaluates the network's learning and prediction capabilities. The experiment's progress and results are logged, 
        /// including the start and end times, configuration details, and the accuracy of predictions made after each learning cycle.
        /// </summary>
        /// <param name="inputBits">The number of bits in the input representation for the encoder.</param>
        /// <param name="cfg">Configuration settings for the HTM network, including parameters like MaxNewSynapseCount.</param>
        /// <param name="encoder">The encoder used to transform raw input into a binary representation suitable for the HTM network.</param>
        /// <param name="sequences">A collection of input sequences that the network will learn and predict. Each sequence is a list of double values.</param>
        /// <returns>A Predictor object that can be used to make predictions based on the learned model.</returns>
        private Predictor RunExperiment(int inputBits, HtmConfig cfg, EncoderBase encoder, Dictionary<string, List<double>> sequences)
        {
            Random rnd = new Random();
            int num = rnd.Next();
            string fileName = "experiment_results" + "_" + num.ToString() + "_" + this.maxNewSynapseCount.ToString() + ".txt";
            CortexLayer<object, object> layer1 = new CortexLayer<object, object>("L1");
            var mem = new Connections(cfg);
            HtmClassifier<string, ComputeCycle> cls = new HtmClassifier<string, ComputeCycle>();

            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new StreamWriter(memoryStream, leaveOpen: true))
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();

                    int maxMatchCnt = 0;
                    bool isInStableState = false;
                    var numUniqueInputs = GetNumberOfInputs(sequences);

                    TemporalMemory tm = new TemporalMemory();

                    writer.WriteLine($"Experiment Start: {DateTime.Now}");
                    writer.WriteLine($"MaxNewSynapseCount: {cfg.MaxNewSynapseCount}");
                    writer.Flush(); // Ensure data is written to the memory stream

                    Console.WriteLine($"Experiment Start: {DateTime.Now}");
                    Console.WriteLine($"MaxNewSynapseCount: {cfg.MaxNewSynapseCount}");

                    HomeostaticPlasticityController hpc = new HomeostaticPlasticityController(mem, numUniqueInputs * 150, (isStable, numPatterns, actColAvg, seenInputs) =>
                    {
                        if (isStable)
                        {
                            Console.WriteLine($"STABLE: Patterns: {numPatterns}, Inputs: {seenInputs}, iteration: {seenInputs / numPatterns}");
                        }
                        else
                        {
                            Console.WriteLine($"INSTABLE: Patterns: {numPatterns}, Inputs: {seenInputs}, iteration: {seenInputs / numPatterns}");
                        }
                        isInStableState = isStable;
                    }, numOfCyclesToWaitOnChange: 50);

                    SpatialPoolerMT sp = new SpatialPoolerMT(hpc);
                    sp.Init(mem);
                    tm.Init(mem);

                    layer1.HtmModules.Add("encoder", encoder);
                    layer1.HtmModules.Add("sp", sp);

                    int[] prevActiveCols = new int[0];
                    int cycle = 0;
                    int matches = 0;

                    var lastPredictedValues = new List<string>(new string[] { "0" });
                    int maxCycles = 2000;

                    for (int i = 0; i < maxCycles && isInStableState == false; i++)
                    {
                        matches = 0;
                        cycle++;
                        Console.WriteLine($"-------------- Newborn Cycle {cycle} ---------------");

                        foreach (var inputs in sequences)
                        {
                            foreach (var input in inputs.Value)
                            {
                                Debug.WriteLine($" -- {inputs.Key} - {input} --");
                                var lyrOut = layer1.Compute(input, true);

                                if (isInStableState)
                                    break;
                            }

                            if (isInStableState)
                                break;
                        }
                    }

                    cls.ClearState();
                    layer1.HtmModules.Add("tm", tm);

                    foreach (var sequenceKeyPair in sequences)
                    {
                        Console.WriteLine($"-------------- Sequences {sequenceKeyPair.Key} ---------------");

                        int maxPrevInputs = sequenceKeyPair.Value.Count - 1;
                        List<string> previousInputs = new List<string> { "-1.0" };
                        bool isLearningCompleted = false;

                        for (int i = 0; i < maxCycles; i++)
                        {
                            matches = 0;
                            cycle++;
                            Console.WriteLine($"-------------- Cycle {cycle} ---------------");

                            foreach (var input in sequenceKeyPair.Value)
                            {
                                Console.WriteLine($"-------------- {input} ---------------");

                                var lyrOut = layer1.Compute(input, true) as ComputeCycle;
                                var activeColumns = layer1.GetResult("sp") as int[];

                                previousInputs.Add(input.ToString());
                                if (previousInputs.Count > (maxPrevInputs + 1))
                                    previousInputs.RemoveAt(0);

                                if (previousInputs.Count < maxPrevInputs)
                                    continue;

                                string key = GetKey(previousInputs, input, sequenceKeyPair.Key);
                                List<Cell> actCells = lyrOut.ActiveCells.Count == lyrOut.WinnerCells.Count ? lyrOut.ActiveCells : lyrOut.WinnerCells;
                                cls.Learn(key, actCells.ToArray());

                                if (lastPredictedValues.Contains(key))
                                {
                                    matches++;
                                    Console.WriteLine($"Match. Actual value: {key} - Predicted value: {lastPredictedValues.FirstOrDefault(key)}.");
                                }
                                else
                                {
                                    Console.WriteLine($"Missmatch! Actual value: {key} - Predicted values: {string.Join(',', lastPredictedValues)}");
                                }

                                if (lyrOut.PredictiveCells.Count > 0)
                                {
                                    var predictedInputValues = cls.GetPredictedInputValues(lyrOut.PredictiveCells.ToArray(), 3);
                                    foreach (var item in predictedInputValues)
                                    {
                                        Console.WriteLine($"Current Input: {input} \t| Predicted Input: {item.PredictedInput} - {item.Similarity}");
                                    }
                                    lastPredictedValues = predictedInputValues.Select(v => v.PredictedInput).ToList();
                                }
                                else
                                {
                                    Console.WriteLine($"NO CELLS PREDICTED for next cycle.");
                                    lastPredictedValues = new List<string>();
                                }
                            }

                            double maxPossibleAccuracy = (double)((double)sequenceKeyPair.Value.Count - 1) / (double)sequenceKeyPair.Value.Count * 100.0;
                            double accuracy = (double)matches / (double)sequenceKeyPair.Value.Count * 100.0;

                            writer.WriteLine($"Sequence: {sequenceKeyPair.Key}, Cycle: {cycle}, Matches: {matches}, Accuracy: {accuracy}%");
                            writer.Flush(); // Ensure the latest data is written to the memory stream
                            Console.WriteLine($"Sequence: {sequenceKeyPair.Key}, Cycle: {cycle}, Matches: {matches}, Accuracy: {accuracy}%");

                            if (accuracy >= maxPossibleAccuracy)
                            {
                                maxMatchCnt++;
                                Console.WriteLine($"100% accuracy reached {maxMatchCnt} times.");
                                if (maxMatchCnt >= 30)
                                {
                                    sw.Stop();
                                    Console.WriteLine($"Sequence learned. The algorithm is in the stable state after 30 repeats with accuracy {accuracy} of maximum possible {maxMatchCnt}. Elapsed sequence {sequenceKeyPair.Key} learning time: {sw.Elapsed}.");
                                    isLearningCompleted = true;
                                    break;
                                }
                            }
                            else if (maxMatchCnt > 0)
                            {
                                Debug.WriteLine($"At 100% accuracy after {maxMatchCnt} repeats we get a drop of accuracy with accuracy {accuracy}. This indicates instable state. Learning will be continued.");
                                maxMatchCnt = 0;
                            }

                            tm.Reset(mem);
                        }

                        if (!isLearningCompleted)
                            throw new Exception($"The system didn't learn with expected accuracy!");
                    }
                    sw.Stop();
                    writer.WriteLine("Experiment End: " + DateTime.Now);
                    writer.WriteLine($"Total Experiment Duration: {sw.Elapsed}");
                    writer.Flush(); // Ensure all data is written to the memory stream
                }

                memoryStream.Position = 0; // Reset the position of the memory stream to the beginning
                                           // At this point, you can upload `memoryStream` to Azure Blob Storage

                try {
                    storageProvider.UploadResultFile($"Test_data_{DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss")}_MaxNewSynapseCountValue_{maxNewSynapseCount}.txt", memoryStream);
                } catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                

                return new Predictor(layer1, mem, cls);
            }
        }



        /// <summary>
        /// Gets the number of all unique inputs.
        /// </summary>
        /// <param name="sequences">Alle sequences.</param>
        /// <returns></returns>
        private int GetNumberOfInputs(Dictionary<string, List<double>> sequences)
        {
            int num = 0;

            foreach (var inputs in sequences)
            {
                //num += inputs.Value.Distinct().Count();
                num += inputs.Value.Count;
            }

            return num;
        }


        /// <summary>
        /// Constracts the unique key of the element of an sequece. This key is used as input for HtmClassifier.
        /// It makes sure that alle elements that belong to the same sequence are prefixed with the sequence.
        /// The prediction code can then extract the sequence prefix to the predicted element.
        /// </summary>
        /// <param name="prevInputs"></param>
        /// <param name="input"></param>
        /// <param name="sequence"></param>
        /// <returns></returns>
        private static string GetKey(List<string> prevInputs, double input, string sequence)
        {
            string key = String.Empty;

            for (int i = 0; i < prevInputs.Count; i++)
            {
                if (i > 0)
                    key += "-";

                key += (prevInputs[i]);
            }

            return $"{sequence}_{key}";
        }
    }
}
