using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConsole : MonoBehaviour
{
    [SerializeField] private int maxBugs;
    [SerializeField] private float turnTime;
    [SerializeField] private List<ActionStruct> actions = new List<ActionStruct>();
    private int currentBugs = 0;
    private int actionIndex = 0;
    private ActionStruct currentAction => actions[actionIndex];
    public event Action<int> NewBug;
    public event Action<ActionStruct> NewAction;
    public event Action<ActionStruct, GameFile> ActionSelected;
    public event Action<float> TimerTicked;
    public event Action GameCrash;


    private void Start()
    {
        SetUpAction();
    }

    private void OnEnable()
    {
        SubmitButton.Submitted += ActionChosen;
    }

    private void OnDisable()
    {
        SubmitButton.Submitted -= ActionChosen;
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
            
        }

        //Perform action
        ActionSelected?.Invoke(currentAction, file);

        //Increment Action
        actionIndex++;
        if(actionIndex >= actions.Count)
        {
            actionIndex = 0;
        }
        StartCoroutine(WaitForNextTurn());
    }

    private IEnumerator WaitForNextTurn()
    {
        yield return new WaitForSeconds(2);
        SetUpAction();
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
