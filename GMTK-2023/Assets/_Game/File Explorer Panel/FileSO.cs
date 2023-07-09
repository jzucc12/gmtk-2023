using UnityEngine;


[CreateAssetMenu(fileName = "FileSO", menuName = "GMTK-2023/FileSO", order = 0)]
public class FileSO : ScriptableObject 
{
    public ActionType myType;
    public Weapon weapon;
    public int damageToEnemy;
    public int hpRestore;
    public int mpRestore;
    public string outputText;
    public AudioClip sfx;
}