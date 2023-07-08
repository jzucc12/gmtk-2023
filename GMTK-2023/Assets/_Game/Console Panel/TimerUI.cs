using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private GameConsole console;
    [SerializeField] private Image percentageUI;


    private void Start()
    {
        UpdateUI(0);
    }

    private void OnEnable()
    {
        console.TimerTicked += UpdateUI;
    }

    private void OnDisable()
    {
        console.TimerTicked -= UpdateUI;
    }

    private void UpdateUI(float percentage)
    {
        percentageUI.fillAmount = percentage;
    }
}
