using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentTetromino : MonoBehaviour {

    public static CurrentTetromino instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            blocks = GetComponent<BlockMatrix>();

        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    Block.BlockType blockType;
    Tetromino tetromino;
    protected BlockMatrix blocks;

    public void rotate()
    {
        tetromino.rotate();
        blocks.changeBlockTypes(tetromino.getShape());
    }

    public int getRotation()
    {
        return tetromino.getRotation();
    }

    public void setBlockType(Block.BlockType type)
    {
        blockType = type;
        switch (type)
        {
            case Block.BlockType.I:
                tetromino = new ITetromino();
                break;
            case Block.BlockType.J:
                tetromino = new JTetromino();
                break;
            case Block.BlockType.L:
                tetromino = new LTetromino();
                break;
            case Block.BlockType.O:
                tetromino = new OTetromino();
                break;
            case Block.BlockType.S:
                tetromino = new STetromino();
                break;
            case Block.BlockType.T:
                tetromino = new TTetromino();
                break;
            case Block.BlockType.Z:
                tetromino = new ZTetromino();
                break;
        }
        blocks.init(tetromino.getShape());
    }

    public Block.BlockType getBlockType()
    {
        return blockType;
    }

    public void setSpawnPosition()
    {
        this.transform.localPosition = tetromino.getSpawnPosition();
    }

    public Block.BlockType[][] getShape()
    {
        return tetromino.getShape();
    }

    public Block.BlockType[][] getNextShape()
    {
        return tetromino.getNextShape();
    }

    public int[][] getNormalizedShape()
    {
        return tetromino.getNormalizedShape();
    }

    public int[] getShapeHeight()
    {
        return blocks.getColumnsBlocks();
    }
}
