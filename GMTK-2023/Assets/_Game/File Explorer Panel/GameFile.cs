using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameFile : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fileText;
    [SerializeField] private Button button;
    public int damageToEnemy => fileSO.damageToEnemy;
    public int hpRestore => fileSO.hpRestore;
    public int mpRestore => fileSO.mpRestore;
    public static event Action<GameFile> FileSelected;
    private FileSO fileSO;


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
        string output = $"{fileSO.outputText}";
        return output;
    }

    public void SetFileSO(FileSO newSO)
    {
        fileSO = newSO;
        fileText.text = GetFileName();
    }

    public string GetFileName()
    {
        return fileSO.name;
    }

    public ActionType GetActionType()
    {
        return fileSO.myType;
    }

    public Weapon GetWeapon()
    {
        return fileSO.weapon;
    }

    public bool IsEquip()
    {
        return fileSO.myType == ActionType.Equip;
    }
}
