using System;
using System.Collections.Generic;
using System.Text;
using FMOD;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class Dialogue : MonoBehaviour
{
    public bool debugging;
    
    [SerializeField] private Texture fuckassPortrait;
    [SerializeField] private Texture wiseguyPortrait;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private BattleUI battleUI;
    private Label dialogueText;
    private Image dialoguePortrait;
    private VisualElement textBox;
    private VisualElement nameEntryBox;
    private Button nameEntryConfirm;
    private TextField textField;
    private VisualElement fade;
    private VisualElement introBackground;
    
    // Objects to reference when creating sounds
    private GameObject talkingSFX;
    private StudioEventEmitter bgm;
    private StudioEventEmitter realization;
    
    // Scrolling Dialogue
    private string speakerName;
    private string scrollingText; // dialogue displayed character by character
    private string fullText; // actual line to check to know when to stop iterating
    private int currentCharacter;
    [SerializeField] private int currentLine; // which dialogue switch case is selected
    private bool isScrolling;
    private bool nameEntryActive;
    public bool FinalLine;
    
    // Scene changing
    private bool fading;
    
    // Scroll Speed
    private int fixedUpdateTimer = 0;
    public float scrollSpeed = 2f; // higher number means slower. i might try to fix this later but idk how
    private bool skippable = true;
    
    // End Game
    private bool endGame;
    private int endGameTimer;
    
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
        nameEntryBox.style.display = DisplayStyle.None;
        
        // initializing variables for dialogue
        if (!debugging)
        {
            if (SceneManager.GetActiveScene().name == "Level 1")
            {
                currentLine = 0;
            }

            if (SceneManager.GetActiveScene().name == "Level 2")
            {
                currentLine = 18;
            }

            if (SceneManager.GetActiveScene().name == "Level 3")
            {
                currentLine = 28;
            }
        }
        IterateDialogue(currentLine);

        // setting up the scene's audio
        bgm = gameObject.AddComponent<StudioEventEmitter>();
        bgm.EventReference = RuntimeManager.PathToEventReference("event:/Music/wiseguy_theme");
        bgm.Play();
        
        realization = gameObject.AddComponent<StudioEventEmitter>();
        realization.EventReference = RuntimeManager.PathToEventReference("event:/Music/realization");
        
        // dialogue does not appear upon level restart
        if (GameManager.LevelRestarted)
        {
            GameManager.LevelRestarted = false;
            gameObject.SetActive(false);
        }
    }

    
    void Update()
    {
        if (Mouse.current.leftButton.wasReleasedThisFrame && skippable)
        {
            // skipping dialogue
            if (isScrolling)
            {
                dialogueText.text = speakerName + fullText;
                scrollingText = fullText;
                isScrolling = false;
                // Quits game if its the final line
                if (endGame)
                {
                    Application.Quit();
                }
            }
            // normal handling as long as name entry isn't active
            else if (!nameEntryActive && !FinalLine)
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
            skippable = true;
        }
        if ((scrollingText != fullText) && (fixedUpdateTimer > scrollSpeed))
        {
            isScrolling = true;
            scrollingText += fullText[currentCharacter];
            dialogueText.text = speakerName + scrollingText;
            currentCharacter++;
        }
        if (fixedUpdateTimer > scrollSpeed)
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
                bgm.Stop();
                gameManager.StartLevel(GameManager.CurrentLevel);
                gameObject.SetActive(false);
            }
        }
        
        // ends game
        if (endGame)
        {
            endGameTimer++;
            if (endGameTimer > 100)
            {
                Application.Quit();
            }
        }
    }
    
    public void IterateDialogue(int num)
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
                FinalLine = true;
                bgm.SetParameter("Fadeout", 1);
                fade.style.display = DisplayStyle.Flex;
                fade.AddToClassList("fade-out");
                fading = true;
                GameManager.CurrentLevel = 1;
                break;
            case 18:
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/wiseguy_talk"));
                dialoguePortrait.image = wiseguyPortrait;
                speakerName = "WISEGUY: ";
                fullText = "Kong rats on not dyeing!!!!!!!! Bye the bye,,, have you noticed the SECRRET!?!?";
                break; 
            case 19:
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/wiseguy_talk"));
                dialoguePortrait.image = wiseguyPortrait;
                speakerName = "WISEGUY: ";
                fullText = "Thats right. If you click on an enemy. It shows you we’re they’re from and there name!!! there people, just liek you or me. Because in THE GREAT DUDEGUY WAR - your the player. and your choice is. and you’re actions have consequences. so you better not FUC UP";
                break; 
            case 20:
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/fuckass_talk"));
                dialoguePortrait.image = fuckassPortrait;
                speakerName = "FUCKASS: ";
                fullText = "ok well thats really stupid its not like youre giving me a choice here i have to kill these polygons to win and you give me money for doing it";
                break; 
            case 21:
                bgm.Stop();
                RuntimeManager.PlayOneShot(RuntimeManager.PathToEventReference("event:/SFX/record_scratch"));
                textBox.style.display = DisplayStyle.None;
                fullText = "";
                dialogueText.text = "";
                break;
            case 22:
                realization.Play();
                textBox.style.display = DisplayStyle.Flex;
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/wiseguy_talk"));
                skippable = false;
                dialoguePortrait.image = wiseguyPortrait;
                scrollSpeed = 2.5f;
                speakerName = "WISEGUY: ";
                fullText = "Have you ever considered that the role of the “player” in the medium of Video Games, and furthermore the forceful inhabitation and control of an individual or collective within a given game universe, is inherently violent and oppressive?";
                break; 
            case 23:
                skippable = false;
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/wiseguy_talk"));
                dialoguePortrait.image = wiseguyPortrait;
                speakerName = "WISEGUY: ";
                fullText = "These people you see before you, or “Dudes” as they are denominated in-canon, could be prisoners of war, hostages, any number of things that would eliminate their agency and render their alleged villainy void — and yet you disregard any such possibilities, stripping them of their humanity.";
                break;
            case 24:
                skippable = false;
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/wiseguy_talk"));
                dialoguePortrait.image = wiseguyPortrait;
                speakerName = "WISEGUY: ";
                fullText = "Just meat into the grinder, pixels without feelings, point fodder to buy more “Guys.” Not to mention, here you are forcing said “Guys” to carry out your every whim, similarly without care toward how they may object to this senseless violence either.";
                break;
            case 25:
                realization.Stop(); 
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/fuckass_talk"));
                scrollSpeed = 1f;
                dialoguePortrait.image = fuckassPortrait;
                speakerName = "FUCKASS: ";
                fullText = "yeah i rly dgaf";
                break;
            case 26:
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/wiseguy_talk"));
                dialoguePortrait.image = wiseguyPortrait;
                speakerName = "WISEGUY: ";
                fullText = "FUCK YOU!!!!!!!!!!!!!!!!!!!!!!!!!!";
                break;
            case 27:
                FinalLine = true;
                fade.style.display = DisplayStyle.Flex;
                bgm.SetParameter("Fadeout", 1);
                fade.AddToClassList("fade-out");
                fading = true;
                GameManager.CurrentLevel = 2;
                break;
            case 28:
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/wiseguy_talk"));
                dialoguePortrait.image = wiseguyPortrait;
                speakerName = "WISEGUY: ";
                fullText = "Fine lee,,,,, were at the end of the jure knee,,,,,,,";
                break;
            case 29:
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/fuckass_talk"));
                dialoguePortrait.image = fuckassPortrait;
                speakerName = "FUCKASS: ";
                fullText = "wow thats it? that literally took all of 5 minutes. if that even";
                break;
            case 30:
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/wiseguy_talk"));
                dialoguePortrait.image = wiseguyPortrait;
                speakerName = "WISEGUY: ";
                fullText = "Did you ever realeyes,,,,,,, in the game title “Dudeguy” is won word?????? thats because, you and i, dudes and guys — we’re won in the same.";
                break;
            case 31:
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/fuckass_talk"));
                dialoguePortrait.image = fuckassPortrait;
                speakerName = "FUCKASS: ";
                fullText = "wow";
                break;
            case 32:
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/fuckass_talk"));
                dialoguePortrait.image = fuckassPortrait;
                speakerName = "FUCKASS: ";
                fullText = " ";
                break;
            case 33:
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/fuckass_talk"));
                dialoguePortrait.image = fuckassPortrait;
                speakerName = "FUCKASS: ";
                fullText = "can i go now";
                break;
            case 34:
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/wiseguy_talk"));
                dialoguePortrait.image = wiseguyPortrait;
                speakerName = "WISEGUY: ";
                fullText = "NO!! YOU HAVENT HEARD ALL THE LORE ABOUT OUR WORLD YET!!!!!!!!!";
                break;
            case 35:
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/fuckass_talk"));
                dialoguePortrait.image = fuckassPortrait;
                speakerName = "FUCKASS: ";
                fullText = "god how do i turn this thing off";
                break;
            case 36:
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/fuckass_talk"));
                dialoguePortrait.image = fuckassPortrait;
                speakerName = "FUCKASS: ";
                fullText = "oh wait";
                break;
            case 37:
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/fuckass_talk"));
                dialoguePortrait.image = fuckassPortrait;
                speakerName = "FUCKASS: ";
                fullText = "yeap found it";
                break;
            case 38:
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/fuckass_talk"));
                dialoguePortrait.image = fuckassPortrait;
                speakerName = "FUCKASS: ";
                fullText = "bye";
                break;
            case 39:
                endGame = true;
                talkingSFX = audioManager.CreateAudioInstance(RuntimeManager.PathToEventReference("event:/SFX/wiseguy_talk"));
                dialoguePortrait.image = wiseguyPortrait;
                speakerName = "WISEGUY: ";
                fullText = "NOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO";
                break;
        }
    }
}
