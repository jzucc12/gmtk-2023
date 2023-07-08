[System.Serializable]
public struct ActionStruct
{
    public string enemyAttackName;
    public int enemyDamage;
    public ActionType playerAction;


    public string PlayerActionText()
    {
        return $"Player selected [{playerAction.ToString()}]";
    }

    public string EnemyActionText()
    {
        return $"Enemy uses {enemyAttackName} which deals {enemyDamage} damage";
    }
}