using System.Collections.Generic;
using UnityEngine;

public class Ally : MonoBehaviour
{
    // stats
    private int attackDamage;
    private int attackSpeed;
    public int price;
    public int sellprice;
    
    // targeting
    private List<GameObject> targets;
    private GameObject currentTarget;
    private float currentTargetX;
    private float currentTargetY;
    
    // components in child
    [SerializeField] CircleCollider2D attackRange;
    [SerializeField] SpriteRenderer attackRangeVisual;

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
        attackRangeVisual.color = new Color(1, 1, 1, 0.3f);
        attackMode = AttackMode.Closest; // default attack mode to "closest"
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            targets.Add(other.gameObject);
            // makes sure list of targets always has something
            if (targets.Count == 1)
            {
                currentTarget = other.gameObject;
            }
        }
    }

    void FixedUpdate()
    {
        // makes sure there is some target to choose from
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
                print(currentTarget);
            }
            if (attackMode == AttackMode.First)
            {
            }
        }
    }
}
