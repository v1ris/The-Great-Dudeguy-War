using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public int gameSpeed; // multiply into equation for any time-based function of the game; 0 is paused, 1 is normal, 2 is speedup button active

    [SerializeField] private UIDocument ui;
    
    [SerializeField] private SpriteRenderer lvl1Background;
    [SerializeField] private SpriteRenderer lvl2Background;
    [SerializeField] private SpriteRenderer lvl3Background;

    [SerializeField] private Spawner spawner;
    
    [SerializeField] private PolygonCollider2D enemyPath;
    [SerializeField] private GameObject pathPoint1;
    [SerializeField] private GameObject pathPoint2;
    [SerializeField] private GameObject pathPoint3;

    void Start()
    {
        // disables backgrounds & battle UI by default
        lvl1Background.enabled = false;
        lvl2Background.enabled = false;
        lvl3Background.enabled = false;
        ui.enabled = false;
        
        gameSpeed = 1;
    }

    void Update()
    {
    }

    public void StartLevel(int level)
    {
        if (level == 1)
        {
            lvl1Background.enabled = true;
            ui.enabled = true;
            // gameobject spawner method: start level(1)
        }

        if (level == 2)
        {
            lvl2Background.enabled = true;
            ui.enabled = true;
            // gameobject spawner method: start level 2
        }

        if (level == 3)
        {
            lvl3Background.enabled = true;
            ui.enabled = true;
            // gameobject spawner method: start level 3
        }
    }
}