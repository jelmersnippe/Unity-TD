using System;
using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;

    public static event Action<int> OnWaveCleared;
    public static event Action OnLastWaveCleared;

    Transform[] waypoints;

    [SerializeField] Wave[] waves;
    [SerializeField] int currentWaveIndex = 0;
    int currentWaveEnemiesAlive = 0;

    [SerializeField] Monster monsterPrefab;
    Transform monsterHolder;

    private void OnEnable()
    {
        // Subscribe to events
        Monster.OnMonsterDied += ReduceCurrentMonstersAlive;
        Monster.OnMonsterReachedFinalWaypoint += ReduceCurrentMonstersAlive;
    }

    private void OnDisable()
    {
        // Unsubscribe from events
        Monster.OnMonsterDied -= ReduceCurrentMonstersAlive;
        Monster.OnMonsterReachedFinalWaypoint -= ReduceCurrentMonstersAlive;
    }

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
            OnLastWaveCleared?.Invoke();
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

    void ReduceCurrentMonstersAlive(Monster monster)
    {
        currentWaveEnemiesAlive--;

        if (currentWaveEnemiesAlive <= 0)
        {
            OnWaveCleared?.Invoke(currentWaveIndex);
        }
    }
}