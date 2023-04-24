using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public List<TaTask> allTasks;

    public TaTask currentTask;
    
    [Header("UI Elements")]
    public Animator taskUIAnim;
    public TextMeshProUGUI taskText;
    public TextMeshProUGUI taskTimer;

    private float timer;

    private EnvironmentChange environmentController;

    void Start()
    {
        environmentController = FindObjectOfType<EnvironmentChange>();
    }
    
    void Update()
    {
        if (currentTask != null && currentTask.currentlyActive)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                //This means the player has run out of time.
            }
        }
    }
    
    public void RunTask(){
        //Get a task from the list of tasks. Choose a random one.
        int randomTask = Random.Range(0, allTasks.Count);
        TaTask thisTask = allTasks[randomTask];
        
        //Next step: Run the UI
        taskUIAnim.SetBool("isActive", true);
        
        //Next: Skin the UI
        taskText.text = thisTask.startingText;
        
        //Set our timer to match the timer of the task.
        timer = thisTask.timeToComplete;
    }

    public void UpdateTask()
    {
        //Tasks update on a room change. 
        //We check tasks in a few stages:
        //1. Starting room, populate starting text and start timer
        //2. Room of item pickup, change text
        //3. Room of item deposit, change text and end timer
        //These might occur in the same room. Let's create a task.
        
        //First, if there is no active task, we need to run one.
        if (currentTask != null)
        {
            RunTask();
        }
        else
        {
            //We have an active task. So let's break it down.
            //We can assume it has been started. We need to check what state of the task we're in.
        }
    }

    public void EndTask()
    {
        
    }
    
    public void TaskFailure()
    {
        
    }

    public void TaskSuccess()
    {
        
    }
    
}
