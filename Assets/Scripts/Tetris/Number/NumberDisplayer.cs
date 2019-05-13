using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberDisplayer : MonoBehaviour {

    public int size;
    public int maxNumber;
    Number[] numbers;

    private void Awake()
    {
        numbers = new Number[size];
        for (int i = 0; i < size; ++i)
        {
            GameObject newNumber = Instantiate(Resources.Load("Number"), this.transform) as GameObject;
            numbers[i] = newNumber.GetComponent<Number>();
            newNumber.transform.localPosition = new Vector3(-i, 0, 0);
        }
        setNumber(0);
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
        if (number < 0)
        {
            return;
        }
        int numberToDisplay = Mathf.Min(number, maxNumber);
        for (int i = 0; i < size; ++i)
        {
            numbers[i].setNumber(10);
        }

        if (number == 0)
        {
            numbers[0].setNumber(0);
            return;
        }

        int digit = 0;
        while (numberToDisplay > 0)
        {
            numbers[digit].setNumber(numberToDisplay % 10);
            numberToDisplay = numberToDisplay / 10;
            ++digit;
        }
    }
}
