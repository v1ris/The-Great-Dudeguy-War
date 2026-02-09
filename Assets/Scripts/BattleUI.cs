using UnityEngine;
using UnityEngine.UIElements;

public class BattleUI : MonoBehaviour
{
    [SerializeField] private Texture2D waveStart;
    [SerializeField] private Texture2D wavePause;
    [SerializeField] private Texture2D waveFastForward;
    private UIDocument ui;

    private Button waveButton;
    private Button normalGuyButton;
    public static Label Points;
    
    private GameManager gameManager;
    private Spawner spawner;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        spawner = FindFirstObjectByType<Spawner>();
        
        ui = GetComponent<UIDocument>();
        waveButton = ui.rootVisualElement.Q<Button>("waveButton");
        waveButton.RegisterCallback<ClickEvent>(OnWaveButtonClicked);
        normalGuyButton = ui.rootVisualElement.Q<Button>("normalGuyButton");
        normalGuyButton.RegisterCallback<ClickEvent>(OnGuyButtonClicked);
        Points = ui.rootVisualElement.Q<Label>("points");
    }

    private void OnWaveButtonClicked(ClickEvent evt)
    {
        // Start Wave
        if (gameManager.gameState == GameManager.GameState.WaitingToStart)
        {
            Spawner.SpawningActive = true;
            Spawner.WaveIndex++;
            print(Spawner.WaveIndex);
            gameManager.gameState = GameManager.GameState.WaveActive;
            GameManager.GameSpeed = 1;
            waveButton.style.backgroundImage = new StyleBackground(Background.FromTexture2D(waveFastForward));
        }
        
        // Controls Game Speed
        else if (gameManager.gameState == GameManager.GameState.WaveActive)
        {
            gameManager.gameState = GameManager.GameState.FastForwarding;
            GameManager.GameSpeed = 2;
            waveButton.style.backgroundImage = new StyleBackground(Background.FromTexture2D(wavePause));
        }
        else if (gameManager.gameState == GameManager.GameState.FastForwarding)
        {
            gameManager.gameState = GameManager.GameState.WavePaused;
            GameManager.GameSpeed = 0;
            waveButton.style.backgroundImage = new StyleBackground(Background.FromTexture2D(waveStart));
        }
        else // if game state == WavePaused
        {
            gameManager.gameState = GameManager.GameState.WaveActive;
            GameManager.GameSpeed = 1;
            waveButton.style.backgroundImage = new StyleBackground(Background.FromTexture2D(waveFastForward));
        }
        print(gameManager.gameState);
    }

    private void OnGuyButtonClicked(ClickEvent evt)
    {
        // create guy at mouse position
    }
}


