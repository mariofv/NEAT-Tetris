using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public static UIManager instance = null;

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


    public NumberDisplayer scoreDisplayer;
    public NumberDisplayer linesDisplayer;
    public NumberDisplayer levelDisplayer;
    public TetrominoSprite nextTetrominoDisplayer;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setScore(int score)
    {
        scoreDisplayer.setNumber(score);
    }

    public void setLevel(int level)
    {
        levelDisplayer.setNumber(level);
    }

    public void setLines(int lines)
    {
        linesDisplayer.setNumber(lines);
    }

    public void setNextTetromino(Block.BlockType type)
    {
        nextTetrominoDisplayer.changeBlockSprite(type);
    }
}
