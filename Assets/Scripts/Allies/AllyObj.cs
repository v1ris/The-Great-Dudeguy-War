using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

public class AllyObj : MonoBehaviour
{
    private AllyData allyStats;
    [SerializeField] private SpriteRenderer allySprite;
    
    // targeting
    public List<EnemyObj> targets;
    public EnemyObj currentTarget;
    private float currentTargetX;
    private float currentTargetY;
    
    // components in child

    [SerializeField] private CircleCollider2D attackRange;
    [SerializeField] private SpriteRenderer attackRangeVisual;
    [SerializeField] private Bullet bulletPrefab;
    private Bullet shotBullet;
    
    // placing unit on field
    [SerializeField] private bool hasBeenPlaced = false;
    private bool canBePlaced = false;

    public enum AttackMode
    {
        Closest,
        First,
        Last,
        Strongest,
        Weakest
    }

    private AttackMode attackMode;

    void Start()
    {
        attackRangeVisual.color = new Color(1, 1, 1, 0.1f); // attack range is see through
        attackMode = AttackMode.Weakest; // default attack mode to "first"
        targets = new List<EnemyObj>();
        hasBeenPlaced = false;
    }

    public void LoadData(AllyData allyData)
    {
        allyStats = allyData;
        allySprite.sprite = allyData.allySprite;
    }
    
    private int triggers; // number of triggers currently active; needs to be 0 in order to be placeable
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            targets.Add(other.GetComponent<EnemyObj>());
            // ensures list of targets always has something
            if (targets.Count == 1)
            {
                currentTarget = other.GetComponent<EnemyObj>();
            }
        }
        if (other.gameObject.layer == 8) // layer 8 = allies & path
        {
            triggers++;
            attackRangeVisual.color = new Color(1, 0, 0, 0.1f);
            canBePlaced = false;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            targets.Remove(other.GetComponent<EnemyObj>());
        }
        if (other.gameObject.layer == 8) // layer 8 = allies & path
        {
            triggers--;
             if (triggers == 0)
             {
                 attackRangeVisual.color = new Color(1, 1, 1, 0.1f);
                 canBePlaced = true;
             }
        }
    }

    private float bulletTimer;

    void Update()
    {
        if (!hasBeenPlaced)
        {
            Vector3 dragPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            dragPosition.z = 0;
            transform.position = dragPosition; 
            if (Mouse.current.leftButton.wasPressedThisFrame && canBePlaced)
            {
                RuntimeManager.PlayOneShot("event:/SFX/guy_placed");
                hasBeenPlaced = true;
            }
        }
        else
        {
            // target before attacking
            Target();
        
            // attacking
            // starts timer when there is no enemy in sight
            if (bulletTimer < allyStats.attackSpeed)
            {
                bulletTimer += Time.deltaTime * GameManager.GameSpeed;
            }
            // makes sure there is some target to choose from
            if (targets.Count != 0)
            {
                if (bulletTimer > allyStats.attackSpeed)
                {
                    shotBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                    shotBullet.Shoot(allyStats.attackDamage, allyStats.bulletTravelSpeed,  currentTarget, this);
                    bulletTimer = 0f;
                }
            }
        }
    }
    
    public void Target()
    {
        // first, makes sure there is some target to choose from
        if (targets.Count != 0)
        {
            // target closest
            if (attackMode == AttackMode.Closest)
            {
                for (int i = 0; i < targets.Count; i++)
                {
                    if ((Vector2.Distance(targets[i].transform.position, transform.position))
                        <
                        (Vector2.Distance(currentTarget.transform.position, transform.position)))
                    {
                        currentTarget = targets[i];
                    }
                }
            }
            if (attackMode == AttackMode.First)
            {
                currentTarget = targets[0];
            }
            if (attackMode == AttackMode.Last)
            {
                currentTarget = targets[targets.Count - 1];
            }
            if (attackMode == AttackMode.Strongest)
            {
                int targetCurrentHealth = 0;
                for (int i = 0; i < targets.Count; i++)
                {
                    if (targets[i].currentHealth > targetCurrentHealth)
                    {
                        currentTarget = targets[i];
                        targetCurrentHealth = targets[i].currentHealth;
                    }
                }
            }
            if (attackMode == AttackMode.Weakest)
            {
                int targetCurrentHealth = 999;
                for (int i = 0; i < targets.Count; i++)
                {
                    if (targets[i].currentHealth < targetCurrentHealth)
                    {
                        currentTarget = targets[i];
                        targetCurrentHealth = targets[i].currentHealth;
                    }
                }
            }
        }
    }
}
