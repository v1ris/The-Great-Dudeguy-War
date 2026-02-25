using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static int GameSpeed; // multiply into equation for any time-based function of the game; 0 is paused, 1 is normal, 2 is speedup button active

    public static int Points;
    public static int Lives;

    [SerializeField] private UIDocument battleUI;
    [SerializeField] private Dialogue dialogue;
    private VisualElement fade;
    private VisualElement screen;

    [SerializeField] private GameObject pathpoint1;
    [SerializeField] private GameObject pathpoint2;
    [SerializeField] private GameObject pathpoint3;
    [SerializeField] private GameObject pathpoint4;
    [SerializeField] private GameObject pathpoint5;
    [SerializeField] private GameObject pathpoint6;

    [SerializeField] private Spawner spawner;

    // level control
    public static int CurrentLevel;
    private int fadeTimer;
    public static bool Fading;
    public static bool LevelEnded;
    public static bool LevelRestarted;

    void Start()
    {
        // ensures level is correct
        CurrentLevel = int.Parse(SceneManager.GetActiveScene().name[6].ToString()); // gets number in scene name to determine level; just in case for testing
        
        // disables backgrounds & battle UI by default
        screen = battleUI.rootVisualElement.Q<VisualElement>("screen");
        screen.style.display = DisplayStyle.None;
        fade = battleUI.rootVisualElement.Q<VisualElement>("fade");
        fade.AddToClassList("fade-complete");

        // default game parameter values
        GameSpeed = 1;
        SetPoints(400);
        SetLives(50);

        if (LevelRestarted)
        {
            StartLevel(CurrentLevel);
        }
        
        // ensuring level control is correct
        Fading = false;
        LevelEnded = false;
        LevelRestarted = false;
    }

    public enum GameState
    {
        WaitingToStart,
        WaveActive,
        WavePaused
    }

    public GameState gameState = GameState.WaitingToStart;

    public void StartLevel(int level)
    {
        screen.style.display = DisplayStyle.Flex;
        fade.AddToClassList("fade-in");
        spawner.LoadWaves(level);
    }

    public static void EndLevel()
    {
        Fading = true;
        LevelEnded = true;
    }

    public static void RestartLevel()
    {
        Fading = true;
        LevelRestarted = true;
    }

    private bool startedVisualFade;

    void FixedUpdate()
    {
        if (Fading)
        {
            if (!startedVisualFade)
            {
                screen.style.display = DisplayStyle.Flex;
                fade.RemoveFromClassList("fade-in");
                startedVisualFade = true;
            }

            fadeTimer++;
            if (fadeTimer == 101)
            {
                if (SceneManager.GetActiveScene().name == "Level 1" && LevelEnded)
                {
                    Fading = false;
                    SceneManager.LoadScene("Level 2");
                }

                if (SceneManager.GetActiveScene().name == "Level 2" && LevelEnded)
                {
                    Fading = false;
                    SceneManager.LoadScene("Level 3");
                }
                if (LevelRestarted)
                {
                    //fade.RemoveFromClassList("fade-complete");
                    Spawner.Reset();
                    Fading = false;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
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
            GameObject[] pathPoints = { pathpoint1, pathpoint2, pathpoint3, pathpoint4, pathpoint5, pathpoint6 };
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

    public static void UpdatePoints(int points)
    {
        Points += points;
        BattleUI.Points.text = "Points: \n" + Points;
    }
    
    public static void SetPoints(int points)
    {
        Points = points;
        BattleUI.Points.text = "Points: \n" + Points;
    }

    public static void UpdateLives(int healthLost)
    {
        Lives -= healthLost;
        BattleUI.Lives.text = "Lives: " + Lives;
        if (Lives <= 0)
        {
            SetLives(0);
            RestartLevel();
        }
    }

    private static void SetLives(int lives)
    {
        Lives = lives;
        BattleUI.Lives.text = "Lives: " + Lives;
    }
}