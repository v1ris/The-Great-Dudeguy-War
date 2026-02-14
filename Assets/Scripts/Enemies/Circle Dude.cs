using UnityEngine;

public class CircleDude : MonoBehaviour
{
    private Enemy enemyClass;
    void Start()
    {
        enemyClass = gameObject.GetComponent<Enemy>();
        enemyClass.health = 3;
        enemyClass.droppedMoney = 20;
        enemyClass.moveSpeed = 1f;
    }
}
