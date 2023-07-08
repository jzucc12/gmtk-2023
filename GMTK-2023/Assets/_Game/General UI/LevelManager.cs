using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameConsole console;
    [SerializeField] private CombatManager combat;
    [SerializeField] private GameObject winFightScreen;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject defeatScreen;
    [SerializeField] private GameObject gameCrashScreen;
    [SerializeField] private int maxCombats;
    private int combatIndex = 1;


    private void Start()
    {
        StartFight();
    }

    private void OnEnable()
    {
        console.GameCrash += GameCrash;
        combat.PlayerLose += GameOver;
        combat.PlayerWin += WinFight;
    }

    private void OnDisable()
    {
        console.GameCrash -= GameCrash;
        combat.PlayerLose -= GameOver;
        combat.PlayerWin -= WinFight;
    }

    public void StartFight()
    {
        console.StartFight(combatIndex);
        combat.SetUp(combatIndex);
        winFightScreen.SetActive(false);
        victoryScreen.SetActive(false);
        defeatScreen.SetActive(false);
        gameCrashScreen.SetActive(false);
    }

    private void WinFight()
    {
        console.stop = true;
        combatIndex++;
        if(combatIndex > maxCombats)
        {
            victoryScreen.SetActive(true);
        }
        else
        {
            winFightScreen.SetActive(true);
        }
    }

    private void GameOver()
    {
        console.stop = true;
        defeatScreen.SetActive(true);
    }

    private void GameCrash()
    {
        console.stop = true;
        gameCrashScreen.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
