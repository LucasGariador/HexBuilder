[System.Serializable]
public class StatsPlayer
{
    public int constitution = 5;
    public int dexterity = 5;
    public int mental = 5;

    public int GetStat(StatType type)
    {
        return type switch
        {
            StatType.Constitution => constitution,
            StatType.Dexterity => dexterity,
            StatType.Mental => mental,
            _ => 0
        };
    }
}

public enum StatType
{
    Constitution,
    Dexterity,
    Mental
}
