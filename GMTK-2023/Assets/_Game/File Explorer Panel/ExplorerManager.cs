using System;
using UnityEngine;

public class ExplorerManager : MonoBehaviour
{
    [SerializeField] GameConsole console;
    [SerializeField] private GameObject explorerBlockCanvas;
    [SerializeField] private FilesUI files;
    [SerializeField] private GameObject[] folderStructures;

    
    private void Start()
    {
        Block(new ActionStruct(), null);
        NewFolders(0);
    }

    private void OnEnable()
    {
        console.ActionSelected += Block;
        console.NewActionLate += Unblock;
    }

    private void OnDisable()
    {
        console.ActionSelected -= Block;
        console.NewActionLate -= Unblock;
    }

    public void NewFolders(int newIndex)
    {
        for(int ii = 0; ii < folderStructures.Length; ii++)
        {
            folderStructures[ii].SetActive(ii == newIndex - 1);
        }
    }

    private void Block(ActionStruct _, GameFile file)
    {
        explorerBlockCanvas.SetActive(true);
        files.UpdateFolder(null);
    }

    private void Unblock()
    {
        explorerBlockCanvas.SetActive(false);
    }
}