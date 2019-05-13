using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    public enum BlockType
    {
        I,
        J,
        L,
        O,
        S,
        T,
        Z,
        NONE
    }


    BlockType blockType;
    SpriteRenderer blockSprite;

    void Awake()
    {
        blockSprite = GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start () {
        
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void changeBlockSprite(BlockType type)
    {
        blockType = type;
        if (blockType == BlockType.NONE)
        {
            blockSprite.enabled = false;
        }
        else
        {
            blockSprite.enabled = true;
            blockSprite.sprite = SpriteManager.instance.getBlock(blockType);
        }
        
    }

    public BlockType getBlockType()
    {
        return blockType;
    }
}
