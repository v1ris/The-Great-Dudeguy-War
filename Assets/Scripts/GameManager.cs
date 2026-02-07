using System;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    [SerializeField] private GameObject pathpoint1;
    [SerializeField] private GameObject pathpoint2;
    [SerializeField] private GameObject pathpoint3;
    [SerializeField] private GameObject pathpoint4;
    [SerializeField] private GameObject pathpoint5;
    
    [SerializeField] private Spawner spawner;
    

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
        
        // if scene == lvl 1

        

        gameSpeed = 1;
    }

    void Update()
    {
    }

    public void StartLevel(int level)
    {
        screen.style.display = DisplayStyle.Flex;
        fade.AddToClassList("fade-in");
        if (level == 1)
        {
            // pathPoints = GameObject.FindGameObjectsWithTag("Level 1 Pathpoint");
            // Array.Sort(pathPoints);


            lvl1Background.enabled = true;
            
            // Temp until wave button is implemented
            spawner.StartWave(level);
        }

        if (level == 2)
        {

            lvl2Background.enabled = true;
            // gameobject spawner method: start level 2
        }

        if (level == 3)
        {

            lvl3Background.enabled = true;
            // gameobject spawner method: start level 3
        }
    }

    public GameObject[] RetrievePathPoints()
    {
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            GameObject[] pathPoints = { pathpoint1, pathpoint2, pathpoint3 };
            return pathPoints;
        }
        else if (SceneManager.GetActiveScene().name == "Level 2")
        {
            GameObject[] pathPoints = { pathpoint1, pathpoint2, pathpoint3 };
            return pathPoints;
        }
        else if (SceneManager.GetActiveScene().name == "Level 3")
        {
            GameObject[] pathPoints = { pathpoint1, pathpoint2, pathpoint3 };
            return pathPoints;
        }
        else
        {
            return null;
        }
    }
}