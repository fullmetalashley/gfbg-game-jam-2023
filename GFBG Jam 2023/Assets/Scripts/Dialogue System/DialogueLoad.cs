using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueLoad : MonoBehaviour
{

    public List<Dialogue> loaded;

    public DialogueTrigger[] allTriggers;
    
    // Start is called before the first frame update
    void Start()
    {
        allTriggers = FindObjectsOfType<DialogueTrigger>();

        LoadDialogue();
        SetTriggers();
    }

    public void LoadDialogue()
    {
        TextAsset[] keys = Resources.LoadAll<TextAsset>("Keys");

        for (int i = 0; i < keys.Length; i++)
        {
            string[] dummy = keys[i].text.Split("\n"[0]);
            //Index 0: Name
            //Index 1: Trigger
            //Index 2 - end: Sentences
            
            dummy[0] = dummy[0].Trim(); //Not necessary but good practice.
            dummy[1] = dummy[1].Trim();
            
            //Length of sentences is length of list, - 2. 

            List<string> dummySentences = new List<string>();

            for (int y = 2; y < dummy.Length; y++)
            {
                dummySentences.Add(dummy[y]);
            }

            Dialogue toLoad = new Dialogue(dummy[0], dummySentences, dummy[1]);
            loaded.Add(toLoad);
            
        }
    }

    public void SetTriggers()
    {
        foreach (DialogueTrigger trigger in allTriggers)
        {
            for (int i = 0; i < loaded.Count; i++)
            {
                if (loaded[i].trigger == trigger.trigger)
                {
                    trigger.dialogue = loaded[i];
                    break;
                }
            }
        }
    }
}
