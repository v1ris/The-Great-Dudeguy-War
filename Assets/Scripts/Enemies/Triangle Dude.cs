using UnityEngine;

public class TriangleDude : MonoBehaviour
{
    private Enemy enemyClass;
    void Start()
    {
        enemyClass = gameObject.GetComponent<Enemy>();
        enemyClass.health = 4;
        enemyClass.droppedMoney = 20;
        enemyClass.moveSpeed = 1.3f;
    }
}
