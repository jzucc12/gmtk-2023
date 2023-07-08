using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameConsole console;
    [SerializeField] private CombatManager combat;
    [SerializeField] private ExplorerManager explorer;
    [SerializeField] private GameObject winFightScreen;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject defeatScreen;
    [SerializeField] private GameObject gameCrashScreen;
    private int combatIndex = 1;


    private void Start()
    {
        GoToNextFight();
    }

    private void OnEnable()
    {
        console.GameCrash += GameCrash;
        combat.PlayerLose += GameOver;
        combat.PlayerWin += WinFight;
        combat.ScrollFinished += StartFight;
    }

    private void OnDisable()
    {
        console.GameCrash -= GameCrash;
        combat.PlayerLose -= GameOver;
        combat.PlayerWin -= WinFight;
        combat.ScrollFinished -= StartFight;
    }

    public void GoToNextFight()
    {
        combat.Scroll(combatIndex);
        winFightScreen.SetActive(false);
        victoryScreen.SetActive(false);
        defeatScreen.SetActive(false);
        gameCrashScreen.SetActive(false);
    }

    private void StartFight()
    {
        explorer.NewFolders(combatIndex);
        console.StartFight(combatIndex);
        combat.SetUp();
    }

    private void WinFight()
    {
        combat.Victory();
        console.stop = true;
        combatIndex++;
        if(combatIndex > combat.CombatCount())
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
