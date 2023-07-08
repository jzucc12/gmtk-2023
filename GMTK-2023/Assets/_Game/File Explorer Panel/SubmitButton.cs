using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubmitButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI selectionText;
    [SerializeField] private Button myButton;
    private string noSelection = "Nothing Selected";
    private string selectedPrefix = "Use:";
    private GameFile selectedFile = null;
    public static event Action<GameFile> Submitted;
    private GameFolder activeFolder;


    private void Start()
    {
        NewFile(null);
    }

    private void OnEnable()
    {
        GameFile.FileSelected += NewFile;
        GameFolder.ChangeFolder += FolderChanged;
    }

    private void OnDisable()
    {
        GameFile.FileSelected -= NewFile;
        GameFolder.ChangeFolder += FolderChanged;
    }

    public void UseSelected()
    {
        if(selectedFile == null) return;
        Submitted?.Invoke(selectedFile);
        FolderChanged(null);
    }

    public void FolderChanged(GameFolder folder)
    {
        // if(selectedFile == null)
        // {
        //     return;
        // }
        if(folder != activeFolder)
        {
            activeFolder = folder;
            NewFile(null);
        }
    }

    private void NewFile(GameFile file)
    {
        selectedFile = file;
        if(file == null)
        {
            myButton.interactable = false;
            selectionText.text = noSelection;
        }
        else
        {
            myButton.interactable = true;
            selectionText.text = $"{selectedPrefix} {file.GetFileName()}";
        }
    }
}