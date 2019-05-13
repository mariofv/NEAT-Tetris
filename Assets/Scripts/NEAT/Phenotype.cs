using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phenotype {

    List<int> nodeOrder;
    List<int> nodeIds;
    Dictionary<int, double> nodeValues;

    Dictionary<int, Dictionary<int, double>> adjacencies;

    bool hasCycle;

    public Phenotype(Genotype genotype)
    {
        Gene[] genotypeGenes = genotype.getGenes();
        nodeOrder = new List<int>();
        nodeIds = new List<int>();
        nodeValues = new Dictionary<int, double>();

        adjacencies = new Dictionary<int, Dictionary<int, double>>();


        foreach (Gene gene in genotypeGenes) {
            //INPUTS NODE KEYS
            if (gene.isNode())
            {
                nodeValues.Add(gene.getId(), -NEAT.Instance.parameters.INF);
                nodeIds.Add(gene.getId());
            }

            //INPUTS NODES CONNECTIONS
            else if (gene.isConnection() && gene.isEnabled())
            {
               
                int inNode = gene.getInNode();
                int outNode = gene.getOutNode();
                double weight = gene.getWeight();

                if (!adjacencies.ContainsKey(outNode))
                {
                    adjacencies.Add(outNode, new Dictionary<int,double>());
                }
                if (adjacencies[outNode].ContainsKey(inNode))
                {
                    throw new System.Exception("Phenotype has 2 connections between 2 nodes.");
                }
                else
                {
                    adjacencies[outNode].Add(inNode, weight);
                } 
            }
	    }
        topologicalSort();
    }


   public int feedForward(double[] netInput)
    {
        for (int i = 0; i < NEAT.Instance.parameters.NET_INPUTS; ++i)
        {
            nodeValues[i] = Utils.hyperbolicTangent(netInput[i]);
        }

        foreach (int currentNode in nodeOrder)
        {
            if (currentNode >= NEAT.Instance.parameters.NET_INPUTS)
            {
                bool connected = adjacencies.ContainsKey(currentNode) && adjacencies[currentNode].Keys.Count != 0;

                if (connected)
                {
                    double inputSum = 0;

                    foreach (KeyValuePair<int,double> adjacentNode in adjacencies[currentNode])
                    {
                        if (nodeValues[adjacentNode.Key] != -NEAT.Instance.parameters.INF)
                        {
                            inputSum += nodeValues[adjacentNode.Key] * adjacentNode.Value;

                        }
                    }

                    nodeValues[currentNode] = Utils.hyperbolicTangent(inputSum);
                }
            }
        }

        // SOFTMAX

        double[] netOutput = new double[NEAT.Instance.parameters.NET_OUTPUTS];
        double outputSum = 0;
        bool someOutputActive = false;

        for (int i = 0; i < NEAT.Instance.parameters.NET_OUTPUTS; ++i)
        {
            netOutput[i] = nodeValues[i + NEAT.Instance.parameters.NET_INPUTS];
            if (netOutput[i] != -NEAT.Instance.parameters.INF && netOutput[i] != 0)
            {
                someOutputActive = true;
                netOutput[i] = Mathf.Exp((float)netOutput[i]);
                outputSum += netOutput[i];
            }
        }


        if (someOutputActive)
        {
            int maxPos = -1;
            double max = -NEAT.Instance.parameters.INF;

            for (int i = 0; i < NEAT.Instance.parameters.NET_OUTPUTS; ++i)
            {
                if (netOutput[i] != -NEAT.Instance.parameters.INF && netOutput[i] != 0)
                {
                    netOutput[i] = netOutput[i] / outputSum;
                    if (netOutput[i] > max)
                    {
                        max = netOutput[i];
                        maxPos = i;
                    }
                }
            }
            return maxPos;
        }

        return -1;

    }

    private void topologicalSort()
    {
        Dictionary<int,string> state = new Dictionary<int,string>();
        foreach (int node in nodeIds)
        {
            state.Add(node, "alive");
        }

        foreach (int node in nodeIds)
        {
            visit(node, state);
            if (hasCycle)
            {
                return;
            }
        }
    }
  

    private void visit(int node, Dictionary<int, string> state)
    {
        if (state[node] == "dead")
        {
            return; // We've done this one already.
        }
        if (state[node] == "undead")
        {
            hasCycle = true;
            return; // We have a cycle; if you have special cycle handling code do it here.
                   
        }
        // It's alive. Mark it as undead.
        state[node] = "undead";
        bool dependsOnSomething = adjacencies.ContainsKey(node);
        if (dependsOnSomething)
        {
            foreach (int neighbour in adjacencies[node].Keys)
            {
                visit(neighbour, state);
            }
        }
        
        state[node] = "dead";
        nodeOrder.Add(node);
    }
  

    public bool hasCycles()
    {
        return hasCycle;
    }


}
