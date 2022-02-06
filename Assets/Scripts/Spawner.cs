using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;

    [SerializeField]
    Transform[] waypoints;

    [SerializeField]
    Wave[] waves;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        if (waves.Length > 0)
        {
            StartSpawning(waves[0]);
        }
    }

    void StartSpawning(Wave waveToSpawn)
    {
        for (int i = 0; i < waveToSpawn.monsters.Count; i++)
        {
            MonsterSet monsterSet = waveToSpawn.monsters[i];
            StartCoroutine(SpawnMonsterSet(monsterSet));
        }
    }

    IEnumerator SpawnMonsterSet(MonsterSet monsterSet)
    {
        yield return new WaitForSeconds(monsterSet.initialDelay);

        for (int i = 0; i < monsterSet.count; i++)
        {
            SpawnMonster(monsterSet.monsterType);
            yield return new WaitForSeconds(monsterSet.delayBetweenSpawns);
        }
    }

    void SpawnMonster(Monster monster)
    {
        Monster spawnedMonster = Instantiate(monster, transform.position, Quaternion.Euler(0, 0, 0));
        spawnedMonster.setWaypoints(waypoints);
    }
}

[System.Serializable]
public class MonsterSet
{
    public Monster monsterType;
    public int count;
    public float initialDelay;
    public float delayBetweenSpawns;
}

[System.Serializable]
public class Wave
{
    public List<MonsterSet> monsters = new List<MonsterSet>();
}