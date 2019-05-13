using System.Collections;
using System.Collections.Generic;

public class Innovation   {

    public enum InnovationType
    {
        ADDNODE,
        ADDCONNECTION
    }

    InnovationType type;
    int inNode;
    int outNode;

    int innovation;

    public Innovation(int inNode, int outNode, InnovationType type)
    {
        this.inNode = inNode;
        this.outNode = outNode;
        this.type = type;
    }

    public int getInnovation()
    {
        return innovation;
    }

    public void setInnovation(int value)
    {
        innovation = value;
    }

    public bool compare(Innovation enemy)
    {
        return type == enemy.type && inNode == enemy.inNode && outNode == enemy.outNode;
    }

    public override string ToString()
    {
        return "" + type + " " + inNode + " " + outNode;
    }

}
