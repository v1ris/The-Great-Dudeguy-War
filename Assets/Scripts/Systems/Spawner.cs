using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    private int spawnWaitTime;
    private int spawnIndex;
    
    public static int WaveIndex;
    public static bool SpawningActive;
    
    private List<List<GameObject>> enemyList;
    public static int DeadEnemies;

    [SerializeField] private GameObject circleDude;
    [SerializeField] private GameObject triangleDude;
    [SerializeField] private GameObject squareDude;
    [SerializeField] private GameManager gameManager;

    void Start()
    {
        WaveIndex = -1;
        DeadEnemies = 0;
        SpawningActive = false;
    }
    
    public void LoadWaves(int level)
    {
        print(level);
        if (level == 1)
        {
            enemyList  = new List<List<GameObject>>();
            enemyList.Insert(0, new List<GameObject>());
            enemyList.Insert(1, new List<GameObject>());
            enemyList.Insert(2, new List<GameObject>());
            enemyList.Insert(3, new List<GameObject>());
            
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
            enemyList[2].Add(triangleDude);
            enemyList[2].Add(circleDude);
            enemyList[2].Add(triangleDude);
            enemyList[2].Add(circleDude);
            enemyList[2].Add(triangleDude);
            enemyList[2].Add(triangleDude);
            enemyList[2].Add(triangleDude);
            
            // wave 4
            enemyList[3].Add(triangleDude);
            enemyList[3].Add(triangleDude);
            enemyList[3].Add(triangleDude);
            enemyList[3].Add(triangleDude);
            enemyList[3].Add(triangleDude);
            enemyList[3].Add(triangleDude);
            enemyList[3].Add(triangleDude);
            enemyList[3].Add(triangleDude);
            enemyList[3].Add(triangleDude);
            enemyList[3].Add(squareDude);
            
            // // test level
            // enemyList  = new List<List<GameObject>>();
            // enemyList.Insert(0, new List<GameObject>());
            // enemyList[0].Add(circleDude);
        }
        if (level == 2)
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
            enemyList[0].Add(triangleDude);
            enemyList[0].Add(triangleDude);
            enemyList[0].Add(triangleDude);
            
            // wave 2
            enemyList[1].Add(circleDude);
            enemyList[1].Add(circleDude);
            enemyList[1].Add(circleDude);
            enemyList[1].Add(circleDude);
            enemyList[1].Add(circleDude);
            enemyList[1].Add(triangleDude);
            enemyList[1].Add(triangleDude);
            enemyList[1].Add(triangleDude);
            enemyList[1].Add(triangleDude);
            enemyList[1].Add(triangleDude);
            
            // wave 3
            enemyList[2].Add(circleDude);
            enemyList[2].Add(triangleDude);
            enemyList[2].Add(squareDude);
            enemyList[2].Add(circleDude);
            enemyList[2].Add(triangleDude);
            enemyList[2].Add(triangleDude);
            enemyList[2].Add(triangleDude);
            
            // wave 4
            enemyList[3].Add(squareDude);
            enemyList[3].Add(triangleDude);
            enemyList[3].Add(triangleDude);
            enemyList[3].Add(triangleDude);
            enemyList[3].Add(triangleDude);
            enemyList[3].Add(triangleDude);
            enemyList[3].Add(triangleDude);
            enemyList[3].Add(triangleDude);
            enemyList[3].Add(triangleDude);
            enemyList[3].Add(squareDude);
            
            // wave 5
            enemyList[4].Add(squareDude);
            enemyList[4].Add(squareDude);
            enemyList[4].Add(squareDude);
            enemyList[4].Add(squareDude);
            enemyList[4].Add(squareDude);
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
}
