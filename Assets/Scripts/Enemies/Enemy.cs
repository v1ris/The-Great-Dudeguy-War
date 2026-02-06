using System;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // stats
    public int health;
    public float moveSpeed;
    public int droppedMoney;
    
    // pathing
    private float distance;
    private int pathPointIndex;
    private GameManager gameManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pathPointIndex = 0;
        gameManager = FindFirstObjectByType<GameManager>();
            
        // move toward next path point
        // (something) * moveSpeed
    }

    // Update is called once per frame
    void Update()
    {
        // move towards path point
        // 

        distance = moveSpeed * Time.deltaTime;
        print(gameManager.pathPoints.Length);
        print(gameManager.pathPoints[pathPointIndex]);
        transform.position = Vector2.MoveTowards(transform.position, gameManager.pathPoints[pathPointIndex].transform.position, distance);
        
        // if bullet hits
        // health - 1
        // destroy other
            // if health == 0
            // destroy
    }
}
