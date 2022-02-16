using System.Collections.Generic;

[System.Serializable]
public class MonsterSet
{
    public MonsterBlueprint monsterType;
    public int count;
    public float initialDelay;
    public float delayBetweenSpawns;
}

[System.Serializable]
public class Wave
{
    public List<MonsterSet> monsters = new List<MonsterSet>();
}
