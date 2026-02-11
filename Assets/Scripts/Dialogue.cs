using System;
using System.Collections.Generic;
using System.Text;
using FMOD;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private Texture fuckassPortrait;
    [SerializeField] private Texture wiseguyPortrait;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private GameManager gameManager;
    private Label dialogueText;
    private Image dialoguePortrait;
    private VisualElement textBox;
    private VisualElement nameEntryBox;
    private Button nameEntryConfirm;
    private TextField textField;
    private VisualElement fade;
    private VisualElement introBackground;
    
    // Objects to reference when changing sounds
    private GameObject talkingSFX;
    private GameObject bgm;
    
    // Scrolling Dialogue
    private string speakerName;
    private string scrollingText; // dialogue displayed character by character
    private string fullText; // actual line to check to know when to stop iterating
    private int currentCharacter;
    [SerializeField] private int currentLine; // which dialogue switch case is selected
    private bool isScrolling;
    private bool nameEntryActive;
    
    // Scene changing
    private bool fading;
    
    // Scroll Speed
    private int fixedUpdateTimer = 0;
    public int scrollSpeed = 2; // higher number means slower. i might try to fix this later but idk how
    
    void Start()
    {
        // getting all the UI stuff
        UIDocument ui = gameObject.GetComponent<UIDocument>();
        dialogueText = ui.rootVisualElement.Q<Label>("text");
        dialoguePortrait = ui.rootVisualElement.Q<Image>("portrait");
        textBox = ui.rootVisualElement.Q<VisualElement>("textbox");
        nameEntryBox = ui.rootVisualElement.Q<VisualElement>("nameentrybox");
        textField = ui.rootVisualElement.Q<TextField>("nameentry");
        nameEntryConfirm = ui.rootVisualElement.Q<Button>("nameentryconfirm");
        fade = ui.rootVisualElement.Q<VisualElement>("fade");
        fade.style.display = DisplayStyle.None;
        introBackground = ui.rootVisualElement.Q<VisualElement>("introbackground");
        introBackground.style.display = DisplayStyle.Flex;
        
        // initializing variables for dialogue
        currentLine = 0;
        currentCharacter = 0;
        IterateDialogue(0);
        
        // setting up the scene
        bgm = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/Music/wiseguy_theme"));
        nameEntryBox.style.display = DisplayStyle.None;
    }
    
    void Update()
    {
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            // skipping dialogue
            if (isScrolling)
            {
                dialogueText.text = speakerName + fullText;
                scrollingText = fullText;
                isScrolling = false;
            }
            // normal handling as long as name entry isn't active
            else if (!nameEntryActive)
            {
                currentCharacter = 0;
                scrollingText = "";
                fullText = "";
                currentLine++;
                IterateDialogue(currentLine);
            }
        }
        // if name entry is active, makes sure text field is not blank
        if ((nameEntryActive) && (textField.text != ""))
        {
            nameEntryConfirm.RegisterCallback<ClickEvent>(OnButtonClicked);
        }
    }
    
    private void OnButtonClicked(ClickEvent evt)
    {
        currentLine++;
        IterateDialogue(currentLine);
    }

    private int fadeTimer;
    private void FixedUpdate()
    {
        // scrolling timer, new character every frame
        if (scrollingText != fullText)
        {
            nameEntryActive = false;
            fixedUpdateTimer++;
        }
        // makes sure the sound isn't nothing first before stopping scrolling
        else if (talkingSFX != null)
        {
            audioManager.DestroyAudioInstance(talkingSFX);
            isScrolling = false;
        }
        if ((scrollingText != fullText) && (fixedUpdateTimer == scrollSpeed))
        {
            isScrolling = true;
            scrollingText += fullText[currentCharacter];
            dialogueText.text = speakerName + scrollingText;
            currentCharacter++;
        }
        if (fixedUpdateTimer == scrollSpeed)
        {
            fixedUpdateTimer = 0;
        }
        
        // handles fadeout of dialogue UI
        if (fading)
        {
            fadeTimer++;
            if (fadeTimer == 101)
            {
                introBackground.style.display = DisplayStyle.None;
                audioManager.DestroyAudioInstance(bgm);
                gameManager.StartLevel(GameManager.CurrentLevel);
                gameObject.SetActive(false);
            }
        }
    }
    
    private void IterateDialogue(int num)
    {
        switch (num)
        {
            case 0:
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/fuckass_talk"));
                dialoguePortrait.image = fuckassPortrait;
                speakerName = "?: ";
                fullText = "oh god what the fuck i was just granted consciousness what the fuck who am i. who are you";
                break;
            case 1:
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/wiseguy_talk"));
                dialoguePortrait.image = wiseguyPortrait;
                speakerName = "WISEGUY: ";
                fullText = "Im the WISEGUY. Guy for alllllll the things wise.";
                break;
            case 2:
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/fuckass_talk"));
                dialoguePortrait.image = fuckassPortrait;
                speakerName = "?: ";
                fullText = "uhhhhhhh huh";
                break;
            case 3:
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/wiseguy_talk"));
                dialoguePortrait.image = wiseguyPortrait;
                speakerName = "WISEGUY: ";
                fullText = "I see u instaled the VIDEO GAME! Enter you’re name.";
                break; 
            case 4:
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/fuckass_talk"));
                dialoguePortrait.image = fuckassPortrait;
                speakerName = "?: ";
                fullText = "if you put a gun to my head i could not tell you my name dude im ngl";
                break;
            case 5:
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/wiseguy_talk"));
                dialoguePortrait.image = wiseguyPortrait;
                speakerName = "WISEGUY: ";
                fullText = "Enter you’re name. Enter you’re name. Enter you’re name.";
                break; 
            case 6:
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/fuckass_talk"));
                dialoguePortrait.image = fuckassPortrait;
                speakerName = "?: ";
                fullText = "alright alright fine jesus christ";
                break;
            case 7:
                dialoguePortrait.image = wiseguyPortrait;
                speakerName = "WISEGUY: ";
                fullText = "";
                dialogueText.text = "";
                nameEntryActive = true;
                textBox.style.display = DisplayStyle.None;
                nameEntryBox.style.display = DisplayStyle.Flex;
                break;
            case 8:
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/wiseguy_talk"));
                dialoguePortrait.image = wiseguyPortrait;
                speakerName = "WISEGUY: ";
                fullText = "Thank you,,,,,,, FUCKASS! Now i will tell you about this wondrous world we live in. In case you yknow MISSED it or something in all you’re years of being alive.";
                textBox.style.display = DisplayStyle.Flex;
                nameEntryBox.style.display = DisplayStyle.None;
                break; 
            case 9:
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/fuckass_talk"));
                dialoguePortrait.image = fuckassPortrait;
                speakerName = "FUCKASS: ";
                fullText = "pretty sure that’s not what i entered at all but okay";
                break;
            case 10:
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/wiseguy_talk"));
                dialoguePortrait.image = wiseguyPortrait;
                speakerName = "WISEGUY: ";
                fullText = "Their’s two fuckers you have to worry about. GUYS and DUDES. Were the “Guys”. ";
                break; 
            case 11:
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/wiseguy_talk"));
                dialoguePortrait.image = wiseguyPortrait;
                speakerName = "WISEGUY: ";
                fullText = "The “Dudes” are shaped like poly gones and will go down the lane to enter your base. Spendingng points will let you buy “Guys”. you’re “Guys” will shoot the “Dudes” with bullets. Yeas.";
                break; 
            case 12:
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/fuckass_talk"));
                dialoguePortrait.image = fuckassPortrait;
                speakerName = "FUCKASS: ";
                fullText = "so it’s like bl-";
                break;
            case 13:
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/wiseguy_talk"));
                dialoguePortrait.image = wiseguyPortrait;
                speakerName = "WISEGUY: ";
                fullText = "Its not at all like bloons tower defense.";
                break; 
            case 14:
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/fuckass_talk"));
                dialoguePortrait.image = fuckassPortrait;
                speakerName = "FUCKASS: ";
                fullText = "...";
                break;
            case 15:
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/fuckass_talk"));
                dialoguePortrait.image = fuckassPortrait;
                speakerName = "FUCKASS: ";
                fullText = "ok";
                break;
            case 16:
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/wiseguy_talk"));
                dialoguePortrait.image = wiseguyPortrait;
                speakerName = "WISEGUY: ";
                fullText = "The time has comef,,,,,,,, GO,,,,,,";
                break; 
            case 17:
                bgm.GetComponent<StudioEventEmitter>().SetParameter("Fadeout", 1);
                fade.style.display = DisplayStyle.Flex;
                fade.AddToClassList("fade-out");
                fading = true;
                GameManager.CurrentLevel = 1;
                break;
        }
    }
}
