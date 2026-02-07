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
    private PolygonCollider2D hitbox;
    
    void Start()
    {
        pathPointIndex = 0;
        gameManager = FindFirstObjectByType<GameManager>();
        hitbox = gameObject.GetComponent<PolygonCollider2D>();
    }
    
    void Update()
    {
        // move towards path point
        distance = moveSpeed * Time.deltaTime;
        Vector2 pathPointTransform = gameManager.RetrievePathPoints()[pathPointIndex].transform.position;
        transform.position = Vector2.MoveTowards(transform.position, pathPointTransform, distance);
        if ((Mathf.Approximately(transform.position.x, pathPointTransform.x)) && (Mathf.Approximately(transform.position.y, pathPointTransform.y)))
        {
            pathPointIndex++;
            if (pathPointIndex >= gameManager.RetrievePathPoints().Length)
            {
                Destroy(gameObject);
            }
        }
        


        // if bullet hits
        // health - 1
        // destroy other
        // if health == 0
        // destroy
    }
}
