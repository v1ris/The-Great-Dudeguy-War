using FMODUnity;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage;
    private int speed;
    private bool moving;
    private Rigidbody2D rb;
    private AllyObj shooter;
    private Vector3 movementVector;
    
    public void Shoot(int passedDamage, int passedSpeed, EnemyObj target, AllyObj passedShooter)
    {
        damage = passedDamage;
        speed = passedSpeed;
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
            transform.position += movementVector * (speed * Time.deltaTime * GameManager.GameSpeed);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            // damage and killing
            var enemyClass = other.gameObject.GetComponent<EnemyObj>();
            enemyClass.UpdateHealth(damage);
            if (enemyClass.GetIsDead())
            {
                // mark enemy as dead and untargetable
                shooter.targets.Remove(other.GetComponent<EnemyObj>());
                // makes sure current target is not null
                if (shooter.targets.Count != 0)
                {
                    shooter.currentTarget = shooter.targets[0]; // some temp target to make sure there's no error
                    shooter.Target();
                }
                Destroy(other.gameObject);
            }
            Destroy(gameObject);
        }
    }
}
