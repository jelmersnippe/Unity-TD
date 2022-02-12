using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;

    Transform[] waypoints;

    [SerializeField] Wave[] waves;


    [SerializeField] int currentWaveIndex = 0;
    int currentWaveEnemiesAlive = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        waypoints = new Transform[transform.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = transform.GetChild(i);
        }
    }

    private void Update()
    {
        if (currentWaveEnemiesAlive > 0)
        {
            return;
        }

        if (currentWaveIndex >= waves.Length)
        {
            GameManager.instance.WinGame();
            return;
        }

        SpawnWave(waves[currentWaveIndex]);
    }

    void SpawnWave(Wave wave)
    {
        for (int i = 0; i < wave.monsters.Count; i++)
        {
            MonsterSet monsterSet = wave.monsters[i];
            currentWaveEnemiesAlive += monsterSet.count;
            StartCoroutine(SpawnMonsterSet(monsterSet));
        }
        currentWaveIndex++;
        GameManager.instance.currentRound = currentWaveIndex;
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

    public void ReduceCurrentMonstersAlive()
    {
        currentWaveEnemiesAlive--;
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