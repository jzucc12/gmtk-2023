using TMPro;
using UnityEngine;

public class ActionUI : MonoBehaviour
{
    [SerializeField] private GameConsole console;
    [SerializeField] private CombatManager combat;
    [SerializeField] TextMeshProUGUI enemyText;
    [SerializeField] TextMeshProUGUI playerText;
    [SerializeField] TextMeshProUGUI selectedText;


    private void Start()
    {
        enemyText.text = "";
        playerText.text = "";
        selectedText.text = "";
    }

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
            selectedText.text = "No action selected. Gained a bug.";
        }
    }

    public void SubmittedText(GameFile file, bool enoughMP, bool correctWeapon, bool wrongActionType)
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
        if(wrongActionType)
        {
            output += "\n Wrong file type chosen. Bug added";
        }
        selectedText.text = output;
    }
}
