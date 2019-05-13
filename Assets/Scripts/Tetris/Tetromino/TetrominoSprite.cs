using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoSprite : MonoBehaviour {

   
    SpriteRenderer tetrominoSprite;
    Block.BlockType currentBlockType;

    // Use this for initialization
    void Start()
    {
        tetrominoSprite = GetComponent<SpriteRenderer>();
        currentBlockType = Block.BlockType.NONE;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void changeBlockSprite(Block.BlockType type)
    {
        currentBlockType = type;
        tetrominoSprite.sprite = SpriteManager.instance.getTetromino(type);
    }

    public void changeBlockSprite(int num)
    {
        tetrominoSprite.sprite = SpriteManager.instance.getTetromino(currentBlockType, num);
    }
}