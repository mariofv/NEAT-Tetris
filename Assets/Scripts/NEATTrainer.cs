using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEATTrainer {

    private static NEATTrainer instance = null;
    public static NEATTrainer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new NEATTrainer();
            }
            return instance;
        }
    }

    private int posX;
    private int posY;

    private int[][] gameboard;
    private int[][] tetrominoShape;

    bool softDrop;
    bool hardDrop;

    private Tetromino tetromino;

    public void train()
    {
        bool endEpoch = false;
        generateRandomFrame();
        double[] randomFrame = FrameGenerator.generateFrame(posX, posY, tetrominoShape, gameboard);

        while (!endEpoch)
        {
            int command = NEAT.Instance.feedCurrentGenome(randomFrame);
            InputManager.GameInput input;

            switch (command)
            {
                case -1:
                    input = InputManager.GameInput.NONE;
                    return;
                case 0:
                    input = InputManager.GameInput.UP;
                    break;
                case 1:
                    input = InputManager.GameInput.LEFT;
                    break;
                case 2:
                    input = InputManager.GameInput.RIGHT;
                    break;
                case 3:
                    input = InputManager.GameInput.DOWN;
                    break;
                case 4:
                    input = InputManager.GameInput.SPACE;
                    break;
                default:
                    throw new System.Exception("Incorrect number of output activation " + command);
            }
            double previousFitness = computeFitness();
            moveTetromino(input);
            fallTetromino();
            double fitness = computeFitness();
            endEpoch = NEAT.Instance.nextGenome(fitness- previousFitness);
        }
    }

    private double computeFitness()
    {
        int clearedLines = computeClearedLines();
        float bumpiness = computeBumpiness();
        int aggregatedHeight = computeAggregatedHeight();
        int holes = computeHoles();
        return
             - 0.18 *bumpiness
             - 0.5 *aggregatedHeight
             - 0.35 *holes
             + 0.76*clearedLines;
    }

    private int fallTetromino()
    {
        int falledLines = 0;
        bool canFall = checkMovement(BlockManager.Direction.DOWN);
        while (canFall)
        {
            --posY;
            ++falledLines;
            canFall = checkMovement(BlockManager.Direction.DOWN);
        }
        lockTetromino();
        return falledLines;
    }

    private void lockTetromino()
    {
        int tetrominoHeight = tetrominoShape.Length;
        int tetrominoWidth = tetrominoShape[0].Length;

        for (int i = tetrominoHeight - 1; i >= 0; --i)
        {
            for (int j = 0; j < tetrominoWidth; ++j)
            {
                int currentTetrominoBlock = tetrominoShape[i][j];

                if (currentTetrominoBlock != 0)
                {
                    int gameboardX = posX + j;
                    int gameboardY = posY + i;
                    gameboard[gameboardY][gameboardX] = 1;
                }
            }
        }
    }

    private int computeHoles()
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
        
        Queue<KeyValuePair<int,int>> q = new Queue<KeyValuePair<int, int>>();
        q.Enqueue(new KeyValuePair<int, int>(17,0));
        visited[17][0] = true;
        while (q.Count > 0)
        {
            KeyValuePair<int,int> current = q.Dequeue();
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
                        && gameboard[y][x] == 0
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
                if (!visited[i][j] && gameboard[i][j] == 0)
                {
                    ++numHoles;
                }
            }
        }
        return numHoles;
    }


    private int computeClearedLines()
    {
        int completedLines = 0;

        for (int i = 0; i < 28; ++i)
        {
            bool completed = true;

            for (int j = 0; j < 10; ++j)
            {
                if (gameboard[i][j] == 0)
                {
                    completed = false;
                }
            }

            if (completed)
            {
                ++completedLines;
            }
        }

        return completedLines;
    }

    private double computeObtainedScore(double fallenLines, int clearedLines)
    {
        double obtainedScore = 0;
        if (softDrop)
        {
            ++obtainedScore;
        }
        if (hardDrop)
        {
            obtainedScore += fallenLines * 2;
        }
        int linesScore;
        switch (clearedLines)
        {
            case 0:
                linesScore = 0;
                break;
            case 1:
                linesScore = 100;
                break;
            case 2:
                linesScore = 300;
                break;
            case 3:
                linesScore = 500;
                break;
            case 4:
                linesScore = 800;
                break;
            default:
                throw new System.Exception("Too many lines at once " + clearedLines);
        }

        obtainedScore += linesScore;
        return obtainedScore;
    }

    private int computeAggregatedHeight()
    {
        int aggregateHeight = 0;
        for (int i = 0; i < 10; ++i)
        {
            aggregateHeight += getColumnHeight(i);
        }
        return aggregateHeight;
    }

    private int computeBumpiness()
    {
        int[] shape = getGameboardShape();
        int bumpiness = 0;
        int previousHeight = shape[0];
        for (int i = 1; i < shape.Length; ++i)
        {
            bumpiness += (shape[i] - previousHeight);
            previousHeight = shape[i];
        }
        return bumpiness;
    }

    private int computeWidth()
    {
        int[] shape = getGameboardShape();
        int min = -1;
        int max = -2;
        int i = 0;
        while (min == -1 && i < shape.Length)
        {
            if (shape[i] != 0)
            {
                min = i;
            }
            ++i;
        }
        i = shape.Length - 1;
        while (max == -2 && i >= 0)
        {
            if (shape[i] != 0)
            {
                max = i;
            }
            --i;
        }

        return max - min + 1;
    }

    private int[] getGameboardShape()
    {

        int[] shape = new int[10];
        for (int i = 0; i < 10; ++i)
        {
            shape[i] = getColumnHeight(i);
        }

        return shape;
    }

    private int getColumnHeight(int column)
    {
        for (int i = 28 - 1; i >= 0; --i)
        {
            if (gameboard[i][column] != 0)
            {
                return i + 1;
            }
        }

        return 0;
    }

    private void moveTetromino(InputManager.GameInput input)
    {

        softDrop = false;
        hardDrop = false;
        switch (input)
        {
            case InputManager.GameInput.NONE:
                break;
            case InputManager.GameInput.UP:
                if (checkRotation())
                {
                    tetrominoShape = tetromino.getNextNormalizedShape();
                }
                break;
            case InputManager.GameInput.LEFT:
                if (checkMovement(BlockManager.Direction.LEFT))
                {
                    --posX;
                }
                break;
            case InputManager.GameInput.RIGHT:
                if (checkMovement(BlockManager.Direction.RIGHT))
                {
                    ++posX;
                }
                break;
            case InputManager.GameInput.DOWN:
                softDrop = true;
                break;
            case InputManager.GameInput.SPACE:
                hardDrop = true;
                break;
        }
    }

    private void generateRandomFrame()
    {
        bool validFrame = false;
        while (!validFrame)
        {
            gameboard = new int[28][];
            
            for (int i = 0; i < 28; ++i)
            {
                gameboard[i] = new int[10];
            }

            int previousHeight = Utils.generateRandomNumber(18); //HALF OF GAMEBOARD HEIGHT

            for (int i = 0; i < 10; ++i)
            {
                int currentHeight;
                int r1 = Utils.generateRandomNumber();
                if (r1 < 10)
                {
                    currentHeight = Utils.generateRandomNumber(9);
                }
                else
                {
                    int r2 = Utils.generateRandomNumber(2);
                    int spike = Utils.generateRandomNumber(5);
                    currentHeight = previousHeight;
                    if (r2 == 0)
                    {
                        currentHeight = Mathf.Min(currentHeight + spike, 9);
                    }
                    else
                    {
                        currentHeight = Mathf.Max(currentHeight - spike, 0);
                    }
                }
                
                for (int j = 0; j < 28; ++j)
                {
                    if (j < currentHeight)
                    {
                        gameboard[j][i] = 1;
                    }
                    else
                    {
                        gameboard[j][i] = 0;
                    }
                    
                }
            }

            for (int i = 0; i < 18; ++i)
            {
                bool clearedLine = true;
                for (int j = 0; j < 10; ++j)
                {
                    if (gameboard[i][j] == 0)
                    {
                        clearedLine = false;
                    }
                }
                if (clearedLine)
                {
                    int r = Utils.generateRandomNumber(10);
                    gameboard[i][r] = 0;
                }

            }

            posX = Utils.generateRandomNumber(10);
            posY = Utils.generateRandomNumber(18);

            generateRandomTetromino();
            tetrominoShape = tetromino.getNormalizedShape();
            validFrame = checkFrame(posX, posY, tetrominoShape, gameboard);
        }
    }

    private int[] generateRandomPermutation()
    {
        int[] randomBag = new int[10];
        for (int i = 0; i < 10; ++i)
        {
            randomBag[i] = i;
        }

        for (int i = 0; i < randomBag.Length; i++)
        {
            int temp = randomBag[i];
            int randomIndex = Utils.generateRandomNumber(i, randomBag.Length);
            randomBag[i] = randomBag[randomIndex];
            randomBag[randomIndex] = temp;
        }

        return randomBag;
    }

    private void generateRandomTetromino()
    {
        int r = Utils.generateRandomNumber(7);
        switch (r)
        {
            case 0:
                tetromino = new ITetromino();
                break;
            case 1:
                tetromino = new JTetromino();
                break;
            case 2:
                tetromino = new LTetromino();
                break;
            case 3:
                tetromino = new OTetromino();
                break;
            case 4:
                tetromino = new STetromino();
                break;
            case 5:
                tetromino = new TTetromino();
                break;
            case 6:
                tetromino = new ZTetromino();
                break;
            default:
                throw new System.Exception("This should never happen");
        }

        tetromino.rotateRandom();
    }

    bool checkFrame(int posX, int posY, int[][] tetrominoShape, int[][] gameboard)
    {
        return checkBoundaries(posX, posY, tetrominoShape, gameboard) && checkOverlap(posX, posY, tetrominoShape, gameboard);
    }

    private bool checkMovement(BlockManager.Direction direction)
    {
        int nextX = posX;
        int nextY = posY;
        switch (direction)
        {
            case BlockManager.Direction.DOWN:
                --nextY;
                break;
            case BlockManager.Direction.LEFT:
                --nextX;
                break;
            case BlockManager.Direction.RIGHT:
                ++nextX;
                break;
            default:
                break;
        }

        return
            checkBoundaries(nextX, nextY, tetrominoShape, gameboard)
            && checkOverlap(nextX, nextY, tetrominoShape, gameboard);
    }

    private bool checkRotation()
    {
        int[][] nextShape = tetromino.getNextNormalizedShape();
        return
            checkBoundaries(posX, posY, nextShape, gameboard)
            && checkOverlap(posX, posY, nextShape, gameboard);
    }


    bool checkBoundaries(int posX, int posY, int[][] tetrominoShape, int[][] gameboard)
    {
        int tetrominoHeight = tetrominoShape.Length;
        int tetrominoWidth = tetrominoShape[0].Length;

        for (int i = tetrominoHeight - 1; i >= 0; --i)
        {
            for (int j = 0; j < tetrominoWidth; ++j)
            {
                int currentTetrominoBlock = tetrominoShape[i][j];

                if (currentTetrominoBlock == 1)
                {
                    int y = i + posY;
                    int x = j + posX;
                    if (y < 0 || y >= 28 || x < 0 || x >= 10)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    bool checkOverlap(int posX, int posY, int[][] tetrominoShape, int[][] gameboard)
    {
        int tetrominoHeight = tetrominoShape.Length;
        int tetrominoWidth = tetrominoShape[0].Length;

        for (int i = tetrominoHeight - 1; i >= 0; --i)
        {
            for (int j = 0; j < tetrominoWidth; ++j)
            {
                int currentTetrominoBlock = tetrominoShape[i][j];

                if (currentTetrominoBlock == 1)
                {
                    int gameboardX = posX + j;
                    int gameboardY = posY + i;
                    if (gameboard[gameboardY][gameboardX] == 1)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

}
