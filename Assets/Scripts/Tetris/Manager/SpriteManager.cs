using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour {

    public static SpriteManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            blockSprites = new Sprite[7];
            blockSprites[(int)Block.BlockType.I] = IBlockTexture;
            blockSprites[(int)Block.BlockType.J] = JBlockTexture;
            blockSprites[(int)Block.BlockType.L] = LBlockTexture;
            blockSprites[(int)Block.BlockType.O] = OBlockTexture;
            blockSprites[(int)Block.BlockType.S] = SBlockTexture;
            blockSprites[(int)Block.BlockType.T] = TBlockTexture;
            blockSprites[(int)Block.BlockType.Z] = ZBlockTexture;

            numberSprites = new Sprite[10];
            numberSprites[0] = Texture0;
            numberSprites[1] = Texture1;
            numberSprites[2] = Texture2;
            numberSprites[3] = Texture3;
            numberSprites[4] = Texture4;
            numberSprites[5] = Texture5;
            numberSprites[6] = Texture6;
            numberSprites[7] = Texture7;
            numberSprites[8] = Texture8;
            numberSprites[9] = Texture9;

            tetrominoSprites = new Sprite[7];
            tetrominoSprites[(int)Block.BlockType.I] = ITetrominoTexture;
            tetrominoSprites[(int)Block.BlockType.J] = JTetrominoTexture;
            tetrominoSprites[(int)Block.BlockType.L] = LTetrominoTexture;
            tetrominoSprites[(int)Block.BlockType.O] = OTetrominoTexture;
            tetrominoSprites[(int)Block.BlockType.S] = STetrominoTexture;
            tetrominoSprites[(int)Block.BlockType.T] = TTetrominoTexture;
            tetrominoSprites[(int)Block.BlockType.Z] = ZTetrominoTexture;

            ITetrominoTextures = new Sprite[2];
            ITetrominoTextures[0] = ITetrominoTexture;
            ITetrominoTextures[1] = ITetrominoTexture2;

            JTetrominoTextures = new Sprite[4];
            JTetrominoTextures[0] = JTetrominoTexture;
            JTetrominoTextures[1] = JTetrominoTexture2;
            JTetrominoTextures[2] = JTetrominoTexture3;
            JTetrominoTextures[3] = JTetrominoTexture4;

            LTetrominoTextures = new Sprite[4];
            LTetrominoTextures[0] = LTetrominoTexture;
            LTetrominoTextures[1] = LTetrominoTexture2;
            LTetrominoTextures[2] = LTetrominoTexture3;
            LTetrominoTextures[3] = LTetrominoTexture4;

            STetrominoTextures = new Sprite[2];
            STetrominoTextures[0] = STetrominoTexture;
            STetrominoTextures[1] = STetrominoTexture2;

            TTetrominoTextures = new Sprite[4];
            TTetrominoTextures[0] = TTetrominoTexture;
            TTetrominoTextures[1] = TTetrominoTexture2;
            TTetrominoTextures[2] = TTetrominoTexture3;
            TTetrominoTextures[3] = TTetrominoTexture4;

            ZTetrominoTextures = new Sprite[2];
            ZTetrominoTextures[0] = ZTetrominoTexture;
            ZTetrominoTextures[1] = ZTetrominoTexture2;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }


    public Sprite IBlockTexture;
    public Sprite JBlockTexture;
    public Sprite LBlockTexture;
    public Sprite OBlockTexture;
    public Sprite SBlockTexture;
    public Sprite TBlockTexture;
    public Sprite ZBlockTexture;

    Sprite[] blockSprites;

    public Sprite Texture0;
    public Sprite Texture1;
    public Sprite Texture2;
    public Sprite Texture3;
    public Sprite Texture4;
    public Sprite Texture5;
    public Sprite Texture6;
    public Sprite Texture7;
    public Sprite Texture8;
    public Sprite Texture9;

    Sprite[] numberSprites;

    public Sprite ITetrominoTexture;
    public Sprite ITetrominoTexture2;
    Sprite[] ITetrominoTextures;

    public Sprite JTetrominoTexture;
    public Sprite JTetrominoTexture2;
    public Sprite JTetrominoTexture3;
    public Sprite JTetrominoTexture4;
    Sprite[] JTetrominoTextures;

    public Sprite LTetrominoTexture;
    public Sprite LTetrominoTexture2;
    public Sprite LTetrominoTexture3;
    public Sprite LTetrominoTexture4;
    Sprite[] LTetrominoTextures;

    public Sprite OTetrominoTexture;

    public Sprite STetrominoTexture;
    public Sprite STetrominoTexture2;
    Sprite[] STetrominoTextures;

    public Sprite TTetrominoTexture;
    public Sprite TTetrominoTexture2;
    public Sprite TTetrominoTexture3;
    public Sprite TTetrominoTexture4;
    Sprite[] TTetrominoTextures;

    public Sprite ZTetrominoTexture;
    public Sprite ZTetrominoTexture2;
    Sprite[] ZTetrominoTextures;

    Sprite[] tetrominoSprites;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public Sprite getNumber(int number)
    {
        return numberSprites[number];
    }

    public Sprite getBlock(Block.BlockType type)
    {
        return blockSprites[(int)type];
    }

    public Sprite getTetromino(Block.BlockType type)
    {
        return tetrominoSprites[(int)type];
    }

    public Sprite getTetromino(Block.BlockType type, int i)
    {
        switch (type)
        {
            case Block.BlockType.I:
                return ITetrominoTextures[i];
            case Block.BlockType.J:
                return JTetrominoTextures[i];
            case Block.BlockType.L:
                return LTetrominoTextures[i];
            case Block.BlockType.O:
                return OTetrominoTexture;
            case Block.BlockType.S:
                return STetrominoTextures[i];
            case Block.BlockType.T:
                return TTetrominoTextures[i];
            case Block.BlockType.Z:
                return ZTetrominoTextures[i];
            default:
                throw new Exception("Incorrect block type when rotating: " + type);
        }
    }
}
