using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public static InputManager instance;

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

    public enum GameInput
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        SPACE,
        NONE
    };

    public void input(GameInput input)
    {
        switch (input)
        {
            case GameInput.UP:
                rotateTetromino();
                break;
            case GameInput.DOWN:
                softDrop();
                break;
            case GameInput.LEFT:
                moveTetrominoLeft();
                break;
            case GameInput.RIGHT:
                moveTetrominoRight();
                break;
            case GameInput.SPACE:
                hardDrop();
                break;
            default:
                throw new System.Exception("Incorrect input");
        }
    }

    private void moveTetrominoLeft()
    {
        PhaseManager.instance.setMoveLeft(true);
    }

    private void moveTetrominoRight()
    {
        PhaseManager.instance.setMoveRight(true);
    }

    private void rotateTetromino()
    {
        PhaseManager.instance.setRotate(true);
    }

    private void softDrop()
    {
        PhaseManager.instance.setSoftDrop(true);
    }

    private void hardDrop()
    {
        PhaseManager.instance.setHardDrop(true);
    }


}
