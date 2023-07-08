using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [Header("Combat mechanics")]
    [SerializeField] private GameConsole console;
    public int maxPlayerHP;
    public int maxPlayerMP;
    [SerializeField] private List<CombatStruct> combats;
    private CombatStruct currentCombat => combats[combatIndex];
    public int maxEnemyHP => currentCombat.enemyHP;
    [HideInInspector] public int currentPlayerHP;
    [HideInInspector] public int currentPlayerMP;
    [HideInInspector] public int currentEnemyHP;
    private int combatIndex = 0;
    public event Action TurnTaken;
    public event Action PlayerLose;
    public event Action PlayerWin;

    [Header("Combat UI")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private RectTransform background;
    [SerializeField] private float scrollSpeed = 5;
    public event Action ScrollFinished;
    private float offset;


    #region //Monobehaviour
    private void Awake()
    {
        SetUp();
    }

    private void OnEnable()
    {
        console.ActionSelected += DoAction;
    }

    private void OnDisable()
    {
        console.ActionSelected -= DoAction;
    }
    #endregion

    #region //Fight
    public void SetUp()
    {
        currentPlayerHP = maxPlayerHP;
        currentPlayerMP = maxPlayerMP;
        currentEnemyHP = maxEnemyHP;
        TurnTaken?.Invoke();
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
    #endregion

    #region //UI
    public void Scroll(int combatIndex)
    {
        this.combatIndex = combatIndex - 1;
        StartCoroutine(ScrollRoutine());
        playerAnimator.SetBool("walking", true);
    }

    private IEnumerator ScrollRoutine()
    {
        float start = background.anchoredPosition.x;
        float moved = -start;
        float buffer = 100;
        float target = currentCombat.enemyLocation.localPosition.x - background.parent.GetComponent<RectTransform>().rect.width + buffer;
        while(moved != target)
        {
            moved = Mathf.MoveTowards(moved, target, scrollSpeed);
            background.anchoredPosition = new Vector2(-moved, background.localPosition.y);
            yield return null;
        }
        ScrollFinished?.Invoke();
        playerAnimator.SetBool("walking", false);
    }

    public void Victory()
    {
        currentCombat.enemyLocation.gameObject.SetActive(false);
    }
    #endregion
}

[Serializable]
public struct CombatStruct
{
    public int enemyHP;
    public Transform enemyLocation;
}