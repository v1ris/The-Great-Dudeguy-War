using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    private int spawnWaitTime;
    public static int SpawnIndex;
    
    public static int WaveIndex;
    public static bool SpawningActive;
    
    private List<List<EnemyData>> enemyList;
    public static int DeadEnemies;
    [SerializeField] private EnemyObj enemyPrefab;
    
    [Serializable]
    public struct WaveData
    {
        [SerializeField] public EnemyData[] waveEnemies;
    }

    [Serializable]
    public struct LevelData
    {

        [SerializeField] public List<WaveData> waves;
    }
    [SerializeField] private List<LevelData> levels;
    [SerializeField] private GameManager gameManager;

    void Start()
    {
        WaveIndex = -1;
        DeadEnemies = 0;
        SpawningActive = false;
        enemyList = new List<List<EnemyData>>();
    }
    
    public void LoadWaves(int levelIndex)
    {
        enemyList.Clear();
        if (levels.Count > levelIndex-1 && levelIndex-1 > -1)
        {
            for (int i = 0; i < levels[levelIndex-1].waves.Count; i++)
            {
                List<EnemyData> currentWave = new List<EnemyData>();
                foreach (var enemyData in levels[levelIndex-1].waves[i].waveEnemies)
                {
                    currentWave.Add(enemyData);
                }
                enemyList.Add(currentWave);
            }
        }
    }

    // Enemy Spawning
    void FixedUpdate()
    {
        if (SpawningActive)
        {
            // spawn until wave is complete
            spawnWaitTime += GameManager.GameSpeed;
            if (spawnWaitTime >= 50)
            {
                EnemyObj enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
                enemy.LoadData(enemyList[WaveIndex][SpawnIndex]);
                spawnWaitTime = 0;
                SpawnIndex++;
                if (SpawnIndex > enemyList[WaveIndex].Count - 1)
                {
                    spawnWaitTime = 0;
                    SpawnIndex = 0;
                    SpawningActive = false;
                }
            }
        }
        else if (gameManager.gameState != GameManager.GameState.WaitingToStart) // check to see if each enemy in wavelist is dead
        {
            if (DeadEnemies == enemyList[WaveIndex].Count)
            {
                DeadEnemies = 0;
                gameManager.gameState = GameManager.GameState.WaitingToStart;
                BattleUI.WaveButton.style.backgroundImage = new StyleBackground(Background.FromTexture2D(BattleUI.WaveStartSprite));
                // check if it's the final wave
                if (WaveIndex == enemyList.Count - 1)
                {
                    GameManager.EndLevel();
                }
            }
        }
    }

    public static void Reset()
    {
        WaveIndex = 0;
        SpawnIndex = 0;
        DeadEnemies = 0;
        SpawningActive = false;
    }
}
