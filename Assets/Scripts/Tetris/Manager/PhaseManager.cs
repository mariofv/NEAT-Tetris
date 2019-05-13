using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour {
    public static PhaseManager instance;

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

    private enum Phase
    {
        GENERATION_PHASE,
        FALLING_PHASE,
        LOCK_PHASE,
        PATTERN_PHASE,
        ITERATE_PHASE,
        ANIMATE_PHASE,
        COMPLETION_PHASE
    };

    Phase currentPhase;
    float timeCounter;
    bool hardDrop;
    bool softDrop;
    bool rotate;
    bool moveRight;
    bool moveLeft;


    public void init()
    {
        currentPhase = Phase.GENERATION_PHASE;
        timeCounter = 0;
        hardDrop = false;
        softDrop = false;
        rotate = false;
        moveRight = false;
        moveLeft = false;
    }

    public void Update()
    {
        if (NEAT.Instance.hasFinished())
        {
            return;
        }
        switch (currentPhase)
        {
            case Phase.GENERATION_PHASE:
                BlockManager.instance.takeOutTetromino();
                currentPhase = Phase.FALLING_PHASE;
                break;

            case Phase.FALLING_PHASE:
                if (hardDrop)
                {
                    hardDrop = false;
                    int hardDropScore = BlockManager.instance.hardDrop();
                    NEATAdapter.instance.lockPiece();
                    GameManager.instance.addScore(hardDropScore);
                    currentPhase = Phase.PATTERN_PHASE;
                }
                else if (softDrop)
                {
                    timeCounter = 0;
                    softDrop = false;
                    GameManager.instance.addScore(1);
                    BlockManager.instance.fallCurrentTetromino();
                    if (BlockManager.instance.currentTetrominoLanded())
                    {
                        currentPhase = Phase.LOCK_PHASE;
                    }
                }
                else
                {
                    solvePendingMovements();
                    timeCounter += Time.deltaTime;
                    if (timeCounter >= GameManager.instance.getTimeToMove())
                    {
                        timeCounter = 0;
                        BlockManager.instance.fallCurrentTetromino();

                        if (BlockManager.instance.currentTetrominoLanded())
                        {
                            currentPhase = Phase.LOCK_PHASE;
                        }
                    }
                }
                break;

            case Phase.LOCK_PHASE:
                BlockManager.instance.lockCurrentTetromino();
                NEATAdapter.instance.lockPiece();
                currentPhase = Phase.PATTERN_PHASE;
                break;

            case Phase.PATTERN_PHASE:
                Pattern pattern = BlockManager.instance.getCurrentPattern();

                GameManager.instance.addLines(pattern.getRewardedLines());
                GameManager.instance.addScore(pattern.getScore());

                BlockManager.instance.erasePatternLines(pattern);
                currentPhase = Phase.COMPLETION_PHASE;
                break;

            case Phase.COMPLETION_PHASE:
                currentPhase = Phase.GENERATION_PHASE;
                break;

            default:
                break;
        }

    }

    public void setHardDrop(bool value)
    {
        hardDrop = value;
    }

    public void setSoftDrop(bool value)
    {
        softDrop = value;
    }

    public void setRotate(bool value)
    {
        rotate = true;
    }

    public void setMoveRight(bool value)
    {
        moveRight = true;
    }

    public void setMoveLeft(bool value)
    {
        moveLeft = true;
    }

    private void solvePendingMovements()
    {
        if (moveLeft)
        {
            BlockManager.instance.moveTetrominoLeft();
            moveLeft = false;
        }
        else if (moveRight)
        {
            BlockManager.instance.moveTetrominoRight();
            moveRight = false;
        }
        else if (rotate)
        {
            BlockManager.instance.rotateTetromino();
            rotate = false;
        }
    }

}
