using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class NEAT  {

    public NEATParams parameters;

    private static NEAT instance = null;
    private Epoch epoch;
    private List<Innovation> innovations;
    private Genotype[] currentPopulationGenomes;
    private int currentGenome;
    private int currentInnovation;

    private bool finishedTraining;
    private Phenotype selectedPhenotype;
    private double previousBestGenomeFitness;
    private int numGenerationsWithoutImprovement;

    public static NEAT Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new NEAT();
            }
            return instance;
        }
    }

    public void init()
    {
        parameters = NEATParams.loadFromJson();
        currentInnovation = 0;
        epoch = new Epoch();
        innovations = new List<Innovation>();
        File.Create(Path.Combine(Application.streamingAssetsPath, parameters.RESULTS_FILE_PATH)).Dispose();
        StreamWriter file = new StreamWriter(Path.Combine(Application.streamingAssetsPath, parameters.RESULTS_FILE_PATH), true);
        file.WriteLine("generation,genome,specie,fitness");
        file.Close();
        finishedTraining = false;
        selectedPhenotype = null;
        previousBestGenomeFitness = -parameters.INF;
        numGenerationsWithoutImprovement = 0;
        currentPopulationGenomes = epoch.spawnNextPopulation();
        currentGenome = 0;
    }

    public bool nextGenome(double fitness)
    {
        currentPopulationGenomes[currentGenome].setFitness(fitness);
        StreamWriter file = new StreamWriter(Path.Combine(Application.streamingAssetsPath, parameters.RESULTS_FILE_PATH), true);
        file.WriteLine("" + epoch.getGenerationNumber() + "," + currentGenome + "," + currentPopulationGenomes[currentGenome].getSpecie() + "," + currentPopulationGenomes[currentGenome].getFitness());
        file.Close();
        /*
        using (StreamWriter file = new StreamWriter(Path.Combine(Application.streamingAssetsPath, parameters.RESULTS_FILE_PATH), true))
        {
            file.WriteLine("" + epoch.getGenerationNumber() + "," + currentGenome + "," + currentPopulationGenomes[currentGenome].getSpecie() + "," + currentPopulationGenomes[currentGenome].getFitness());
        }
        */
        //Debug.Log("Generation: " + epoch.getGenerationNumber() + " GENOME: " + currentGenome + " SPECIE: " + currentPopulationGenomes[currentGenome].getSpecie() + " FITNESS: " + currentPopulationGenomes[currentGenome].getFitness());
        if (currentGenome + 1 == parameters.POPULATION_SIZE) 
        {
            if (epoch.getGenerationNumber() == parameters.MAX_GENERATIONS)
            {
                endTraining();
                return true;
            }
            else
            {
                nextEpoch();
                return true;
            }
        }
        else
        {
            ++currentGenome;
            return false;
        }

    }

    public int feedCurrentGenome(double[] netInput)
    {
        if (finishedTraining)
        {
            return selectedPhenotype.feedForward(netInput);
        }
        else
        {
            return currentPopulationGenomes[currentGenome].getPhenotype().feedForward(netInput);
        }
    }

    public void endTraining()
    {
        finishedTraining = true;
    }

    private double getBestGenomeFitness()
    {
        double maxFitness = -parameters.INF;
        foreach (Genotype g in currentPopulationGenomes)
        {
            if (g.getFitness() > maxFitness)
            {
                maxFitness = g.getFitness();
            }
        }
        return maxFitness;
    }

    public bool hasFinished()
    {
        return finishedTraining;
    }

    private void nextEpoch()
    {
        double bestGenomeFintess = getBestGenomeFitness(); 
        if (bestGenomeFintess > previousBestGenomeFitness * 1.1 )
        {
            previousBestGenomeFitness = bestGenomeFintess;
            numGenerationsWithoutImprovement = 0;
        }
        else
        {
            ++numGenerationsWithoutImprovement;
        }

        if(numGenerationsWithoutImprovement >= 20)
        {
            endTraining();
        }
        else
        {
            currentPopulationGenomes = epoch.spawnNextPopulation();

            currentGenome = 0;
        }
        
    }

    public int getCurrentInnovationNumber()
    {
        return currentInnovation;
    }

    public void increaseCurrentInnovationNumber()
    {
        ++currentInnovation;
    }

    public bool innovationExists(Innovation newInnovation)
    {
        foreach (Innovation i in innovations)
        {
            if (newInnovation.compare(i))
            {
                return true;
            }
        }
        return false;
    }

    public void addNewInnovation(Innovation newInnovation)
    {
        innovations.Add(newInnovation);
    }

    public Innovation getExistingInnovation(Innovation newInnovation)
    {
        foreach (Innovation i in innovations)
        {
            if (newInnovation.compare(i))
            {
                return i;
            }
        }
        throw new System.Exception("Can get innovation " + newInnovation.ToString());
    }

    public int getEpochNumber()
    {
        return epoch.getGenerationNumber();
    }

    public int getCurrentGenomeNumber()
    {
        return currentGenome;
    }
}
