using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubmitButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI selectionText;
    [SerializeField] private Button myButton;
    private string noSelection = "Nothing Selected";
    private string selectedPrefix = "Use:";
    private GameFile fileSelected = null;
    public static event Action<GameFile> Submitted;


    private void Start()
    {
        NewFile(null);
    }

    private void OnEnable()
    {
        GameFile.FileSelected += NewFile;
    }

    private void OnDisable()
    {
        GameFile.FileSelected -= NewFile;
    }

    private void NewFile(GameFile file)
    {
        fileSelected = file;
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

    public void UseSelected()
    {
        if(fileSelected == null) return;
        Submitted?.Invoke(fileSelected);
    }
}