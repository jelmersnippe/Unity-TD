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

    [SerializeField] Monster monsterPrefab;
    Transform monsterHolder;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        monsterHolder = Instantiate(new GameObject().transform, transform);
        monsterHolder.name = "Monsters";

        Transform waypointsHolder = transform.Find("Waypoints");

        if (waypointsHolder == null)
        {
            Debug.LogError("No Waypoints object inside of Spawner");
            return;
        }

        waypoints = new Transform[waypointsHolder.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = waypointsHolder.GetChild(i);
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

    void SpawnMonster(MonsterBlueprint monster)
    {
        Monster spawnedMonster = Instantiate(monsterPrefab, transform.position, Quaternion.Euler(0, 0, 0), monsterHolder);
        spawnedMonster.Setup(monster.sprite, monster.health, monster.speed, monster.damage, monster.currencyToDrop, waypoints);
    }

    public void ReduceCurrentMonstersAlive()
    {
        currentWaveEnemiesAlive--;
    }
}

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