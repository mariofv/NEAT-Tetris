using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMatrix : MonoBehaviour {

    int blockMatrixWidth;
    int blockMatrixHeight;

    Block[][] blockMatrix;

    GameObject blockMatrixContainer;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void init(int blockMatrixHeight, int blockMatrixWidth)
    {
        createContainer();

        this.blockMatrixWidth = blockMatrixWidth;
        this.blockMatrixHeight = blockMatrixHeight;
        blockMatrix = new Block[blockMatrixHeight][];
        for (int i = 0; i < blockMatrixHeight; ++i)
        {
            blockMatrix[i] = new Block[blockMatrixWidth];
            for (int j = 0; j < blockMatrixWidth; ++j)
            {
                GameObject newBlock = Instantiate(Resources.Load("Block"), blockMatrixContainer.transform) as GameObject;
                newBlock.transform.localPosition = new Vector3(j, i, 0);
                blockMatrix[i][j] = newBlock.GetComponent<Block>();
                blockMatrix[i][j].changeBlockSprite(Block.BlockType.NONE);
            }
        }
    }

    public void init(Block.BlockType[][] newBlockTypeMatrix)
    {
        createContainer();

        this.blockMatrixWidth = newBlockTypeMatrix[0].Length;
        this.blockMatrixHeight = newBlockTypeMatrix.Length;
        blockMatrix = new Block[blockMatrixHeight][];
        for (int i = 0; i < blockMatrixHeight; ++i)
        {
            blockMatrix[i] = new Block[blockMatrixWidth];
            for (int j = 0; j < blockMatrixWidth; ++j)
            {
                GameObject newBlock = Instantiate(Resources.Load("Block"), blockMatrixContainer.transform) as GameObject;
                newBlock.transform.localPosition = new Vector3(j, i, 0);
                blockMatrix[i][j] = newBlock.GetComponent<Block>();
                blockMatrix[i][j].changeBlockSprite(newBlockTypeMatrix[i][j]);
            }
        }
    }


    private void createContainer()
    {
        Transform previousContainer = transform.Find("blockMatrixContainer");
        if (previousContainer != null)
        {
            Destroy(previousContainer.gameObject);
        }
        blockMatrixContainer = new GameObject();
        blockMatrixContainer.transform.parent = transform;
        blockMatrixContainer.transform.localPosition = Vector3.zero;
        blockMatrixContainer.name = "blockMatrixContainer";
    }

    public void changeBlockTypes(Block.BlockType[][] newBlockTypeMatrix)
    {
        if (newBlockTypeMatrix.Length != blockMatrixHeight || newBlockTypeMatrix[0].Length != blockMatrixWidth)
        {
            throw new Exception("Incorrect block size!");
        }
        for (int i = 0; i < blockMatrixHeight; ++i)
        {
            for (int j = 0; j < blockMatrixWidth; ++j)
            {
                blockMatrix[i][j].changeBlockSprite(newBlockTypeMatrix[i][j]);
            }
        }
    }

    public Block.BlockType getBlockType(int i, int j)
    {
        return blockMatrix[i][j].getBlockType();
    }

    public void setBlockType(int i, int j, Block.BlockType type)
    {
        blockMatrix[i][j].changeBlockSprite(type);
    }

    public int getHeight()
    {
        return blockMatrixHeight;
    }

    public int getWidth()
    {
        return blockMatrixWidth;
    }


    public int[] getCompletedRows()
    {
        List<int> completedRows = new List<int>();

        for (int i = 0; i < blockMatrixHeight; ++i)
        {
            bool completed = true;

            for (int j = 0; j < blockMatrixWidth; ++j)
            {
                if (blockMatrix[i][j].getBlockType() == Block.BlockType.NONE)
                {
                    completed = false;
                }
            }

            if (completed)
            {
                completedRows.Add(i);
            }
        }
        completedRows.Sort();
        completedRows.Reverse();
        return completedRows.ToArray();

    }

    public void eraseLine(int line)
    {
        for (int i = line; i < blockMatrixHeight; ++i)
        {
            for (int j = 0; j < blockMatrixWidth; ++j)
            {
                if (i == blockMatrixHeight - 1)
                {
                    blockMatrix[i][j].changeBlockSprite(Block.BlockType.NONE);
                }
                else
                {
                    blockMatrix[i][j].changeBlockSprite(blockMatrix[i + 1][j].getBlockType());
                }
            }
        }
    }

    public int[] getShape()
    {
        
        int[] shape = new int[blockMatrixWidth];
        for (int i = 0; i < blockMatrixWidth; ++i)
        {
            shape[i] = getColumnHeight(i);
        }

        return shape;
    }


    public int[] getColumnsBlocks()
    {
        int[] columnsBlocks = new int[blockMatrixWidth];
        for (int i = 0; i < blockMatrixWidth; ++i)
        {
            columnsBlocks[i] = getColumnBlocks(i);
        }

        return columnsBlocks;
    }

    private int getColumnHeight(int column)
    {
        for (int i = blockMatrixHeight - 1; i >= 0; --i)
        {
            if (blockMatrix[i][column].getBlockType() != Block.BlockType.NONE)
            {
                return i + 1;
            }
        }

        return 0;
    }

    private int getColumnBlocks(int column)
    {
        int numBlocks = 0;
        for (int i = blockMatrixHeight - 1; i >= 0; --i)
        {
            if (blockMatrix[i][column].getBlockType() != Block.BlockType.NONE)
            {
                ++numBlocks;
            }
        }

        return numBlocks;
    }

}
