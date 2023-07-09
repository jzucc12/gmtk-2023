using TMPro;
using UnityEngine;

public class ActionUI : MonoBehaviour
{
    [SerializeField] private GameConsole console;
    [SerializeField] private CombatManager combat;
    [SerializeField] TextMeshProUGUI enemyText;
    [SerializeField] TextMeshProUGUI playerText;
    [SerializeField] TextMeshProUGUI selectedText;


    private void OnEnable()
    {
        console.NewAction += SetUpAction;
        console.ActionSelected += FileSubmitted;
        combat.CombatText += SubmittedText;
    }

    private void OnDisable()
    {
        console.NewAction -= SetUpAction;
        console.ActionSelected -= FileSubmitted;
        combat.CombatText -= SubmittedText;
    }

    private void SetUpAction(ActionStruct currentAction)
    {
        enemyText.text = currentAction.EnemyActionText();
        playerText.text = currentAction.PlayerActionText();
        selectedText.text = "";
    }

    private void FileSubmitted(ActionStruct action, GameFile file)
    {
        if(file == null)
        {
            selectedText.text = "No action selected. Turn Skipped.";
        }
    }

    public void SubmittedText(GameFile file, bool enoughMP, bool correctWeapon)
    {
        string output = "";
        if(!correctWeapon)
        {
            output = "You don't have the right weapon equipped. No effect";
        }
        else if(!enoughMP)
        {
            output = "You didn't have enough MP. No effect";
        }
        else
        {
            output = file.UseText();
        }
        selectedText.text = output;
    }
}
