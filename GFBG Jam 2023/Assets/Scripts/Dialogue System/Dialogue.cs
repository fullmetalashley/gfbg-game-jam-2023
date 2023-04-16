using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name;
    
    [TextArea(3, 10)]
    public List<string> sentences;

    public string trigger;

    public Dialogue(string _name, List<string> _sentences, string _trigger)
    {
        name = _name;
        sentences = _sentences;
        trigger = _trigger;
    }
}
