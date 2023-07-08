using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameConsole : MonoBehaviour
{
    [SerializeField] private List<ActionStruct> actions = new List<ActionStruct>();

    [SerializeField] private int maxBugs;
    private int currentBugs = 0;
    private int actionIndex = 0;
    private ActionStruct currentAction => actions[actionIndex];
    public event Action<int> NewBug;
    public event Action<ActionStruct> NewAction;


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
        if(file.GetActionType() != currentAction.playerAction)
        {
            currentBugs++;
            NewBug?.Invoke(currentBugs);
        }

        if(currentBugs >= maxBugs)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        //Do enemy action
        //Do file action
        Debug.Log($"Enemy attacks for {currentAction.enemyDamage}");

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
    }
}
