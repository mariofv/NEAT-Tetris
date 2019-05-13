using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    private int currentScore;
    private int currentLines;
    private int goalLines;
    private int currentLevel;

    private float timeToMove;

    private bool lockOut;
    private bool blockOut;

    private int speed;

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

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void startGame()
    {
        currentLevel = 0;
        UIManager.instance.setLevel(currentLevel);

        currentScore = 0;
        UIManager.instance.setScore(currentScore);

        currentLines = 0;
        UIManager.instance.setLines(currentLines);

        goalLines = 5;
        timeToMove = Mathf.Pow(0.8f - ((currentLevel - 1) * 0.007f), currentLevel - 1);

        lockOut = false;
        blockOut = false;

        PhaseManager.instance.init();
        BlockManager.instance.init();
    }

    public void addScore(int score)
    {
        currentScore = Mathf.Min(999999, score + currentScore);
        UIManager.instance.setScore(currentScore);
    }

    public int getScore()
    {
        return currentScore;
    }

    public void addLines(int lines)
    {
        currentLines = Mathf.Min(9999, lines + currentLines);
        UIManager.instance.setLines(currentLines);

        if (currentLines >= goalLines)
        {
            if (currentLevel < 15)
            {
                levelUp();
                timeToMove = Mathf.Pow(0.8f - ((currentLevel - 1) * 0.007f), currentLevel - 1);
                goalLines += currentLevel * 5;
            }
        }
    }

    public int getClearedLines()
    {
        return currentLines;
    }

    public void levelUp()
    {
        currentLevel = Mathf.Min(9999, currentLevel + 1);
        UIManager.instance.setLevel(currentLevel);
    }

    public void setLockOut(bool lockOut)
    {
        this.lockOut = lockOut;
    }

    public void setBlockOut(bool blockOut)
    {
        this.blockOut = blockOut;
    }

    public float getTimeToMove()
    {
        return timeToMove / speed;
    }

    public void setSpeed(int speed)
    {
        this.speed = speed;
    }

    public bool gameOver()
    {
        return blockOut || lockOut;
    }
}
