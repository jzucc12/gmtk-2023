using TMPro;
using UnityEngine;

public class ActionUI : MonoBehaviour
{
    [SerializeField] private GameConsole console;
    [SerializeField] TextMeshProUGUI enemyText;
    [SerializeField] TextMeshProUGUI playerText;
    [SerializeField] TextMeshProUGUI selectedText;


    private void OnEnable()
    {
        console.NewAction += SetUpAction;
        SubmitButton.Submitted += FileSubmitted;
    }

    private void OnDisable()
    {
        console.NewAction -= SetUpAction;
        SubmitButton.Submitted -= FileSubmitted;
    }

    private void SetUpAction(ActionStruct currentAction)
    {
        enemyText.text = currentAction.EnemyActionText();
        playerText.text = currentAction.PlayerActionText();
        selectedText.text = "";
    }

    private void FileSubmitted(GameFile file)
    {
        selectedText.text = file.UseText();
    }
}
