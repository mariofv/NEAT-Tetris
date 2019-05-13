using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class NEATParams  {
    public int INF;

    public int NET_INPUTS;
    public int NET_OUTPUTS;

    public int MAX_GENERATIONS;

    public int POPULATION_SIZE;

    public int CROSSOVER_PROBABILITY;
    public int MUTATION_ADDNODE_PROBABILITY;
    public int MUTATION_LINK_PROBABILITY;
    public int MUTATION_WEIGHT_PROBABILITY;
    public int MUTATION_WEIGHT_REPLACAMENT_PROBABILITY;
    public double SPECIES_THRESHOLD;

    public double C1;
    public double C2;
    public double C3;

    public string RESULTS_FILE_PATH = "results.csv";

    public static NEATParams loadFromJson()
    {
        string paramsPath = Path.Combine(Application.streamingAssetsPath, "params.json");
        string dataAsJson = File.ReadAllText(paramsPath);
        NEATParams loadedData = JsonUtility.FromJson<NEATParams>(dataAsJson);

        return loadedData;
    }

    public void toJson()
    {
        string dataAsJson = JsonUtility.ToJson(this);
        File.Create("params.json").Dispose();
        File.WriteAllText("params.json", dataAsJson);
    }

    public NEATParams()
    {
        INF = 10000;

        NET_INPUTS = 16;
        NET_OUTPUTS = 5;

        MAX_GENERATIONS = 10000;

        POPULATION_SIZE = 100;

        CROSSOVER_PROBABILITY = 70;
        MUTATION_ADDNODE_PROBABILITY = 20;
        MUTATION_LINK_PROBABILITY = 20;
        MUTATION_WEIGHT_PROBABILITY = 20;
        MUTATION_WEIGHT_REPLACAMENT_PROBABILITY = 10;
        SPECIES_THRESHOLD = 3;

        C1 = 0.5;
        C2 = 0.5;
        C3 = 1;

    }

}
