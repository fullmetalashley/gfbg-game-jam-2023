using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentChange : MonoBehaviour
{
    public List<GameObject> environments;
    public int currentIndex;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeEnvironment(-1);
        }
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeEnvironment(1);
        }
    }

    //Not sure we'll stay with this method, but for now, it's fine
    public void ChangeEnvironment(int direction)
    {
        environments[currentIndex].SetActive(false);
        currentIndex += direction;

        if (currentIndex >= environments.Count)
        {
            currentIndex = 0;
        }

        if (currentIndex < 0)
        {
            currentIndex = environments.Count - 1;
        }
        environments[currentIndex].SetActive(true);
    }
}