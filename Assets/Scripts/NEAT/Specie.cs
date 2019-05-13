using System.Collections;
using System.Collections.Generic;
using System;

public class Specie  {
    int id;
    Genotype representative;
    bool hasBestRepresentative;

    int numberGenerationsWithoutImprovement;

    List<Genotype> specieGenomes;


    double numberOffspring;

    public Specie(int id)
    {
        this.id = id;
        numberGenerationsWithoutImprovement = 0;
        specieGenomes = new List<Genotype>();
    }

    public void setAdjustedFitness()
    {
        foreach (Genotype g in specieGenomes)
        {
            g.setAdjustedFitness(g.getFitness() / specieGenomes.Count);
        }
    }

    public double getNumberOffspring()
    {
        return numberOffspring;
    }

    public void setNumberOffspring(double averageFitness)
    {
        numberOffspring = 0;
        foreach(Genotype g in specieGenomes)
        {
            numberOffspring += g.getAdjustedFitness();
        }

        numberOffspring = numberOffspring / averageFitness;
    }

    public void addGenome(Genotype genome)
    {
        specieGenomes.Add(genome);
    }

    public Genotype getRandomGenome()
    {
        return specieGenomes[Utils.generateRandomNumber(specieGenomes.Count)];
    }

    public bool hasImproved()
    {
        return numberGenerationsWithoutImprovement < 15;
    }

    public double selectRepresentative()
    {
        specieGenomes.Sort((a, b) => (int)(b.getFitness() - a.getFitness()));

        Genotype  newRepresentative = specieGenomes[0];
        if (newRepresentative.getFitness()  < 1.1* representative.getFitness())
        {
            ++numberGenerationsWithoutImprovement;
        }
        else
        {
            numberGenerationsWithoutImprovement = 0;
        }
        representative = newRepresentative;
        specieGenomes = new List<Genotype>();
        specieGenomes.Add(representative);

        return representative.getFitness();
    }

    public void killWeakGenomes()
    {
        specieGenomes.Sort((a, b) => (int)(b.getFitness() - a.getFitness()));
        int survivors = Math.Max(1,(int)Math.Round(specieGenomes.Count * 0.5));

        List<Genotype> newGenomes = new List<Genotype>();
        newGenomes.Add(specieGenomes[0]);
        representative = specieGenomes[0];
        for (int i = 1; i < survivors; ++i)
        {
            newGenomes.Add(specieGenomes[i]);
        }
        specieGenomes = newGenomes;
    }

    public bool containsBestRepresentative()
    {
        return hasBestRepresentative;
    }

    public void setBestRepresentative(bool value)
    {
        hasBestRepresentative = value;
    }

    public void setRepresentative(Genotype genome)
    {
        representative = genome;
    }

    public Genotype getRepresentative()
    {
        return representative;
    }

    public List<Genotype> getGenomes()
    {
        return specieGenomes;
    }

    public int getId()
    {
        return id;
    }
}
