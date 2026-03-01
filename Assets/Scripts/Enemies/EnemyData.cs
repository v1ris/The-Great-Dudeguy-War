using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Data/EnemyData")]
public class EnemyData : ScriptableObject
{
    //display
    public Sprite enemySprite;
    
    // stats
    public int maxHealth;
    public float moveSpeed;
    public int droppedMoney;
}
