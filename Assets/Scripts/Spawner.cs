using System;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private int spawnWaitTime;
    private int spawnIndex;
    
    private int waveIndex;
    private bool waveActive;
    
    private List<List<GameObject>> enemyList;

    [SerializeField] private GameObject circleDude;
    [SerializeField] private GameObject circleDude2;
    [SerializeField] private GameObject triangleDude;
    [SerializeField] private GameObject squareDude;
    
    public void StartWave(int level)
    {
        if (level == 1)
        {
            enemyList  = new List<List<GameObject>>();
            List<GameObject> waveList = new List<GameObject>();
            waveList.Add(circleDude);
            waveList.Add(circleDude2);
            waveList.Add(circleDude);
            waveList.Add(circleDude);
            waveList.Add(circleDude);
            waveActive = true;
            enemyList.Add(waveList);
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
        if (waveActive)
        {
            // spawn until wave is complete
            spawnWaitTime++;
            if (spawnWaitTime == 50)
            {
                Instantiate(enemyList[waveIndex][spawnIndex], transform.position, Quaternion.identity);
                spawnWaitTime = 0;
                spawnIndex++;
                if (spawnIndex > enemyList[waveIndex].Count - 1)
                {
                    spawnWaitTime = 0;
                    spawnIndex = 0;
                    waveActive = false;
                }
            }
        }
    }
}
