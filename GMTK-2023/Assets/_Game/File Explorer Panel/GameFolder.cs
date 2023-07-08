using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameFolder : MonoBehaviour
{
    [SerializeField] private string folderName;
    [SerializeField] private TextMeshProUGUI fileText;
    [SerializeField] private Button button;
    [SerializeField] private List<FileSO> files;
    private bool opened = false;
    public static event Action<GameFolder> ChangeFolder;


    private void OnValidate()
    {
        fileText.text = folderName;
    }

    private void OnEnable()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnClick);
    }
    
    private void OnClick()
    {
        ChangeFolder?.Invoke(this);
    }

    public string GetName()
    {
        return folderName;
    }

    public List<FileSO> GetFiles()
    {
        return files;
    }
}
