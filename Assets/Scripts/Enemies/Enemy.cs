using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public int moveSpeed;
    public int droppedMoney;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()        
    {
        // move toward next path point
        // (something) * moveSpeed
    }

    // Update is called once per frame
    void Update()
    {
        // if bullet hits
        // health - 1
        // destroy other
            // if health == 0
            // destroy
    }
}
