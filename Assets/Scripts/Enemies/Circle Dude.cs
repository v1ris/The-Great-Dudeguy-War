using UnityEngine;

public class CircleDude : MonoBehaviour
{
    private Enemy enemyClass;
    void Start()
    {
        enemyClass = gameObject.GetComponent<Enemy>();
        enemyClass.health = 1;
        enemyClass.droppedMoney = 10;
        enemyClass.moveSpeed = 1f;
    }
}
