using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TaTask
{
    public string startingArea;
    public string endingArea;

    public bool currentlyActive;

    public float timeToComplete;

    public string startingText;
    public string endingText;

    public string itemToPickUp;
}
