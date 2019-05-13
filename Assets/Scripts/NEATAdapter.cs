using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEATAdapter : MonoBehaviour {
    public static NEATAdapter instance;

    int lockedPieces;

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

    public void init()
    {
        lockedPieces = 0;
    }

    public void inputateCommand()
    {
        double[] frameInfo = generateFrameInfo();
        int output = NEAT.Instance.feedCurrentGenome(frameInfo);

        InputManager.GameInput input;

        switch (output)
        {
            case -1:
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
                throw new System.Exception("Incorrect number of output activation " + output);
        }

        InputManager.instance.input(input);
    }

    public void lockPiece()
    {
        ++lockedPieces;
    }

    public void endGame()
    {
        double fitness = computeFitness();
        lockedPieces = 0;
        bool endEpoch = NEAT.Instance.nextGenome(fitness);
        //TetrominoRandomBag.instance.startRandom(NEAT.Instance.getEpochNumber());
        TetrominoRandomBag.instance.startRandom(1);

    }

    private double[] generateFrameInfo()
    {
        int[][] boardShape = BlockManager.instance.getNormalizedShape();
        int[][] tetrominoShape = CurrentTetromino.instance.getNormalizedShape();
        Vector3 position = CurrentTetromino.instance.transform.localPosition;
        double posX = position.x;
        double posY = position.y;
        return FrameGenerator.generateFrame(posX, posY, tetrominoShape, boardShape);
    }

    private double computeFitness()
    {
        int clearedLines = GameManager.instance.getClearedLines();
        float bumpiness = BlockManager.instance.computeBumpiness();
        int holes = BlockManager.instance.computeHoles();
        int almostCompletedLines = BlockManager.instance.computeAlmostCompletedLines();
        return
             5 * almostCompletedLines
             +  lockedPieces
             + 10*clearedLines
             - holes
             - 0.5 * bumpiness;
    }

}
