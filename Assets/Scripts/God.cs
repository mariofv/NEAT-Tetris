using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class God : MonoBehaviour {
    
    private float timeSinceInput;
    
	// Use this for initialization
	void Start () {
        NEAT.Instance.init();
        //TetrominoRandomBag.instance.startRandom(NEAT.Instance.getEpochNumber());
        TetrominoRandomBag.instance.startRandom(1);
        GameManager.instance.startGame();
        GameManager.instance.setSpeed(10000);
        timeSinceInput = 0;
    }

    // Update is called once per frame
    void Update () {
        if (!NEAT.Instance.hasFinished())
        {
            timeSinceInput += Time.deltaTime;

            if (GameManager.instance.gameOver())
            {
                NEATAdapter.instance.endGame();
                GameManager.instance.startGame();
            }

            if (timeSinceInput > 0)
            {
                NEATAdapter.instance.inputateCommand();
                timeSinceInput = 0;
            }
        }
        else
        {
            Application.Quit();
        }
        

    }
}
