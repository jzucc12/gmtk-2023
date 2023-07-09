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
    [HideInInspector] public Weapon myWeapon = Weapon.Sword;
    private int combatIndex = 0;
    public event Action<float> TurnTaken;
    public event Action PlayerLose;
    public event Action PlayerWin;

    [Header("Combat UI")]
    [SerializeField] private Animator playerAnimator;
    private Animator enemyAnimator => currentCombat.enemyLocation.GetComponent<Animator>();
    [SerializeField] private Transform dungeonDoor;
    [SerializeField] private Transform teleportDestination;
    [SerializeField] private Image blockOutScreen;
    [SerializeField] private RectTransform background;
    [SerializeField] private float scrollSpeed = 1;
    [SerializeField] private float fadeTime = 1;
    public event Action ScrollFinished;
    public event Action<ActionStruct, GameFile, Weapon, bool, bool, bool> CombatText;
    private float offset;
    private SFXPlayer sfx;

    [Header("Combat Audio")]
    [SerializeField] private AudioClip playerDie;
    [SerializeField] private AudioClip enemyDie;
    [SerializeField] private AudioClip badChoiceClip;
    [SerializeField] private AudioClip doorClip;


    #region //Monobehaviour
    private void Awake()
    {
        sfx = FindAnyObjectByType<SFXPlayer>();
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
        TurnTaken?.Invoke(currentPlayerHP);
    }

    private void DoAction(ActionStruct action, GameFile file)
    {
        float playerHPAfterTheirTurn = currentPlayerHP;
        bool enoughMP = false;
        bool correctWeapon = true;
        if(file != null)
        {
            currentPlayerMP += file.mpRestore;
            bool wrongActionType = action.playerAction != file.GetActionType();

            if(file.IsEquip())
            {
                myWeapon = file.GetWeapon();
            }
            else if(file.GetWeapon() != Weapon.None && myWeapon != file.GetWeapon())
            {
                correctWeapon = false;
            }

            if(currentPlayerMP >= 0)
            {
                enoughMP = true;
            }

            if(correctWeapon && enoughMP)
            {
                currentPlayerHP += file.hpRestore;
                currentEnemyHP -= file.damageToEnemy;
                playerHPAfterTheirTurn = currentPlayerHP;
            }

            bool attackedForEnemy = file.hpRestore < 0 && file.damageToEnemy == 0;
            if(!correctWeapon || !enoughMP || !attackedForEnemy)
            {
                currentPlayerHP -= action.enemyDamage;
            }

            currentPlayerHP = Mathf.Clamp(currentPlayerHP, 0, maxPlayerHP);
            currentPlayerMP = Mathf.Clamp(currentPlayerMP, 0, maxPlayerMP);
            currentEnemyHP = Mathf.Clamp(currentEnemyHP, 0, maxEnemyHP);
            CombatText?.Invoke(action, file, myWeapon, enoughMP, correctWeapon, wrongActionType);
        }
        else
        {
            currentPlayerHP -= action.enemyDamage;
            currentPlayerHP = Mathf.Clamp(currentPlayerHP, 0, maxPlayerHP);
        }
        StartCoroutine(AttackRoutine(file, enoughMP, correctWeapon, playerHPAfterTheirTurn));
    }

    private IEnumerator AttackRoutine(GameFile file, bool enoughMP, bool correctWeapon, float playerHPMid)
    {
        bool didSomething = file.hpRestore > 0 || file.mpRestore > 0 || file.damageToEnemy > 0 || file.GetActionType() == ActionType.Equip;
        if(file != null && enoughMP && correctWeapon)
        {
            if(didSomething)
            {
                if(file.GetActionType() == ActionType.Item || file.GetActionType() == ActionType.Equip)
                {
                    playerAnimator.SetTrigger("Item");
                }
                else
                {
                    playerAnimator.SetTrigger(myWeapon.ToString());
                }
            }
            yield return new WaitForSeconds(0.2f);
            if(file.GetClip() != null)
            {
                sfx.PlaySound(file.GetClip());
            }
            if(file.damageToEnemy > 0)
            {
                enemyAnimator.SetTrigger("Damage");
            }
            TurnTaken?.Invoke(playerHPMid);

            yield return new WaitForSeconds(0.75f);
            if(currentEnemyHP <= 0)
            {
                PlayerWin?.Invoke();
                sfx.PlaySound(enemyDie);
                yield break;
            }
        }
        else
        {
            sfx.PlaySound(badChoiceClip);
        }

        enemyAnimator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.3f);
        sfx.PlaySound(currentCombat.enemyName);
        playerAnimator.SetTrigger("Damage");
        TurnTaken?.Invoke(currentPlayerHP);
        yield return new WaitForSeconds(0.75f);
        if(currentPlayerHP <= 0)
        {
            PlayerLose?.Invoke();
            sfx.PlaySound(playerDie);
        }
        else
        {
            console.ContinueAfterAttack();
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
        float buffer = combatIndex == 2 ? 200 : 100;
        yield return ScrollToPoint(currentCombat.enemyLocation.localPosition.x, buffer);
        ScrollFinished?.Invoke();
        playerAnimator.SetBool("walking", false);
    }

    private IEnumerator EnterDungeon()
    {
        float xShift = playerAnimator.transform.localPosition.x + 41; //Empirical
        yield return ScrollToPoint(dungeonDoor.localPosition.x, 0);
        playerAnimator.SetBool("walking", false);

        sfx.PlaySound(doorClip);
        blockOutScreen.DOFade(1, fadeTime);
        yield return new WaitForSeconds(fadeTime);

        float moveTo = teleportDestination.localPosition.x;
        float yShift = 44; //Empirical
        playerAnimator.transform.parent.localPosition += new Vector3(0, -yShift, 0);
        background.anchoredPosition = new Vector2(-moveTo, background.anchoredPosition.y);

        blockOutScreen.DOFade(0, fadeTime);
        FindObjectOfType<MusicPlayer>().PlayBossSong();
        yield return new WaitForSeconds(fadeTime);
        playerAnimator.SetBool("walking", true);
    }

    private IEnumerator ScrollToPoint(float destination, float buffer)
    {
        float start = background.anchoredPosition.x;
        float moved = -start;
        float target;
        if(buffer == 0)
        {
            target = 4954; //Empirical for dungeon door because I hate everything.
        }
        else
        {
            target = destination - background.parent.GetComponent<RectTransform>().rect.width + buffer;
        }
        while(moved != target)
        {
            moved = Mathf.MoveTowards(moved, target, scrollSpeed * Time.deltaTime);
            background.anchoredPosition = new Vector2(-moved, background.anchoredPosition.y);
            yield return null;
        }
    }

    public void Victory()
    {
        if(combatIndex == 2)
        {
            FindObjectOfType<MusicPlayer>().PlayMainSong();
        }
        currentCombat.enemyLocation.DOScale(Vector3.zero, 0.75f);
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