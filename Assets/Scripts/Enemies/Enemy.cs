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
    private int pathPointIndex;
    private GameManager gameManager;
    private PolygonCollider2D hitbox;
    
    void Start()
    {
        pathPointIndex = 0;
        gameManager = FindFirstObjectByType<GameManager>();
        hitbox = gameObject.GetComponent<PolygonCollider2D>();
    }
    
    void Update()
    {
        // pathing
        // getting temp values
        float distance = moveSpeed * Time.deltaTime;
        Vector2 pathPointTransform = gameManager.RetrievePathPoints()[pathPointIndex].transform.position;
        // move towards path point
        transform.position = Vector2.MoveTowards(transform.position, pathPointTransform, distance * GameManager.GameSpeed);
        if ((Mathf.Approximately(transform.position.x, pathPointTransform.x)) && (Mathf.Approximately(transform.position.y, pathPointTransform.y)))
        {
            pathPointIndex++;
            if (pathPointIndex >= gameManager.RetrievePathPoints().Length)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnDestroy()
    {
        Spawner.DeadEnemies++;
        GameManager.Points += droppedMoney;
        BattleUI.Points.text = "Points: \n" + GameManager.Points;
    }
}
