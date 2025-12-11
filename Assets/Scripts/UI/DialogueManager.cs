using UnityEngine;
using TMPro;
using System.Collections;

// Handles displaying short popup-style dialogue with a typewriter effect.
// Supports skipping the typewriter animation and auto-hides the panel after a set delay.
public class DialogueManager : MonoBehaviour
{
    // Global access point for triggering dialogue anywhere in the game.
    public static DialogueManager Instance { get; private set; }

    public GameObject dialoguePanel;        // Container panel for the dialogue UI
    public TextMeshProUGUI dialogueText;    // Text field where the dialogue appears

    public float typeSpeed = 0.05f;         // Delay between typed characters
    public float displayDuration = 3f;      // How long the full text stays visible

    private bool isDialogueActive = false;  // True when dialogue is on screen
    private bool isTyping = false;          // True when typewriter effect is running
    private string currentFullText = "";    // Stores full dialogue for skipping
    private Coroutine dialogueCoroutine;    // Tracks the running dialogue sequence

    void Awake()
    {
        // Only one active instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Hide panel initially to avoid showing empty UI at start
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (!isDialogueActive) return;

        // Handle skipping the typewriter animation
        if (UnityEngine.InputSystem.Keyboard.current != null &&
            UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame &&
            isTyping)
        {
            SkipTypewriter();
        }
    }

    // Public entry point to show dialogue.
    // Cancels any existing dialogue sequence and starts a new one.
    public void ShowDialogue(string text)
    {
        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
        }

        dialogueCoroutine = StartCoroutine(ShowDialogueSequence(text));
    }

    // Show UI, type out text, wait, then hide it
    IEnumerator ShowDialogueSequence(string text)
    {
        isDialogueActive = true;
        dialoguePanel.SetActive(true);

        currentFullText = text;

        // Play typewriter animation
        yield return StartCoroutine(TypewriterEffect(text));

        // When finished, keep the text on screen for a while
        yield return new WaitForSeconds(displayDuration);

        HideDialogue();
    }

    // Reveals the text one character at a time.
    IEnumerator TypewriterEffect(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
    }

    // Instantly shows the full text and cancels typing.
    // Triggered when the player presses Space during typing.
    void SkipTypewriter()
    {
        StopAllCoroutines();                   // Stop typing immediately
        dialogueText.text = currentFullText;   // Show full text
        isTyping = false;

        // Start countdown to hide text
        dialogueCoroutine = StartCoroutine(WaitAndHide());
    }

    // Waits the normal display duration (post-skip) before hiding.
    IEnumerator WaitAndHide()
    {
        yield return new WaitForSeconds(displayDuration);
        HideDialogue();
    }

    // Turns off dialogue UI and resets flags.
    void HideDialogue()
    {
        isDialogueActive = false;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }
}
