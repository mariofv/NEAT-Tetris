using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTetromino : Tetromino
{
    public override Block.BlockType getBlockType()
    {
        return Block.BlockType.T;
    }

    protected override void startAux()
    {
        numberRotations = 4;

        shapes = new Block.BlockType[numberRotations][][];
        shapes[0] = new Block.BlockType[][]{
            new Block.BlockType[]{ Block.BlockType.NONE, Block.BlockType.T,    Block.BlockType.NONE },
            new Block.BlockType[]{ Block.BlockType.T,    Block.BlockType.T,    Block.BlockType.T },
            new Block.BlockType[]{ Block.BlockType.NONE, Block.BlockType.NONE, Block.BlockType.NONE }
        };
        shapes[1] = new Block.BlockType[][]{
            new Block.BlockType[]{ Block.BlockType.NONE, Block.BlockType.T, Block.BlockType.NONE },
            new Block.BlockType[]{ Block.BlockType.T, Block.BlockType.T, Block.BlockType.NONE },
            new Block.BlockType[]{ Block.BlockType.NONE, Block.BlockType.T, Block.BlockType.NONE }
        };
        shapes[2] = new Block.BlockType[][]{
            new Block.BlockType[]{ Block.BlockType.NONE, Block.BlockType.NONE, Block.BlockType.NONE },
            new Block.BlockType[]{ Block.BlockType.T,    Block.BlockType.T,    Block.BlockType.T },
            new Block.BlockType[]{ Block.BlockType.NONE, Block.BlockType.T,    Block.BlockType.NONE }
        };
        shapes[3] = new Block.BlockType[][]{
            new Block.BlockType[]{ Block.BlockType.NONE, Block.BlockType.T, Block.BlockType.NONE },
            new Block.BlockType[]{ Block.BlockType.NONE,    Block.BlockType.T, Block.BlockType.T },
            new Block.BlockType[]{ Block.BlockType.NONE, Block.BlockType.T, Block.BlockType.NONE }
        };
    }

    public override Vector3 getSpawnPosition()
    {
        return new Vector3(3, 18, 0);
    }

    public override int[][] getNormalizedShape()
    {
        int[][] normShape = new int[4][];
        for (int i = 0; i < 4; ++i)
        {
            normShape[i] = new int[4];
            for (int j = 0; j < 4; ++j)
            {
                if (i == 3 || j == 3)
                {
                    normShape[i][j] = 0;
                }
                else
                {
                    if (shapes[currentRotation][i][j] == Block.BlockType.NONE)
                    {
                        normShape[i][j] = 0;
                    }
                    else
                    {
                        normShape[i][j] = 1;
                    }
                }
            }
        }

        return normShape;
    }

    public override int[][] getNormalizedShape(int rotation)
    {
        int[][] normShape = new int[4][];
        for (int i = 0; i < 4; ++i)
        {
            normShape[i] = new int[4];
            for (int j = 0; j < 4; ++j)
            {
                if (i == 3 || j == 3)
                {
                    normShape[i][j] = 0;
                }
                else
                {
                    if (shapes[rotation][i][j] == Block.BlockType.NONE)
                    {
                        normShape[i][j] = 0;
                    }
                    else
                    {
                        normShape[i][j] = 1;
                    }
                }
            }
        }

        return normShape;
    }
}
