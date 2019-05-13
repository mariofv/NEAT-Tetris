using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Number : MonoBehaviour {

    SpriteRenderer numberSprite;

    void Awake()
    {
        numberSprite = GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setNumber(int number)
    {
        if (number == 10)
        {
            numberSprite.enabled = false;
        }
        else
        {
            numberSprite.enabled = true;
            numberSprite.sprite = SpriteManager.instance.getNumber(number);
        }
    }
}
