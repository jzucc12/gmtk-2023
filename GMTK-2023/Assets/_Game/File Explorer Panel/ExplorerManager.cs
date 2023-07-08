using UnityEngine;

public class ExplorerManager : MonoBehaviour
{
    [SerializeField] GameConsole console;
    [SerializeField] private GameObject explorerBlockCanvas;
    [SerializeField] private FilesUI files;

    
    private void OnEnable()
    {
        SubmitButton.Submitted += Block;
        console.NewAction += Unblock;
    }

    private void OnDisable()
    {
        SubmitButton.Submitted -= Block;
        console.NewAction -= Unblock;
    }

    private void Block(GameFile file)
    {
        explorerBlockCanvas.SetActive(true);
        files.UpdateFolder(null);
    }

    private void Unblock(ActionStruct _)
    {
        explorerBlockCanvas.SetActive(false);
    }
}