using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ally : MonoBehaviour
{
    // stats
    public int attackDamage;
    public int attackSpeed;
    public int bulletTravelSpeed;
    public int price;
    public int sellprice;
    
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
        attackMode = AttackMode.Closest; // default attack mode to "closest"
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
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            targets.Remove(other.gameObject);
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
            if (Mouse.current.leftButton.wasPressedThisFrame)
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
            }
        }
    }
}
