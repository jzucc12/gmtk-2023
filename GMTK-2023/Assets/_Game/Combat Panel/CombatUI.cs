using TMPro;
using UnityEngine;

public class CombatUI : MonoBehaviour
{
    [SerializeField] private CombatManager combat;
    [SerializeField] private TextMeshProUGUI hpPlayerDisplay;
    [SerializeField] private TextMeshProUGUI mpPlayerDisplay;
    [SerializeField] private TextMeshProUGUI hpEnemyDisplay;


    private void Start()
    {
        UpdateUI();
    }

    private void OnEnable()
    {
        combat.TurnTaken += UpdateUI;
    }

    private void OnDisable()
    {
        combat.TurnTaken -= UpdateUI;
    }

    private void UpdateUI()
    {
        hpPlayerDisplay.text = $"HP: {combat.currentPlayerHP}/{combat.maxPlayerHP}";
        mpPlayerDisplay.text = $"MP: {combat.currentPlayerMP}/{combat.maxPlayerMP}";
        hpEnemyDisplay.text = $"Enemy: {combat.currentEnemyHP}/{combat.maxEnemyHP}";
    }
}