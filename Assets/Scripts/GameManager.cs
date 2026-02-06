using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public int gameSpeed; // multiply into equation for any time-based function of the game; 0 is paused, 1 is normal, 2 is speedup button active

    [SerializeField] private UIDocument battleUI;
    private VisualElement fade;
    private VisualElement screen;
    
    [SerializeField] private SpriteRenderer lvl1Background;
    [SerializeField] private SpriteRenderer lvl2Background;
    [SerializeField] private SpriteRenderer lvl3Background;

    [SerializeField] private Spawner spawner;
    
    [SerializeField] private PolygonCollider2D enemyPath;
    public GameObject[] pathPoints;

    public int currentLevel;

    void Start()
    {
        // disables backgrounds & battle UI by default
        lvl1Background.enabled = false;
        lvl2Background.enabled = false;
        lvl3Background.enabled = false;
        screen = battleUI.rootVisualElement.Q<VisualElement>("screen");
        screen.style.display = DisplayStyle.None;
        fade = battleUI.rootVisualElement.Q<VisualElement>("fade");
        fade.AddToClassList("fade-complete");
        
        gameSpeed = 1;
    }

    void Update()
    {
    }

    public void StartLevel(int level)
    {
        print(fade);
        screen.style.display = DisplayStyle.Flex;
        fade.AddToClassList("fade-in");
        if (level == 1)
        {
            pathPoints = GameObject.FindGameObjectsWithTag("Level 1 Pathpoint");
            print(pathPoints.Length);
            print(pathPoints[0]);
            lvl1Background.enabled = true;
            
            // Temp until wave button is implemented
            spawner.StartWave(level);
        }

        if (level == 2)
        {
            pathPoints = GameObject.FindGameObjectsWithTag("Level 2 Pathpoint");
            lvl2Background.enabled = true;
            // gameobject spawner method: start level 2
        }

        if (level == 3)
        {
            pathPoints = GameObject.FindGameObjectsWithTag("Level 3 Pathpoint");
            lvl3Background.enabled = true;
            // gameobject spawner method: start level 3
        }
    }
}