using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    [Header("Combat mechanics")]
    [SerializeField] private GameConsole console;
    public int maxPlayerHP;
    public int maxPlayerMP;
    [SerializeField] private List<CombatStruct> combats;
    private CombatStruct currentCombat => combats[combatIndex];
    public int maxEnemyHP => currentCombat.enemyHP;
    public string enemyName => currentCombat.enemyName;
    [HideInInspector] public int currentPlayerHP;
    [HideInInspector] public int currentPlayerMP;
    [HideInInspector] public int currentEnemyHP;
    private int combatIndex = 0;
    public event Action TurnTaken;
    public event Action PlayerLose;
    public event Action PlayerWin;

    [Header("Combat UI")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Transform dungeonDoor;
    [SerializeField] private Transform teleportDestination;
    [SerializeField] private Image blockOutScreen;
    [SerializeField] private RectTransform background;
    [SerializeField] private float scrollSpeed = 1;
    [SerializeField] private float fadeTime = 1;
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
        console.turnTime = currentCombat.turnTime;
        StartCoroutine(ScrollRoutine());
        playerAnimator.SetBool("walking", true);
    }

    private IEnumerator ScrollRoutine()
    {
        if(combatIndex == 2)
        {
            yield return EnterDungeon();
        }
        float buffer = 100;
        yield return ScrollToPoint(currentCombat.enemyLocation.localPosition.x, buffer);
        ScrollFinished?.Invoke();
        playerAnimator.SetBool("walking", false);
    }

    private IEnumerator EnterDungeon()
    {
        float xShift = playerAnimator.transform.localPosition.x + 41; //Empirical
        yield return ScrollToPoint(dungeonDoor.localPosition.x, background.parent.GetComponent<RectTransform>().rect.width-xShift);
        playerAnimator.SetBool("walking", false);

        //Play door sfx
        blockOutScreen.DOFade(1, fadeTime);
        yield return new WaitForSeconds(fadeTime);

        float moveTo = teleportDestination.localPosition.x;
        float yShift = 44; //Empirical
        playerAnimator.transform.localPosition += new Vector3(0, -yShift, 0);
        background.anchoredPosition = new Vector2(-moveTo, background.anchoredPosition.y);

        blockOutScreen.DOFade(0, fadeTime);
        yield return new WaitForSeconds(fadeTime);
        playerAnimator.SetBool("walking", true);
    }

    private IEnumerator ScrollToPoint(float destination, float buffer)
    {
        float start = background.anchoredPosition.x;
        float moved = -start;
        float target = destination - background.parent.GetComponent<RectTransform>().rect.width + buffer;
        while(moved != target)
        {
            moved = Mathf.MoveTowards(moved, target, scrollSpeed);
            background.anchoredPosition = new Vector2(-moved, background.anchoredPosition.y);
            yield return null;
        }
    }

    public void Victory()
    {
        currentCombat.enemyLocation.gameObject.SetActive(false);
    }

    public int CombatCount()
    {
        return combats.Count;
    }
    #endregion
}

[Serializable]
public struct CombatStruct
{
    public int enemyHP;
    public Transform enemyLocation;
    public float turnTime;
    public string enemyName;
}

//TODO
//Lower max bugs to gain hp option
//Can't use attacks if not enough MP/wrong weapon equipped
//Weapon equipped UI
//Attack animations
//Idle animations
//Put enemies in
//Music and sfx
//Update font

//Try map as sprite instead of image
//Try green tint on console log
//Add border to top box

//Main menu
//Pause with audio settings
//Credits