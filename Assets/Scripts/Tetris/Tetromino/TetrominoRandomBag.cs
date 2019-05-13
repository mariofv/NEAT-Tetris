using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoRandomBag : MonoBehaviour {

    public static TetrominoRandomBag instance;

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

    Block.BlockType[] randomBag;
    int currentTetromino;
    System.Random r;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void startRandom(int seed)
    {
        r = new System.Random(seed);
    }

    public void generateRandomBag()
    {
        currentTetromino = 0;
        randomBag = new Block.BlockType[7];
        randomBag[0] = Block.BlockType.I;
        randomBag[1] = Block.BlockType.J;
        randomBag[2] = Block.BlockType.L;
        randomBag[3] = Block.BlockType.O;
        randomBag[4] = Block.BlockType.S;
        randomBag[5] = Block.BlockType.T;
        randomBag[6] = Block.BlockType.Z;

        for (int i = 0; i < randomBag.Length; i++)
        {
            Block.BlockType temp = randomBag[i];
            int randomIndex = r.Next(i, randomBag.Length);
            randomBag[i] = randomBag[randomIndex];
            randomBag[randomIndex] = temp;
        }
    }

    public Block.BlockType takeNext()
    {
        Block.BlockType nextTetromino = randomBag[currentTetromino];
        if (currentTetromino + 1 == randomBag.Length)
        {
            generateRandomBag();
        }
        else
        {
            ++currentTetromino;
        }
        return nextTetromino;
    }


    public Block.BlockType getNext()
    {
        return randomBag[currentTetromino];
    }
}
