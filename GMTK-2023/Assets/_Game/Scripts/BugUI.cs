using UnityEngine;
using UnityEngine.UI;

public class BugUI : MonoBehaviour
{
    [SerializeField] private GameConsole console;
    [SerializeField] private Image[] icons;


    private void Start()
    {
        UpdateBugCount(0);
    }

    private void OnEnable()
    {
        console.NewBug += UpdateBugCount;
    }

    private void OnDisable()
    {
        console.NewBug -= UpdateBugCount;
    }

    private void UpdateBugCount(int newBugs)
    {
        for(int ii = 0; ii < icons.Length; ii++)
        {
            icons[ii].enabled = ii < newBugs;
        }
    }
}
