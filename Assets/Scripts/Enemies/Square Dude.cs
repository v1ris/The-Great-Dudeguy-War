using UnityEngine;

public class SquareDude : MonoBehaviour
{
    private Enemy enemyClass;
    void Start()
    {
        enemyClass = gameObject.GetComponent<Enemy>();
        enemyClass.health = enemyClass.maxHealth = 40;
        enemyClass.droppedMoney = 75;
        enemyClass.moveSpeed = .7f;
    }
}
