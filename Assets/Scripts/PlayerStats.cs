using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    public int Dexterity = 0;
    public int Constitution = 0;
    public int Psyche = 0;

    public int GetStat(SkillType type)
    {
        return type switch
        {
            SkillType.Dexterity => Dexterity,
            SkillType.Constitution => Constitution,
            SkillType.Psyche => Psyche,
            _ => 0
        };
    }
}

public enum SkillType
{
    Dexterity,
    Constitution,
    Psyche
}
