using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Yarn.Unity;
using Random = UnityEngine.Random;

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

    public bool haveItem;

    private bool succeeded;

    void Start()
    {
        environmentController = FindObjectOfType<EnvironmentChange>();
    }
    
    void Update()
    {
        if (currentTask != null && currentTask.currentlyActive)
        {
            timer -= Time.deltaTime;
            taskTimer.text = Mathf.RoundToInt(timer) + "";
        }

        //TODO: Adjust input key
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("You'll pick up an item here");
            haveItem = true;
            ItemCheck();
        }
    }

    IEnumerator TaskEndedDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        taskUIAnim.SetBool("isActive", false);
    }
    
    [YarnCommand("run_task")]
    public void RunTask(){
        //Get a task from the list of tasks. Choose a random one.
        succeeded = false;
        
        int randomTask = Random.Range(0, allTasks.Count);
        TaTask thisTask = allTasks[randomTask];

        currentTask = thisTask;
        currentTask.currentlyActive = true;
        
        //Next step: Run the UI
        taskUIAnim.SetBool("isActive", true);
        
        //Next: Skin the UI
        taskText.text = thisTask.startingText;
        
        //Set our timer to match the timer of the task.
        timer = thisTask.timeToComplete;
    }

    //Checks to see the status of item grabbing / depositing.
    public void ItemCheck()
    {
        //If we have the item, we need to check the room.
        //If we don't have the item, we need to see if we're in the room where we get the item.

        if (!haveItem)
        {
            if (currentTask.startingArea == environmentController.currentEnvironment)
            {
                //We're in the right room, give the player the item and update the text.
                Debug.Log("You have the item");
                haveItem = true;
                UpdateTask();
            }
        }
        else
        {
            //Check to see if we are in the right room to get the item
            if (currentTask.startingArea == environmentController.currentEnvironment)
            {
                Debug.Log("You dropped off the item!");
                succeeded = true;
                StopAllCoroutines();
                EndTask();
            }
        }
    }

    //Updates the UI text based on where the player is in the environment.
    public void UpdateTask()
    {
        //Tasks update on a room change. 
        //We check tasks in a few stages:
        //1. Starting room, populate starting text and start timer
        //2. Room of item pickup, change text
        //3. Room of item deposit, change text and end timer
        //These might occur in the same room. Let's create a task.
        
        //First, if there is no active task, we need to run one.
        if (currentTask == null)
        {
            RunTask();
        }
        else
        {
            //We have an active task. So let's break it down.
            //We can assume it has been started. We need to check what state of the task we're in.
            if (environmentController.currentEnvironment != currentTask.endingArea)
            {
                taskText.text = "Not the right room, keep looking!";
            }
            else
            {
                taskText.text = currentTask.endingText;
            }
        }
    }

    [YarnCommand("custom_wait")]
    IEnumerator StartTimer()
    {
        RunTask();
        yield return new WaitForSeconds(currentTask.timeToComplete);
        EndTask();
    }

    [YarnCommand("debug_yarn")]
    public void CalledFromYarn()
    {
        Debug.Log("Yarn called us!");
    }

    public void EndTask()
    {
        currentTask.currentlyActive = false;
        currentTask = null;

        taskText.text = succeeded ? "Task succeeded, cool!" : "Task failed, yikes";

        StartCoroutine(TaskEndedDelay(3.0f));
    }
    
    public void TaskFailure()
    {
        //TODO: If a task is failed, skip to next sequence of character conversation. Do not allow question answer.
        
    }

    public void TaskSuccess()
    {
        //TODO: If a task is succeeded, allow question answer. 
    }
    
}
