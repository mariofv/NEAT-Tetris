using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tetromino  {

    protected int currentRotation;
    protected int numberRotations;
    protected Block.BlockType[][][] shapes;

    public Tetromino()
    {
        currentRotation = 0;
        startAux();
    }



    public void rotate()
    {
        if (currentRotation + 1 == numberRotations)
        {
            currentRotation = 0;
        }
        else
        {
            ++currentRotation;
        }
    }

    public int getRotation()
    {
        return currentRotation;
    }

    public Block.BlockType[][] getShape()
    {
        return shapes[currentRotation];
    }

    public Block.BlockType[][] getNextShape()
    {
        int auxRotation = currentRotation;
        if (auxRotation + 1 == numberRotations)
        {
            auxRotation = 0;
        }
        else
        {
            ++auxRotation;
        }
        return shapes[auxRotation];
    }

    public int[][] getNextNormalizedShape()
    {
        int auxRotation = currentRotation;
        if (auxRotation + 1 == numberRotations)
        {
            auxRotation = 0;
        }
        else
        {
            ++auxRotation;
        }
        return getNormalizedShape(auxRotation);
    }

    public void rotateRandom()
    {
        currentRotation = Utils.generateRandomNumber(numberRotations);
    }

    public abstract Vector3 getSpawnPosition();
    public abstract Block.BlockType getBlockType();
    public abstract int[][] getNormalizedShape();
    public abstract int[][] getNormalizedShape(int rotation);
    protected abstract void startAux();
}
