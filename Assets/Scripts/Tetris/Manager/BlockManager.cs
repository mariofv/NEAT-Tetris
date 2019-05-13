using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{

    public static BlockManager instance;

    private Tetromino currentTetromino;
    public BlockMatrix gameboardMatrix;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;


        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public enum Direction
    {
        LEFT,
        RIGHT,
        DOWN,
        NONE
    }

    public void init()
    {
        gameboardMatrix.init(38, 10);
        TetrominoRandomBag.instance.generateRandomBag();
    }

    public void moveTetrominoLeft()
    {
        if (checkMovement(Direction.LEFT))
        {
            CurrentTetromino.instance.transform.localPosition += Vector3.left;
        }
    }

    public void moveTetrominoRight()
    {
        if (checkMovement(Direction.RIGHT))
        {
            CurrentTetromino.instance.transform.localPosition += Vector3.right;
        }
    }

    public void rotateTetromino()
    {
        if (checkRotation())
        {
            CurrentTetromino.instance.rotate();
        }
    }

    public void fallCurrentTetromino()
    {
        if (checkMovement(Direction.DOWN))
        {
            CurrentTetromino.instance.transform.localPosition += Vector3.down;
        }
    }

    public int hardDrop()
    {
        int linesDroped = 0;
        while (checkMovement(Direction.DOWN) && linesDroped < 20)
        {
            CurrentTetromino.instance.transform.localPosition += Vector3.down;
            ++linesDroped;
        }
        lockCurrentTetromino();
        return linesDroped * 2;
    }

    public bool currentTetrominoLanded()
    {
        return !checkMovement(Direction.DOWN);
    }

    public void lockCurrentTetromino()
    {
        if (currentTetrominoAboveSkyLine())
        {
            GameManager.instance.setLockOut(true);
        }

        overlap(CurrentTetromino.instance.getShape(), CurrentTetromino.instance.transform.localPosition);
    }

    public Pattern getCurrentPattern()
    {
        return new Pattern(gameboardMatrix.getCompletedRows());
    }

    public void erasePatternLines(Pattern patern)
    {
        int[] clearedLines = patern.getAffectedLines();
        int numClearedLines = clearedLines.Length;
        if (numClearedLines == 0)
        {
            return;
        }
        for (int i = 0; i < numClearedLines; ++i)
        {
            gameboardMatrix.eraseLine(clearedLines[i]);
        }
    }

    public void takeOutTetromino()
    {
        Block.BlockType blockType = TetrominoRandomBag.instance.takeNext();
        UIManager.instance.setNextTetromino(TetrominoRandomBag.instance.getNext());
        CurrentTetromino.instance.setBlockType(blockType);
        CurrentTetromino.instance.setSpawnPosition();
        if (!checkMovement(Direction.NONE))
        {
            GameManager.instance.setBlockOut(true);
        }
        else
        {
            fallCurrentTetromino();
        }


    }

    public int[][] getNormalizedShape()
    {
        int[][] normalizedShape = new int[28][];
        for (int i = 0; i < 28; ++i)
        {
            normalizedShape[i] = new int[10];
            for (int j = 0; j < 10; ++j)
            {
                if (gameboardMatrix.getBlockType(i, j) == Block.BlockType.NONE)
                {
                    normalizedShape[i][j] = 0;
                }
                else
                {
                    normalizedShape[i][j] = 1;
                }

            }
        }
        return normalizedShape;
    }

    private bool checkMovement(Direction direction)
    {
        Vector3 nextPosition = CurrentTetromino.instance.transform.localPosition;

        switch (direction)
        {
            case Direction.DOWN:
                nextPosition += Vector3.down;
                break;
            case Direction.LEFT:
                nextPosition += Vector3.left;
                break;
            case Direction.RIGHT:
                nextPosition += Vector3.right;
                break;
            default:
                break;
        }

        return
            checkBoundaries(CurrentTetromino.instance.getShape(), nextPosition)
            && checkIntersection(CurrentTetromino.instance.getShape(), nextPosition);
    }

    private bool checkRotation()
    {
        Vector3 currentPosition = CurrentTetromino.instance.transform.localPosition;
        return
           checkBoundaries(CurrentTetromino.instance.getNextShape(), currentPosition)
           && checkIntersection(CurrentTetromino.instance.getNextShape(), currentPosition);
    }

    private bool checkBoundaries(Block.BlockType[][] tetrominoBlocks, Vector3 position)
    {
        int tetrominoHeight = tetrominoBlocks.Length;
        int tetrominoWidth = tetrominoBlocks[0].Length;

        for (int i = tetrominoHeight - 1; i >= 0; --i)
        {
            for (int j = 0; j < tetrominoWidth; ++j)
            {
                Block.BlockType currentTetrominoBlock = tetrominoBlocks[i][j];

                if (currentTetrominoBlock != Block.BlockType.NONE)
                {
                    int posY = i + (int)position.y;
                    int posX = j + (int)position.x;
                    if (posY < 0 || posY >= gameboardMatrix.getHeight() || posX < 0 || posX >= gameboardMatrix.getWidth())
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    private bool checkIntersection(Block.BlockType[][] tetrominoBlocks, Vector3 position)
    {
        int tetrominoHeight = tetrominoBlocks.Length;
        int tetrominoWidth = tetrominoBlocks[0].Length;

        for (int i = tetrominoHeight - 1; i >= 0; --i)
        {
            for (int j = 0; j < tetrominoWidth; ++j)
            {
                Block.BlockType currentTetrominoBlock = tetrominoBlocks[i][j];

                if (currentTetrominoBlock != Block.BlockType.NONE)
                {
                    int gameboardX = (int)position.x + j;
                    int gameboardY = (int)position.y + i;
                    if (gameboardMatrix.getBlockType(gameboardY, gameboardX) != Block.BlockType.NONE)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    private void overlap(Block.BlockType[][] tetrominoBlocks, Vector3 position)
    {
        int tetrominoHeight = tetrominoBlocks.Length;
        int tetrominoWidth = tetrominoBlocks[0].Length;

        for (int i = tetrominoHeight - 1; i >= 0; --i)
        {
            for (int j = 0; j < tetrominoWidth; ++j)
            {
                Block.BlockType currentTetrominoBlock = tetrominoBlocks[i][j];

                if (currentTetrominoBlock != Block.BlockType.NONE)
                {
                    int gameboardX = (int)position.x + j;
                    int gameboardY = (int)position.y + i;
                    Block.BlockType curentGameBoardBlock = gameboardMatrix.getBlockType(gameboardY, gameboardX);
                    if (curentGameBoardBlock == Block.BlockType.NONE)
                    {
                        gameboardMatrix.setBlockType(gameboardY, gameboardX, currentTetrominoBlock);
                    }
                    else
                    {
                        throw new System.Exception("Interesction not null while overlaping");
                    }
                }
            }
        }
    }

    private bool currentTetrominoAboveSkyLine()
    {
        Block.BlockType[][] tetrominoBlocks = CurrentTetromino.instance.getShape();
        Vector3 position = CurrentTetromino.instance.transform.localPosition;

        int tetrominoHeight = tetrominoBlocks.Length;
        int tetrominoWidth = tetrominoBlocks[0].Length;

        for (int i = tetrominoHeight - 1; i >= 0; --i)
        {
            for (int j = 0; j < tetrominoWidth; ++j)
            {
                Block.BlockType currentTetrominoBlock = tetrominoBlocks[i][j];

                if (currentTetrominoBlock != Block.BlockType.NONE)
                {
                    int gameboardY = (int)position.y + i;
                    if (gameboardY < 18)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    public int computeBumpiness()
    {
        int[] shape = gameboardMatrix.getShape();
        int bumpiness = 0;
        int previousHeight = shape[0];
        for (int i = 1; i < shape.Length; ++i)
        {
            bumpiness += (shape[i] - previousHeight);
            previousHeight = shape[i];
        }
        return bumpiness;
    }

    public int computeAggregatedHeight()
    {
        int[] shape = gameboardMatrix.getShape();
        int aggregateHeight = 0;
        for (int i = 0; i < 10; ++i)
        {
            aggregateHeight += shape[i];
        }
        return aggregateHeight;
    }

    public int computeHoles()
    {
        bool[][] visited = new bool[18][];
        for (int i = 0; i < 18; ++i)
        {
            visited[i] = new bool[10];
            for (int j = 0; j < 10; ++j)
            {
                visited[i][j] = false;
            }
        }

        Queue<KeyValuePair<int, int>> q = new Queue<KeyValuePair<int, int>>();
        q.Enqueue(new KeyValuePair<int, int>(17, 0));
        visited[17][0] = true;
        while (q.Count > 0)
        {
            KeyValuePair<int, int> current = q.Dequeue();
            for (int i = -1; i < 2; ++i)
            {
                for (int j = -1; j < 2; ++j)
                {
                    int y = i + current.Key;
                    int x = j + current.Value;
                    if (
                        (i != 0 || j != 0)
                        && (y >= 0)
                        && (y < 18)
                        && (x > 0)
                        && (x < 10)
                        && !visited[y][x]
                        && gameboardMatrix.getBlockType(y, x) == Block.BlockType.NONE
                    )
                    {
                        q.Enqueue(new KeyValuePair<int, int>(y, x));
                        visited[y][x] = true;
                    }
                }
            }
        }
        int numHoles = 0;
        for (int i = 0; i < 18; ++i)
        {
            for (int j = 0; j < 10; ++j)
            {
                if (!visited[i][j] && gameboardMatrix.getBlockType(i, j) == Block.BlockType.NONE)
                {
                    ++numHoles;
                }
            }
        }
        return numHoles;
    }

    public int computeEmptyness()
    {
        int[] shape = gameboardMatrix.getShape();
        int emptyness = 0;
        for (int j = 0; j < 10; ++j)
        {
            for (int i = 0; i < shape[j] - 1; ++i)
            {
                if (gameboardMatrix.getBlockType(i,j) == Block.BlockType.NONE)
                {
                    ++emptyness;
                }
            }
        }
        return emptyness;
    }

    public int computeAlmostCompletedLines()
    {
        int almostCompletedLines = 0;
        for (int i = 0; i < 18; ++i)
        {
            int blocksInLine = 0;
            for (int j = 0; j < 10; ++j)
            {
                if (gameboardMatrix.getBlockType(i,j) != Block.BlockType.NONE)
                {
                    ++blocksInLine;
                }
            }
            if (blocksInLine > 6)
            {
                ++almostCompletedLines;
            }
        }
        return almostCompletedLines;
    }
}
