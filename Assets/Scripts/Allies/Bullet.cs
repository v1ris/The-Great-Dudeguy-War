using FMODUnity;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage;
    private int speed;
    private bool moving;
    private Rigidbody2D rb;
    private GameObject target;
    private GameObject shooter;
    private Vector3 movementVector;
    
    public void Shoot(int passedDamage, int passedSpeed, GameObject passedTarget, GameObject passedShooter)
    {
        damage = passedDamage;
        speed = passedSpeed;
        target = passedTarget;
        shooter = passedShooter;
        movementVector = (target.transform.position - transform.position);
        moving = true;
        
        // play sound
        RuntimeManager.PlayOneShot("event:/SFX/bullet_shoot");
    }

    void Update()
    {
        if (moving)
        {
            transform.position += movementVector * speed * Time.deltaTime * GameManager.GameSpeed;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            // damage and killing
            var enemyClass = other.gameObject.GetComponent<Enemy>();
            enemyClass.health -= damage;
            if (enemyClass.health == 0)
            {
                // mark enemy as dead and untargetable
                var allyClass = shooter.GetComponent<Ally>();
                allyClass.targets.Remove(other.gameObject);
                // makes sure current target is not null
                if (allyClass.targets.Count != 0)
                {
                    allyClass.currentTarget = allyClass.targets[0]; // some temp target to make sure there's no error
                    allyClass.Target();
                }
                Destroy(other.gameObject);
            }
            // update health bar
            enemyClass.greenHealthBar.transform.localScale = new Vector3((float)enemyClass.health / enemyClass.maxHealth, .2f, 1); // .2f is to make the square sprite into a long rectangle
            Destroy(gameObject);
        }
    }
}
