using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameConsole console;
    [SerializeField] private CombatManager combat;


    private void OnEnable()
    {
        console.GameCrash += GameOver;
        combat.PlayerLose += GameOver;
        combat.PlayerWin += GameOver;
    }

    private void OnDisable()
    {
        console.GameCrash -= GameOver;
        combat.PlayerLose -= GameOver;
        combat.PlayerWin -= GameOver;
    }

    private void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
