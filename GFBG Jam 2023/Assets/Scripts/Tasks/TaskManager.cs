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

    public bool succeeded;

    private InMemoryVariableStorage _variableStorage;
    private DialogueRunner runner;

    void Start()
    {
        environmentController = FindObjectOfType<EnvironmentChange>();
        _variableStorage = FindObjectOfType<InMemoryVariableStorage>();
        runner = FindObjectOfType<DialogueRunner>();
        runner.AddFunction<bool>("return_success", ReturnSuccess);
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
            UpdateTaskUI();
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
        
        TaTask thisTask = allTasks[Random.Range(0, allTasks.Count)];

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
            if (!RoomMatch(currentTask.startingArea)) return;
            //We are in the right room to pick an item up. 
            haveItem = true;
            UpdateTaskUI();
        }
        else
        {
            if (!RoomMatch(currentTask.endingArea)) return;
            haveItem = false;
            succeeded = true;
            StopAllCoroutines();
            EndTask();
        }
    }

    //Updates the UI text based on where the player is in the environment.
    public void UpdateTaskUI()
    {
        if (currentTask == null) return;
        //We have an active task. So let's break it down.
        if (!haveItem)
        {
            //We need to get to the starting area.
            taskText.text = !RoomMatch(currentTask.startingArea) ? currentTask.startingText : "You are in the right room! Press space!";
        }
        else
        {
            taskText.text = !RoomMatch(currentTask.endingArea) ? currentTask.endingText : "Press space to drop the item here!";
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

    private bool ReturnSuccess()
    {
        return succeeded;
    }

    public bool RoomMatch(string room)
    {
        return (environmentController.currentEnvironment == room);
    }
}
