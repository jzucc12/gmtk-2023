[System.Serializable]
public struct ActionStruct
{
    public string enemyAttackName;
    public int enemyDamage;
    public ActionType playerAction;


    public string PlayerActionText()
    {
        return $"Choose a <color=#00ffffff>[{playerAction.ToString()}]</color>";
    }

    public string EnemyActionText()
    {
        return $"{enemyAttackName} <color=#FF7A7A>[Enemy will deal {enemyDamage} damage]</color>";
    }
}