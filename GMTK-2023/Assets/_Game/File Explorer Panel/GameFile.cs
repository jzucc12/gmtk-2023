using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameFile : MonoBehaviour
{
    [SerializeField] private ActionType myType;
    [SerializeField] private string fileName;
    [SerializeField] private TextMeshProUGUI fileText;
    public int damageToEnemy;
    public int hpRestore;
    public int mpRestore;
    public static event Action<GameFile> FileSelected;
    private Button button;


    private void OnValidate()
    {
        fileText.text = fileName;
    }

    private void Awake()
    {
        button = GetComponent<Button>();
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
        FileSelected?.Invoke(this);
    }

    public string UseText()
    {
        string output = $"Selected file {fileName}.";
        if(damageToEnemy > 0)
        {
            output += $" Dealt {damageToEnemy} damage to your foe.";
        }
        if(hpRestore > 0)
        {
            output += $" Restored {hpRestore} HP.";
        }
        if(mpRestore > 0)
        {
            output += $" Restored {mpRestore} MP.";
        }
        return output;
    }

    public string GetFileName()
    {
        return fileName;
    }

    public ActionType GetActionType()
    {
        return myType;
    }
}
