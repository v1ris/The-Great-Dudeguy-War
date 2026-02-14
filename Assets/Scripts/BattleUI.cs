using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class BattleUI : MonoBehaviour
{
    [SerializeField] private Texture2D waveStart;
    [SerializeField] private Texture2D wavePause;
    [SerializeField] private Texture2D waveFastForward;
    public static Texture2D WaveStartSprite;
    private bool fastForwarding;
    private UIDocument ui;

    public static Button WaveButton;
    private Button fastForwardButton;
    public static Label Points;
    
    private Button normalGuyButton;
    private Button hatGuyButton;
    [SerializeField] GameObject normalGuy;
    [SerializeField] GameObject hatGuy;
    
    private GameManager gameManager;
    private Spawner spawner;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        spawner = FindFirstObjectByType<Spawner>();
        
        ui = GetComponent<UIDocument>();
        
        WaveButton = ui.rootVisualElement.Q<Button>("waveButton");
        WaveButton.RegisterCallback<ClickEvent>(OnWaveButtonClicked);
        WaveStartSprite = waveStart;
        
        fastForwardButton = ui.rootVisualElement.Q<Button>("fastForwardButton");
        fastForwardButton.RegisterCallback<ClickEvent>(OnFastFowardButtonClicked);
        
        normalGuyButton = ui.rootVisualElement.Q<Button>("normalGuyButton");
        normalGuyButton.RegisterCallback<ClickEvent>(OnNormalGuyButtonClicked);
        
        hatGuyButton = ui.rootVisualElement.Q<Button>("hatGuyButton");
        hatGuyButton.RegisterCallback<ClickEvent>(OnHatGuyButtonClicked);
        
        Points = ui.rootVisualElement.Q<Label>("points");
    }

    private void OnWaveButtonClicked(ClickEvent evt)
    {
        // Start Wave
        if (gameManager.gameState == GameManager.GameState.WaitingToStart)
        {
            Spawner.SpawningActive = true;
            Spawner.WaveIndex++;
            gameManager.gameState = GameManager.GameState.WaveActive;
            WaveButton.style.backgroundImage = new StyleBackground(Background.FromTexture2D(wavePause));
        }
        
        // set paused
        else if (gameManager.gameState == GameManager.GameState.WaveActive)
        {
            gameManager.gameState = GameManager.GameState.WavePaused;
            GameManager.GameSpeed = 0;
            WaveButton.style.backgroundImage = new StyleBackground(Background.FromTexture2D(waveStart));
        }
        // set active
        else // if game state == WavePaused
        {
            gameManager.gameState = GameManager.GameState.WaveActive;
            GameManager.GameSpeed = 1;
            WaveButton.style.backgroundImage = new StyleBackground(Background.FromTexture2D(wavePause));
        }
    }

    private void OnFastFowardButtonClicked(ClickEvent evt)
    {
        // turn on/off fast forward
        if (gameManager.gameState == GameManager.GameState.WaveActive && !fastForwarding)
        {
            fastForwardButton.AddToClassList("change-tint");
            GameManager.GameSpeed = 2;
            fastForwarding = true;
        }
        else
        {
            GameManager.GameSpeed = 1;
            fastForwardButton.RemoveFromClassList("change-tint");
            fastForwarding = false;
        }
    }
    
    private void OnNormalGuyButtonClicked(ClickEvent evt)
    {
        if (GameManager.Points >= 100)
        {
            GameObject guy = Instantiate(normalGuy, Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Quaternion.identity);
            GameManager.UpdatePoints(-100);
        }
    }
    private void OnHatGuyButtonClicked(ClickEvent evt)
    {
        if (GameManager.Points >= 200)
        {
            GameObject guy = Instantiate(hatGuy, Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Quaternion.identity);
            GameManager.UpdatePoints(-200);
        }
    }
}


