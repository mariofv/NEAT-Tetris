using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genotype {

    List<Gene> genes;
    Phenotype phenotype;
    double fitness;
    double adjustedFitness;
    int specie;

    public Genotype()
    {
        specie = -1;
        genes = new List<Gene>();

        for (int i = 0; i < NEAT.Instance.parameters.NET_INPUTS; ++i)
        {
            Gene newInputNode = new Gene(NEAT.Instance.getCurrentInnovationNumber());
            NEAT.Instance.increaseCurrentInnovationNumber();
            genes.Add(newInputNode);
        }

        for (int i = 0; i < NEAT.Instance.parameters.NET_OUTPUTS; ++i)
        {
            Gene newOutputNode = new Gene(NEAT.Instance.getCurrentInnovationNumber());
            NEAT.Instance.increaseCurrentInnovationNumber();
            genes.Add(newOutputNode);
        }

        for (int i = 0; i < NEAT.Instance.parameters.NET_INPUTS; ++i)
        {
            for (int j = 0; j < NEAT.Instance.parameters.NET_OUTPUTS; ++j)
            {
                int r = Utils.generateRandomNumber();
                if (r <= 5)
                {
                    int inputNode = i;
                    int outputNode = NEAT.Instance.parameters.NET_INPUTS + j;
                    int innovation = NEAT.Instance.getCurrentInnovationNumber();
                    NEAT.Instance.increaseCurrentInnovationNumber();
                    Gene newConnection = new Gene(innovation,  inputNode, outputNode, Utils.randomWeight(), true);
                    Innovation newInnovation = new Innovation(inputNode, outputNode, Innovation.InnovationType.ADDCONNECTION);
                    newInnovation.setInnovation(innovation);
                    NEAT.Instance.addNewInnovation(newInnovation);
                    genes.Add(newConnection);
                }
            }
        }
    }

    public Genotype(List<Gene> genes)
    {
        specie = -1;
        this.genes = genes;
    }

    public void setSpecie(int value)
    {
        specie = value;
    }

    public int getSpecie()
    {
        return specie;
    }

    public double computeDistance(Genotype mate)
    {
        List<Gene> maleGenes= genes;
        List<Gene> femaleGenes = mate.genes;

        int N = System.Math.Max(maleGenes.Count, femaleGenes.Count);
        if (N < 20)
        {
            N = 1;
        }

        int E = 0;
        int D = 0;
        double W = 0;

        int numMatchingGenes = 0;

        int i = 0;
        int j = 0;

        while (i < maleGenes.Count && j < femaleGenes.Count)
        {
            int innovationMaleGene = maleGenes[i].getInnovation();
            int innovationFemaleGene = femaleGenes[j].getInnovation();

            if (innovationMaleGene < innovationFemaleGene)
            {
                ++D;
                ++i;
            }
            else if (innovationMaleGene > innovationFemaleGene)
            {
                ++D;
                ++j;
            }
            else
            {
                W += System.Math.Abs(maleGenes[i].getWeight() - femaleGenes[j].getWeight());
                ++numMatchingGenes;
                ++i;
                ++j;
            }
        }

        while (i < maleGenes.Count)
        {
            ++E;
            ++i;
        }
        while (j < femaleGenes.Count)
        {
            ++E;
            ++j;
        }

        if (numMatchingGenes != 0)
        {
            W = W / numMatchingGenes;
        }
        else
        {
            W = NEAT.Instance.parameters.INF;
        }


        return (NEAT.Instance.parameters.C1 * E + NEAT.Instance.parameters.C2 * D) / N + NEAT.Instance.parameters.C3 * W;
    }

    public Genotype crossover(Genotype mate)
    {
        List<Gene> newGenes = new List<Gene>();
        List<int> addedNode = new List<int>();

        double maleGenomeFitness = fitness;
        double femaleGenomeFitness = mate.fitness;

        List<Gene> maleGenes;
        List<Gene> femaleGenes;
        
        if (maleGenomeFitness > femaleGenomeFitness)
        {
            maleGenes = genes;
            femaleGenes = mate.genes;
        }
        else if (maleGenomeFitness < femaleGenomeFitness)
        {
            maleGenes = mate.genes;
            femaleGenes = genes;
        }
        else
        {
            if (genes.Count > mate.genes.Count)
            {
                maleGenes = mate.genes;
                femaleGenes = genes;
            }
            else {
                maleGenes = genes;
                femaleGenes = mate.genes;
            }
        }

        int i = 0;
        int j = 0;

        while (i < maleGenes.Count && j < femaleGenes.Count)
        {
            Gene maleCurrentGene = maleGenes[i];
            Gene femaleCurrentGene = femaleGenes[j];
            Gene selectedGene = null;
            int innovationMaleGene = maleCurrentGene.getInnovation();
            int innovationFemaleGene = femaleCurrentGene.getInnovation();

            if (innovationMaleGene < innovationFemaleGene)
            {
                selectedGene = maleCurrentGene;
                ++i;
            }

            else if (innovationMaleGene > innovationFemaleGene)
            {
                ++j;
            }

            else
            {
              
                int r = Utils.generateRandomNumber(2);
                if (r == 0)
                {
                    selectedGene = maleCurrentGene;
                }
                else
                {
                    selectedGene = femaleCurrentGene;
                }
                ++i;
                ++j;
            }

            if (selectedGene != null)
            {
                Gene newGene = selectedGene.cloneGene();
                newGenes.Add(newGene);
                if (newGene.isNode())
                {
                    if (addedNode.Contains(newGene.getInnovation()))
                    {
                        throw new System.Exception("TRYING TO ADD DUPLICATED NEURON!");
                    }
                    addedNode.Add(newGene.getInnovation());
                }
                if (newGene.isConnection())
                {
                    int inNode = newGene.getInNode();
                    if (!addedNode.Contains(inNode))
                    {
                        addedNode.Add(inNode);
                        newGenes.Add(new Gene(inNode));
                    }
                    int outNode = newGene.getOutNode();
                    if (!addedNode.Contains(outNode))
                    {
                        addedNode.Add(outNode);
                        newGenes.Add(new Gene(outNode));
                    }
                }
            }
        }

        while (i < maleGenes.Count)
        {
            Gene newGene = maleGenes[i].cloneGene();
            newGenes.Add(newGene);
            if (newGene.isNode())
            {
                if (addedNode.Contains(newGene.getInnovation()))
                {
                    throw new System.Exception("TRYING TO ADD DUPLICATED NEURON!");
                }
                addedNode.Add(newGene.getInnovation());
            }
            if (newGene.isConnection())
            {
                int inNode = newGene.getInNode();
                if (!addedNode.Contains(inNode))
                {
                    addedNode.Add(inNode);
                    newGenes.Add(new Gene(inNode));
                }
                int outNode = newGene.getOutNode();
                if (!addedNode.Contains(outNode))
                {
                    addedNode.Add(outNode);
                    newGenes.Add(new Gene(outNode));
                }
            }
            ++i;
        }
        Genotype newGenome = new Genotype(newGenes);
        newGenome.sortGenes();
        
        return newGenome;
    }

    public void mutateAddNeuron()
    {
        if (Utils.generateRandomNumber() > NEAT.Instance.parameters.MUTATION_ADDNODE_PROBABILITY)
        {
            return;
        }

        List<Gene> geneConnections = new List<Gene>();
        
        foreach (Gene g in genes)
        {
            if (g.isConnection() && g.isEnabled())
            {
                geneConnections.Add(g);
            }
        }

        if (geneConnections.Count == 0 )
        {
            return;
        }

        int r = Utils.generateRandomNumber(geneConnections.Count);
        Gene selectedConnection = geneConnections[r];

        int inNode = selectedConnection.getInNode();
        int outNode = selectedConnection.getOutNode();

        Innovation newInnovation = new Innovation(inNode, outNode, Innovation.InnovationType.ADDNODE);
        if (NEAT.Instance.innovationExists(newInnovation))
        {
            Innovation nodeInnovation = NEAT.Instance.getExistingInnovation(newInnovation);
            foreach (Gene g in genes)
            {
                if (g.getInnovation() == nodeInnovation.getInnovation())
                {
                    return;
                }
            }
            
            selectedConnection.setEnabled(false);
            Gene newNode = new Gene(nodeInnovation.getInnovation());

            Innovation connectionInInnovation = NEAT.Instance.getExistingInnovation(new Innovation(inNode, newNode.getId(), Innovation.InnovationType.ADDCONNECTION));
            Gene newInputConnection = new Gene(connectionInInnovation.getInnovation(), inNode, newNode.getId(), 1, true);

            Innovation connectionOutInnovation = NEAT.Instance.getExistingInnovation(new Innovation(newNode.getId(), outNode, Innovation.InnovationType.ADDCONNECTION));
            Gene newOutputConnection = new Gene(connectionOutInnovation.getInnovation(), newNode.getId(), outNode, selectedConnection.getWeight(), true);

            genes.Add(newNode);
            genes.Add(newInputConnection);
            genes.Add(newOutputConnection);
        }
        else
        {
            int innovation = NEAT.Instance.getCurrentInnovationNumber();
            NEAT.Instance.increaseCurrentInnovationNumber();

            selectedConnection.setEnabled(false);
            Gene newNode = new Gene(innovation);
            newInnovation.setInnovation(innovation);
            NEAT.Instance.addNewInnovation(newInnovation);
            genes.Add(newNode);

            innovation = NEAT.Instance.getCurrentInnovationNumber();
            NEAT.Instance.increaseCurrentInnovationNumber();

            Gene newInputConnection = new Gene(innovation, inNode, newNode.getId(), 1, true);
            Innovation newConnectionInputInnovation = new Innovation(inNode, newNode.getId(), Innovation.InnovationType.ADDCONNECTION);
            newConnectionInputInnovation.setInnovation(innovation);
            NEAT.Instance.addNewInnovation(newConnectionInputInnovation);
            genes.Add(newInputConnection);

            innovation = NEAT.Instance.getCurrentInnovationNumber();
            NEAT.Instance.increaseCurrentInnovationNumber();
            
            Gene newOutputConnection = new Gene(innovation, newNode.getId(), outNode, selectedConnection.getWeight(), true);
            Innovation newConnectionOutputInnovation = new Innovation(newNode.getId(), outNode, Innovation.InnovationType.ADDCONNECTION);
            newConnectionOutputInnovation.setInnovation(innovation);
            NEAT.Instance.addNewInnovation(newConnectionOutputInnovation);
            genes.Add(newOutputConnection);
        }
    }

    public void generatePhenotype()
    {
        phenotype = new Phenotype(this);
    }

    public void mutateAddLink()
    {
        if (Utils.generateRandomNumber() > NEAT.Instance.parameters.MUTATION_LINK_PROBABILITY)
        {
            return;
        }

        List<Gene> nodes = new List<Gene>();
        foreach (Gene g in genes)
        {
            if (g.isNode())
            {
                nodes.Add(g);
            }
        }

        int numberTimesTryed = 0;
        while (numberTimesTryed < 100)
        {
            numberTimesTryed++;
            int inNode = Utils.generateRandomNumber(nodes.Count);
            int outNode = Utils.generateRandomNumber(NEAT.Instance.parameters.NET_INPUTS, nodes.Count);

            bool connectionExists = false;
            foreach (Gene g in genes)
            {
                if (g.isConnection() && g.getInNode() == inNode && g.getOutNode() == outNode)
                {
                    if (g.isEnabled())
                    {
                        connectionExists = true;
                    }
                    else
                    {
                        g.setEnabled(true);
                        return;
                    }

                }
            }

            bool areBothInput = inNode < NEAT.Instance.parameters.NET_INPUTS && outNode < NEAT.Instance.parameters.NET_INPUTS;

            if (!connectionExists && areBothInput)
            {
                Innovation newInnovation = new Innovation(inNode, outNode, Innovation.InnovationType.ADDCONNECTION);
                if (NEAT.Instance.innovationExists(newInnovation))
                {
                    Innovation existingInnovation = NEAT.Instance.getExistingInnovation(newInnovation);
                    Gene newConnection = new Gene(existingInnovation.getInnovation(),  inNode, outNode, Utils.randomWeight(), true);
                    genes.Add(newConnection);
                    return;
                }
                else
                {
                    Genotype clonedGenotype = cloneGenotype();
                    int innovation = NEAT.Instance.getCurrentInnovationNumber();
                    Gene newConnection = new Gene(innovation,  inNode, outNode, Utils.randomWeight(), true);
                    clonedGenotype.genes.Add(newConnection);
                    clonedGenotype.generatePhenotype();
                    if (!clonedGenotype.phenotype.hasCycles())
                    {
                        NEAT.Instance.increaseCurrentInnovationNumber();
                        genes.Add(newConnection);
                        newInnovation.setInnovation(innovation);
                        NEAT.Instance.addNewInnovation(newInnovation);
                        return;
                    }
                }
            }
        }
    }

    public void mutateWeights()
    {
        if (Utils.generateRandomNumber() > NEAT.Instance.parameters.MUTATION_WEIGHT_PROBABILITY)
        {
            return;
        }

        foreach (Gene g in genes)
        {
            if (g.isConnection())
            {
                double newWeight;
                if (Utils.generateRandomNumber() >= NEAT.Instance.parameters.MUTATION_WEIGHT_REPLACAMENT_PROBABILITY)
                {
                    double oldWeight = g.getWeight();
                    newWeight = oldWeight * (Utils.generateRandomNumber(20) + 100) / 100;
                }
                else
                {
                    newWeight = Utils.randomWeight();
                }
                g.setWeight(newWeight);
            }
        }
    }

    public void sortGenes()
    {
        genes.Sort((a, b) => (a.getInnovation() - b.getInnovation()) );
        
    }

    public void setFitness(double fitness)
    {
        this.fitness = fitness;
    }

    public double getFitness()
    {
        return fitness;
    }


    public double getAdjustedFitness()
    {
        return adjustedFitness;
    }

    public void setAdjustedFitness(double value)
    {
        adjustedFitness = value;
    }

    public Gene[] getGenes()
    {
        return genes.ToArray();
    }

    public Phenotype getPhenotype()
    {
        return phenotype;
    }

    public Genotype cloneGenotype()
    {
        Genotype clonedGenotype = new Genotype();
        clonedGenotype.fitness = fitness;
        clonedGenotype.adjustedFitness = adjustedFitness;
        clonedGenotype.genes = new List<Gene>(genes.Count);

        for (int i = 0; i < genes.Count; ++i)
        {
            clonedGenotype.genes.Insert(i,  genes[i].cloneGene());
        }

        return clonedGenotype;
    }

    public Genotype cloneGenotypeWithRandomWeights()
    {
        Genotype clonedGenotype = new Genotype();
        clonedGenotype.fitness = fitness;
        clonedGenotype.adjustedFitness = adjustedFitness;
        clonedGenotype.genes = new List<Gene>(genes.Count);

        for (int i = 0; i < genes.Count; ++i)
        {
            clonedGenotype.genes.Insert(i, genes[i].cloneGene());
        }

        for (int i = 0; i < genes.Count; ++i)
        {
            clonedGenotype.genes[i].setWeight(Utils.randomWeight());
        }

        return clonedGenotype;
    }

    public void checkGenes()
    {
        int previousGeneInnovation = -1;
        string genesString = "";
        foreach (Gene g in genes)
        {
            int currentInnovation = g.getInnovation();
            genesString += currentInnovation + " ";
            if (currentInnovation <= previousGeneInnovation)
            {
                throw new System.Exception("Colliding innovations are " + previousGeneInnovation + " " + currentInnovation + "\n current gene is " + g.ToString());
            }
            previousGeneInnovation = currentInnovation;
        }
    }
}
