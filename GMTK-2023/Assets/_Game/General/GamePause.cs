using UnityEngine;

public class GamePause : MonoBehaviour
{
    private bool paused = false;
    [SerializeField] private GameObject pauseMenu;

    private void Awake()
    {
        Pause(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Pause(!paused);
        }
    }

    public void Pause(bool newPause)
    {
        paused = newPause;
        pauseMenu.SetActive(newPause);
        Time.timeScale = newPause ? 0 : 1;
    }
}