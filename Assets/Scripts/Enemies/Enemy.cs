using System;
using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // stats
    public int health;
    public int maxHealth;
    public float moveSpeed;
    public int droppedMoney;
    
    // pathing
    private int pathPointIndex;
    private GameManager gameManager;
    private PolygonCollider2D hitbox;
    
    // healthbar
    [SerializeField] private GameObject healthBarReference;
    private GameObject healthBar;
    public GameObject greenHealthBar;
    
    void Start()
    {
        pathPointIndex = 0;
        gameManager = FindFirstObjectByType<GameManager>();
        hitbox = gameObject.GetComponent<PolygonCollider2D>();
        
        // create healthbar above enemy, store maxHealth value
        var healthBarPosition = new Vector2(transform.position.x, transform.position.y + .5f);
        healthBar = Instantiate(healthBarReference, healthBarPosition, Quaternion.identity);
        healthBar.transform.parent = gameObject.transform;
        greenHealthBar = healthBar.transform.GetChild(1).gameObject;
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
                GameManager.UpdateLives(health);
                Destroy(gameObject);
            }
        }
    }
    

    void OnDestroy()
    {
        Spawner.DeadEnemies++;
        GameManager.UpdatePoints(droppedMoney);
        
        // play sound
        RuntimeManager.PlayOneShot("event:/SFX/dude_hurt");
    }
}
