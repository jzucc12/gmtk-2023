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
            selectedText.text = "No action selected. Turn wasted.";
        }
    }

    public void SubmittedText(ActionStruct action, GameFile file, Weapon weapon, bool enoughMP, bool correctWeapon, bool wrongActionType)
    {
        string output = "";
        if(!correctWeapon)
        {
            output = $"Wrong weapon equipped (<color=#00ffffff>{weapon}</color> instead of <color=#00ffffff>{file.GetWeapon()}</color>). Turn lost.";
        }
        else if(!enoughMP)
        {
            output = $"Not enough MP (Needed <color=#00ffffff>{-file.mpRestore}</color>). Turn lost.";
        }
        else
        {
            output = file.UseText();
        }
        if(wrongActionType)
        {
            output += $"\nWrong file type chosen (<color=#00ffffff>[{file.GetActionType()}]</color> instead of <color=#00ffffff>[{action.playerAction}]</color>). Bug added.";
        }
        selectedText.text = output;
    }
}
