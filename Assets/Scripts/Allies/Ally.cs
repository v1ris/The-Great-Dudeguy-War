using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ally : MonoBehaviour
{
    // stats
    public int attackDamage;
    public float attackSpeed;
    public int bulletTravelSpeed;
    public int price;
    public int sellPrice;
    
    // targeting
    public List<GameObject> targets;
    public GameObject currentTarget;
    private float currentTargetX;
    private float currentTargetY;
    
    // components in child
    [SerializeField] private CircleCollider2D attackRange;
    [SerializeField] private SpriteRenderer attackRangeVisual;
    [SerializeField] private GameObject bullet;
    private GameObject shotBullet;
    
    // placing unit on field
    private bool hasBeenPlaced = false;
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
        targets = new List<GameObject>();
        hasBeenPlaced = false;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            targets.Add(other.gameObject);
            // ensures list of targets always has something
            if (targets.Count == 1)
            {
                currentTarget = other.gameObject;
            }
        }
        if (other.gameObject.layer == 8) // layer 8 = allies & path
        {
            attackRangeVisual.color = new Color(1, 0, 0, 0.1f);
            canBePlaced = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            targets.Remove(other.gameObject);
        }
        if (other.gameObject.layer == 8) // layer 8 = allies & path
        {
            attackRangeVisual.color = new Color(1, 1, 1, 0.1f);
            canBePlaced = true;
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
                hasBeenPlaced = true;
            }
        }
        else
        {
            // target before attacking
            Target();
        
            // attacking
            // starts timer when there is no enemy in sight
            if (bulletTimer < attackSpeed)
            {
                bulletTimer += Time.deltaTime * GameManager.GameSpeed;
            }
            // makes sure there is some target to choose from
            if (targets.Count != 0)
            {
                if (bulletTimer > attackSpeed)
                {
                    shotBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                    shotBullet.GetComponent<Bullet>().Shoot(attackDamage, bulletTravelSpeed, currentTarget, gameObject);
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
                    if (targets[i].GetComponent<Enemy>().health > targetCurrentHealth)
                    {
                        currentTarget = targets[i];
                        targetCurrentHealth = targets[i].GetComponent<Enemy>().health;
                    }
                }
            }
            if (attackMode == AttackMode.Weakest)
            {
                int targetCurrentHealth = 999;
                for (int i = 0; i < targets.Count; i++)
                {
                    if (targets[i].GetComponent<Enemy>().health < targetCurrentHealth)
                    {
                        currentTarget = targets[i];
                        targetCurrentHealth = targets[i].GetComponent<Enemy>().health;
                    }
                }
            }
        }
    }
}
