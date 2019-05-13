using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NEATUI : MonoBehaviour {

    public static NEATUI instance;

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

    public Slider progressBar;
    public Text progressText;

    public void displayProgress(float value)
    {
        progressBar.value = value;
        progressText.text = (100 * value).ToString() + " %";
    }

    public void disableProgress()
    {
        progressBar.gameObject.SetActive(false);
    }
}
