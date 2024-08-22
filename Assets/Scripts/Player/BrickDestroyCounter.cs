using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickDestroyCounter : MonoBehaviour
{
    [SerializeField] CounterDisplayer displayer;
    [SerializeField] int counter = 0;
    public int Counter => counter;

    public void Increase()
    {
        counter++;
        displayer.Display(counter);
    }

    public void Reset()
    {
        counter = 0;
    }
}
