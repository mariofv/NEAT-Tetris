using System.Collections;
using System.Collections.Generic;

public class Gene {

    public enum GeneType
    {
        NODE,
        CONNECTION
    }

    int innovation;

    bool enabled;
    int inNode;
    int outNode;
    double weight;
    GeneType type;

    public Gene(int innovation)
    {
        this.innovation = innovation;
        type = GeneType.NODE;

        enabled = true;
        inNode = -1;
        outNode = -1;
        weight = -1;
    }

    public Gene(int innovation, int inputNode, int outputNode, double weight, bool enabled)
    {
        this.innovation = innovation;
        type = GeneType.CONNECTION;
        this.enabled = enabled;
        inNode = inputNode;
        outNode = outputNode;
        this.weight = weight;
    }

    public Gene(int innovation, GeneType type, int inputNode, int outputNode, double weight, bool enabled)
    {
        this.innovation = innovation;
        this.type = type;
        this.enabled = enabled;
        inNode = inputNode;
        outNode = outputNode;
        this.weight = weight;
    }

    public int getInnovation()
    {
        return innovation;
    }

    public int getId()
    {
        return innovation;
    }

    public bool isNode()
    {
        return type == GeneType.NODE;
    }

    public bool isConnection()
    {
        return type == GeneType.CONNECTION;
    }

    public void setEnabled(bool value)
    {
        enabled = value;
    }

    public bool isEnabled()
    {
        return enabled;
    }

    public int getInNode()
    {
        return inNode;
    }

    public int getOutNode()
    {
        return outNode;
    }

    public double getWeight()
    {
        return weight;
    }

    public void setWeight(double value)
    {
        weight = value;
    }

    public Gene cloneGene()
    {
        Gene clonedGene = new Gene(innovation,type,inNode,outNode,weight,enabled);
        return clonedGene;
    }

    public override string ToString()
    {
        string str = "TYPE: " + type + " INNOVATION: " + innovation + " ENABLED: " + enabled + " INNODE: " + inNode + " OUTNODE: " + outNode;
        return str;
    }
}
