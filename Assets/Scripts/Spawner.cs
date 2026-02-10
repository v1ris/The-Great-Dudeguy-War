using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    private int spawnWaitTime;
    private int spawnIndex;
    
    public static int WaveIndex;
    public static bool SpawningActive = false;
    
    private List<List<GameObject>> enemyList;
    public static int DeadEnemies;

    [SerializeField] private GameObject circleDude;
    [SerializeField] private GameObject circleDude2;
    [SerializeField] private GameObject triangleDude;
    [SerializeField] private GameObject squareDude;
    [SerializeField] private GameManager gameManager;

    void Start()
    {
        WaveIndex = -1;
        DeadEnemies = 0;
    }
    
    public void LoadWaves(int level)
    {
        if (level == 1)
        {
            enemyList  = new List<List<GameObject>>();
            enemyList.Insert(0, new List<GameObject>());
            enemyList.Insert(1, new List<GameObject>());
            enemyList.Insert(2, new List<GameObject>());
            enemyList.Insert(3, new List<GameObject>());
            enemyList.Insert(4, new List<GameObject>());

            // wave 1
            enemyList[0].Add(circleDude);
            enemyList[0].Add(circleDude);
            enemyList[0].Add(circleDude);
            enemyList[0].Add(circleDude);
            enemyList[0].Add(circleDude);
            
            // wave 2
            enemyList[1].Add(circleDude);
            enemyList[1].Add(circleDude);
            enemyList[1].Add(circleDude);
            enemyList[1].Add(circleDude);
            enemyList[1].Add(circleDude);
            enemyList[1].Add(circleDude);
            enemyList[1].Add(circleDude);
            enemyList[1].Add(circleDude);
            enemyList[1].Add(circleDude);
            enemyList[1].Add(circleDude);
            
            // wave 3
            enemyList[2].Add(circleDude);
            enemyList[2].Add(circleDude);
            enemyList[2].Add(circleDude);
            
            // wave 4
            enemyList[3].Add(circleDude);
            enemyList[3].Add(circleDude);
            enemyList[3].Add(circleDude);
            
            // wave 5
            enemyList[4].Add(circleDude);
            enemyList[4].Add(circleDude);
            enemyList[4].Add(circleDude);
            
            // print(enemyList[0].Count);
            // print(enemyList[1].Count);
            // print(enemyList[2].Count);
            // print(enemyList[3].Count);
            // print(enemyList[4].Count);
        }
        if (level == 2)
        {
            // start waves
        }
        if (level == 3)
        {
            // start waves
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
                Instantiate(enemyList[WaveIndex][spawnIndex], transform.position, Quaternion.identity);
                spawnWaitTime = 0;
                spawnIndex++;
                if (spawnIndex > enemyList[WaveIndex].Count - 1)
                {
                    spawnWaitTime = 0;
                    spawnIndex = 0;
                    SpawningActive = false;
                }
            }
        }
        else if (gameManager.gameState != GameManager.GameState.WaitingToStart) // check to see if each enemy in wavelist is dead
        {
            print(DeadEnemies);
            if (DeadEnemies == enemyList[WaveIndex].Count)
            {
                DeadEnemies = 0;
                print("waiting to start");
                gameManager.gameState = GameManager.GameState.WaitingToStart;
                BattleUI.WaveButton.style.backgroundImage = new StyleBackground(Background.FromTexture2D(BattleUI.WaveStartSprite));
            }
        }
    }
}
