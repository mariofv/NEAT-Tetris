using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Epoch {
    Genotype[] currentPopulation;
    List<Specie> species;

    int currentGeneration;
    int currentSpecieId;

    public Epoch()
    {
        currentGeneration = 0;
        currentSpecieId = 0;
        species = new List<Specie>();
    }

    public Genotype[] getCurrentPopulation()
    {
        return currentPopulation;
    }

    public Genotype[] spawnNextPopulation()
    {
        if (currentGeneration == 0)
        {
            currentPopulation = spawnFirstPopulation();
        }
        else
        {
            currentPopulation = breedCurrentPopulation();
        }

        foreach (Genotype g in currentPopulation)
        {
            g.generatePhenotype();
        }
        ++currentGeneration;

        return currentPopulation;
        
    }

    private Genotype[] spawnFirstPopulation()
    {
        Genotype[] firstPopulation = new Genotype[NEAT.Instance.parameters.POPULATION_SIZE];
        Genotype originalGenotype = new Genotype();

        for (int i = 0; i < NEAT.Instance.parameters.POPULATION_SIZE; ++i)
        {
            firstPopulation[i] = originalGenotype.cloneGenotypeWithRandomWeights();
        }

        return firstPopulation;
    }

    private Genotype[] breedCurrentPopulation()
    {
        Genotype[] nextPopulation = new Genotype[NEAT.Instance.parameters.POPULATION_SIZE];

        selectSpeciesRepresentatives();
        speciateGenomes();
        setSpeciesAdjustedFitness();
        setSpeciesNumberOffspring();
        killWeakIndividuals();

        int spawnedGenomes = 0;
        foreach (Specie specie in species)
        {
            if (spawnedGenomes < NEAT.Instance.parameters.POPULATION_SIZE)
            {
                int pendingOffspring = (int)System.Math.Round(specie.getNumberOffspring());
                bool bestChoosen = false;
                while (pendingOffspring > 0)
                {
                    --pendingOffspring;
                    Genotype newGenome;
                    if (!bestChoosen)
                    {
                        newGenome = specie.getRepresentative();
                        bestChoosen = true;
                    }
                    else
                    {

                        if (specie.getGenomes().Count == 1)
                        {
                            newGenome = specie.getRepresentative().cloneGenotype();
                        }

                        else
                        {
                            Genotype father = specie.getRandomGenome();
                            if (Utils.generateRandomNumber() < NEAT.Instance.parameters.CROSSOVER_PROBABILITY)
                            {
                                Genotype mother = specie.getRandomGenome();
                                int numTrials = 0;
                                while (father == mother && numTrials < 10)
                                {
                                    mother = specie.getRandomGenome();
                                    ++numTrials;
                                }
                                newGenome = father.crossover(mother);
                            }
                            else
                            {
                                newGenome = father.cloneGenotype();
                            }
                        }

                        newGenome.mutateAddNeuron();
                        newGenome.sortGenes();
                        newGenome.mutateAddLink();
                        newGenome.sortGenes();
                        newGenome.mutateWeights();
                        newGenome.sortGenes();
                    }

                    newGenome.sortGenes();
                    newGenome.setSpecie(specie.getId());
                    nextPopulation[spawnedGenomes] = newGenome;
                    newGenome.generatePhenotype();
                    ++spawnedGenomes;
                    if (spawnedGenomes == NEAT.Instance.parameters.POPULATION_SIZE)
                    {
                        pendingOffspring = 0;
                    }
                }
            }
        }

        while (spawnedGenomes < NEAT.Instance.parameters.POPULATION_SIZE)
        {
            nextPopulation[spawnedGenomes] = tournamentSelection();
            ++spawnedGenomes;
        }

        return nextPopulation;
    }

    private void selectSpeciesRepresentatives()
    {
        int maxRepresentativeSpecie = -1;
        double maxRepresentativeFitness = -1;
        // Select best representative for each specie and kill species with no improvements
        for (int i = 0; i < species.Count; ++i)
        {
            double representativeFitness = species[i].selectRepresentative();
            if (representativeFitness > maxRepresentativeFitness)
            {
                maxRepresentativeSpecie = i;
                maxRepresentativeFitness = representativeFitness;
            }
        }
        for (int i = 0; i < species.Count; ++i)
        {
            species[i].setBestRepresentative(i == maxRepresentativeSpecie);
        }
        species.RemoveAll(item => !item.hasImproved() && !item.containsBestRepresentative());
    }

    private void speciateGenomes()
    {
        foreach (Genotype g in currentPopulation)
        {
            bool speciated = false;
            int i = 0;
            while (!speciated && i < species.Count)
            {
                Specie currentSpecie = species[i];
                double genomesDistance = g.computeDistance(currentSpecie.getRepresentative());
                
                //Debug.Log(genomesDistance);

                if (genomesDistance <= NEAT.Instance.parameters.SPECIES_THRESHOLD)
                {
                    speciated = true;
                    currentSpecie.addGenome(g);
                }
                ++i;
            }

            if (!speciated)
            {
                Specie newSpecie = new Specie(currentSpecieId);
                ++currentSpecieId;
                newSpecie.addGenome(g);
                newSpecie.setRepresentative(g);
                species.Add(newSpecie);
            }
        }
        //Debug.Log("numSpec: " + species.Count);
    }

    private void setSpeciesAdjustedFitness()
    {
        foreach (Specie s in species)
        {
            s.setAdjustedFitness();
        }
    }

    private void killWeakIndividuals()
    {
        foreach (Specie specie in species)
        {
            specie.killWeakGenomes();
        }
    }

    private void setSpeciesNumberOffspring()
    {
        double averageFitness = 0;
        int genomesCount = 0;

        foreach (Specie specie in species)
        {
            foreach (Genotype genome in specie.getGenomes())
            {
                averageFitness += genome.getAdjustedFitness();
                ++genomesCount;
            }
        }

        averageFitness = averageFitness / genomesCount;

        foreach (Specie specie in species)
        {
            specie.setNumberOffspring(averageFitness);
        }
        
    }

    private Genotype tournamentSelection()
    {
        int tournamentSize = currentPopulation.Length / 5;
        double maxFitness = -NEAT.Instance.parameters.INF;
        Genotype tournamentWinner = null;
        for (int i = 0; i < tournamentSize; ++i)
        {
            Genotype contender = currentPopulation[Utils.generateRandomNumber(currentPopulation.Length)];
            if (contender.getFitness() > maxFitness)
            {
                tournamentWinner = contender;
                maxFitness = contender.getFitness();
            }
        }
        if (tournamentWinner == null)
        {
            throw new System.Exception("Cannot find winner in tournament");
        }
        tournamentWinner = tournamentWinner.cloneGenotype();
        tournamentWinner.sortGenes();
        return tournamentWinner;
    }

    public int getGenerationNumber()
    {
        return currentGeneration;
    }
}
