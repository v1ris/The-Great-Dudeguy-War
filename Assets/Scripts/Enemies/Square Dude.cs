using UnityEngine;

public class SquareDude : MonoBehaviour
{
    private Enemy enemyClass;
    void Start()
    {
        enemyClass = gameObject.GetComponent<Enemy>();
        enemyClass.health = 10;
        enemyClass.droppedMoney = 35;
        enemyClass.moveSpeed = .7f;
    }
}
