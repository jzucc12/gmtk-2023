using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConsole : MonoBehaviour
{
    [SerializeField] private int maxBugs;
    [SerializeField] private float turnTime;
    [SerializeField] private List<ActionQueue> actions = new List<ActionQueue>();
    private int currentBugs = 0;
    private int combatIndex = 0;
    private int actionIndex = 0;
    private ActionQueue currentQueue => actions[combatIndex];
    private ActionStruct currentAction => currentQueue.GetAction(actionIndex);
    public event Action<int> NewBug;
    public event Action<ActionStruct> NewAction;
    public event Action<ActionStruct, GameFile> ActionSelected;
    public event Action<float> TimerTicked;
    public event Action GameCrash;
    public bool stop = false;


    private void OnEnable()
    {
        SubmitButton.Submitted += ActionChosen;
    }

    private void OnDisable()
    {
        SubmitButton.Submitted -= ActionChosen;
    }

    public void StartFight(int combatIndex)
    {
        stop = false;
        currentBugs = 0;
        NewBug?.Invoke(currentBugs);
        this.combatIndex = combatIndex - 1;
        actionIndex = 0;
        SetUpAction();
    }

    private void ActionChosen(GameFile file)
    {
        StopAllCoroutines();

        //Bug check
        if(file == null || file.GetActionType() != currentAction.playerAction)
        {
            currentBugs++;
            NewBug?.Invoke(currentBugs);
        }

        if(currentBugs >= maxBugs)
        {
            GameCrash?.Invoke();
            return;
        }

        //Perform action
        ActionSelected?.Invoke(currentAction, file);

        //Increment action
        actionIndex++;
        if(actionIndex >= currentQueue.actions.Count)
        {
            actionIndex = 0;
        }
        StartCoroutine(WaitForNextTurn());
    }

    private IEnumerator WaitForNextTurn()
    {
        if(!stop)
        {
            yield return new WaitForSeconds(2);
            SetUpAction();
        }
    }

    private void SetUpAction()
    {
        NewAction?.Invoke(currentAction);
        StartCoroutine(TurnTimer());
    }

    private IEnumerator TurnTimer()
    {
        float currentTime = 0;
        while(currentTime <= turnTime)
        {
            TimerTicked?.Invoke(currentTime/turnTime);
            yield return null;
            currentTime += Time.deltaTime;
        }
        ActionChosen(null);
    }
}

[Serializable]
public struct ActionQueue
{
    public List<ActionStruct> actions;

    public ActionStruct GetAction(int index)
    {
        return actions[index];
    }
}