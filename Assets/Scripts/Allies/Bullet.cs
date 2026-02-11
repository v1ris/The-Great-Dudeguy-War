using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage;
    private int speed;
    private bool moving;
    private Rigidbody2D rb;
    private GameObject target;
    private Vector2 targetPastPosition;
    private GameObject shooter;
    
    public void Shoot(int passedDamage, int passedSpeed, GameObject passedTarget, GameObject passedShooter)
    {
        damage = passedDamage;
        speed = passedSpeed;
        target = passedTarget;
        targetPastPosition = target.transform.position;
        shooter = passedShooter;
        moving = true;
    }

    void Update()
    {
        if (moving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPastPosition, speed * Time.deltaTime * GameManager.GameSpeed);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            // damage and killing
            other.gameObject.GetComponent<Enemy>().health -= damage;
            if (other.gameObject.GetComponent<Enemy>().health == 0)
            {
                // mark enemy as dead and untargetable
                Ally allyClass = shooter.GetComponent<Ally>();
                allyClass.targets.Remove(other.gameObject);
                // makes sure current target is not null
                if (allyClass.targets.Count != 0)
                {
                    allyClass.currentTarget = allyClass.targets[0]; // some temp target to make sure there's no error
                    allyClass.Target();
                }
                Destroy(other.gameObject);
            }
            Destroy(gameObject);
        }
    }
}
