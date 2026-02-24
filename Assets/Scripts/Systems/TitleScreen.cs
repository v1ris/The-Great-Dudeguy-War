using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    private UIDocument ui;
    private Button playButton;
    private Button quitButton;
    private VisualElement fade;
    private bool fading;
    private GameObject bgm;

    void Start()
    {
        ui = GetComponent<UIDocument>();
        playButton = ui.rootVisualElement.Q<Button>("playbutton");
        quitButton = ui.rootVisualElement.Q<Button>("quitbutton");
        playButton.RegisterCallback<ClickEvent>(ClickPlay);
        quitButton.RegisterCallback<ClickEvent>(ClickQuit);
        fade = ui.rootVisualElement.Q<VisualElement>("fade");
        fade.style.display = DisplayStyle.None;
        fading = false;
        
        // audio
        bgm = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/music/titlescreen"));
    }

    private void ClickPlay(ClickEvent evt)
    {
        bgm.GetComponent<StudioEventEmitter>().SetParameter("Fadeout", 1);
        fade.style.display = DisplayStyle.Flex;
        fading = true;
        fade.AddToClassList("fade-out");
    }

    private void ClickQuit(ClickEvent evt)
    {
        print("I quit");
        Application.Quit();
    }

    private int fadingTimer = 0;
    private int fadingTimerMax = 100;
    void FixedUpdate()
    {
        if (fading)
        {
            fadingTimer++;
            // do something with a timer, fade out music and screen for 2 seconds
            // At the end of the timer, loading state = loaded
        }

        if (fadingTimer == fadingTimerMax)
        {
            SceneManager.LoadScene("Level 1");
        }
    }
}