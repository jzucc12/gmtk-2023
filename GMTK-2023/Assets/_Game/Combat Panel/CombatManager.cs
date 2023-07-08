using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private GameConsole console;
    public int maxPlayerHP;
    public int maxPlayerMP;
    [SerializeField] private List<int> enemyHPs;
    public int maxEnemyHP => enemyHPs[combatIndex];
    [HideInInspector] public int currentPlayerHP;
    [HideInInspector] public int currentPlayerMP;
    [HideInInspector] public int currentEnemyHP;
    private int combatIndex;
    public event Action TurnTaken;
    public event Action PlayerLose;
    public event Action PlayerWin;


    private void Awake()
    {
        SetUp(1);
    }

    public void SetUp(int combatIndex)
    {
        this.combatIndex = combatIndex - 1;
        currentPlayerHP = maxPlayerHP;
        currentPlayerMP = maxPlayerMP;
        currentEnemyHP = maxEnemyHP;
        TurnTaken?.Invoke();
    }

    private void OnEnable()
    {
        console.ActionSelected += DoAction;
    }

    private void OnDisable()
    {
        console.ActionSelected -= DoAction;
    }

    private void DoAction(ActionStruct action, GameFile file)
    {
        currentPlayerHP += file.hpRestore - action.enemyDamage;
        currentPlayerMP += file.mpRestore;
        currentEnemyHP -= file.damageToEnemy;

        currentPlayerHP = Mathf.Clamp(currentPlayerHP, 0, maxPlayerHP);
        currentPlayerMP = Mathf.Clamp(currentPlayerMP, 0, maxPlayerMP);
        currentEnemyHP = Mathf.Clamp(currentEnemyHP, 0, maxEnemyHP);

        TurnTaken?.Invoke();
        if(currentPlayerHP == 0)
        {
            PlayerLose?.Invoke();
        }
        else if(currentEnemyHP == 0)
        {
            PlayerWin?.Invoke();
        }

    }
}