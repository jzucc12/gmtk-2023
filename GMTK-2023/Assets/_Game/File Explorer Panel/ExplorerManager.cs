using UnityEngine;

public class ExplorerManager : MonoBehaviour
{
    [SerializeField] GameConsole console;
    [SerializeField] private GameObject explorerBlockCanvas;
    [SerializeField] private FilesUI files;
    [SerializeField] private GameObject[] folderStructures;

    
    private void Start()
    {
        Block(null);
        NewFolders(0);
    }

    private void OnEnable()
    {
        SubmitButton.Submitted += Block;
        console.NewActionLate += Unblock;
    }

    private void OnDisable()
    {
        SubmitButton.Submitted -= Block;
        console.NewActionLate -= Unblock;
    }

    public void NewFolders(int newIndex)
    {
        for(int ii = 0; ii < folderStructures.Length; ii++)
        {
            folderStructures[ii].SetActive(ii == newIndex - 1);
        }
    }

    private void Block(GameFile file)
    {
        explorerBlockCanvas.SetActive(true);
        files.UpdateFolder(null);
    }

    private void Unblock()
    {
        explorerBlockCanvas.SetActive(false);
    }
}