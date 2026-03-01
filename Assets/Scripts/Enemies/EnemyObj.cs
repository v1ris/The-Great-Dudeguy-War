using System;
using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyObj : MonoBehaviour
{
    public EnemyData enemyData;

    //display
    [SerializeField] private SpriteRenderer renderer; 
    
    // pathing
    private int pathPointIndex;
    private GameManager gameManager;
    private PolygonCollider2D hitbox;
    
    // healthbar
    [SerializeField] private GameObject healthBarReference;
    private GameObject healthBar;
    public GameObject greenHealthBar;
    
    //health
    public int currentHealth;
    public bool isDead = false;
    
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

    public void LoadData(EnemyData data)
    {
        enemyData = data; 
        currentHealth = data.maxHealth;
        renderer.sprite = data.enemySprite;
    }
    
    void Update()
    {
        // pathing
        // getting temp values
        float distance = enemyData.moveSpeed * Time.deltaTime;
        Vector2 pathPointTransform = gameManager.RetrievePathPoints()[pathPointIndex].transform.position;
        // move towards path point
        transform.position = Vector2.MoveTowards(transform.position, pathPointTransform, distance * GameManager.GameSpeed);
        if ((Mathf.Approximately(transform.position.x, pathPointTransform.x)) && (Mathf.Approximately(transform.position.y, pathPointTransform.y)))
        {
            pathPointIndex++;
            if (pathPointIndex >= gameManager.RetrievePathPoints().Length)
            {
                GameManager.UpdateLives(currentHealth);
                Destroy(gameObject);
            }
        }
    }

    public void UpdateHealth(int amountToSubtract)
    {
        currentHealth -= amountToSubtract;
        if (currentHealth <= 0)
        {
            isDead = true;   
        }
        // update health bar
        greenHealthBar.transform.localScale = new Vector3((float)currentHealth / enemyData.maxHealth, .2f, 1); // .2f is to make the square sprite into a long rectangle
    }


    public bool GetIsDead()
    {
        return isDead;
    }

    void OnDestroy()
    {
        Spawner.DeadEnemies++;
        GameManager.UpdatePoints(enemyData.droppedMoney);
        
        // play sound
        RuntimeManager.PlayOneShot("event:/SFX/dude_hurt");
    }
}
