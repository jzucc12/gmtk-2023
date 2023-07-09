using TMPro;
using UnityEngine;

public class CombatUI : MonoBehaviour
{
    [SerializeField] private CombatManager combat;
    [SerializeField] private TextMeshProUGUI hpPlayerDisplay;
    [SerializeField] private TextMeshProUGUI mpPlayerDisplay;
    [SerializeField] private TextMeshProUGUI hpEnemyDisplay;
    [SerializeField] private TextMeshProUGUI weaponDisplay;


    private void Start()
    {
        UpdateUI(combat.currentPlayerHP);
    }

    private void OnEnable()
    {
        combat.TurnTaken += UpdateUI;
    }

    private void OnDisable()
    {
        combat.TurnTaken -= UpdateUI;
    }

    private void UpdateUI(float playerHP)
    {
        weaponDisplay.text = $"Weapon:\n{combat.myWeapon}";
        hpPlayerDisplay.text = $"HP: {playerHP}/{combat.maxPlayerHP}";
        mpPlayerDisplay.text = $"MP: {combat.currentPlayerMP}/{combat.maxPlayerMP}";
        hpEnemyDisplay.text = $"{combat.enemyName}: {combat.currentEnemyHP}/{combat.maxEnemyHP}";
    }
}