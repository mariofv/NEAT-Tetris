using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardManager : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            InputManager.instance.input(InputManager.GameInput.LEFT);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            InputManager.instance.input(InputManager.GameInput.RIGHT);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            InputManager.instance.input(InputManager.GameInput.UP);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            InputManager.instance.input(InputManager.GameInput.DOWN);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InputManager.instance.input(InputManager.GameInput.SPACE);
        }
    }
}
