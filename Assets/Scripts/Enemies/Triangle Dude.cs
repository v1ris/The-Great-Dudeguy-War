using UnityEngine;

public class TriangleDude : MonoBehaviour
{
    private Enemy enemyClass;
    void Start()
    {
        enemyClass = gameObject.GetComponent<Enemy>();
        enemyClass.health = enemyClass.maxHealth = 7;
        enemyClass.droppedMoney = 40;
        enemyClass.moveSpeed = 1.3f;
    }
}