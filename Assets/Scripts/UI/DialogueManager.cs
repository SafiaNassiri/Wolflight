using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [Header("Settings")]
    public float typeSpeed = 0.05f;
    public float displayDuration = 3f;

    private bool isDialogueActive = false;
    private bool isTyping = false;
    private string currentFullText = "";
    private Coroutine dialogueCoroutine;

    void Awake()
    {
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
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (!isDialogueActive) return;

        // Skip typing animation using new Input System
        if (UnityEngine.InputSystem.Keyboard.current != null &&
            UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame &&
            isTyping)
        {
            SkipTypewriter();
        }
    }

    public void ShowDialogue(string text)
    {
        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
        }

        dialogueCoroutine = StartCoroutine(ShowDialogueSequence(text));
    }

    IEnumerator ShowDialogueSequence(string text)
    {
        isDialogueActive = true;
        dialoguePanel.SetActive(true);

        currentFullText = text;

        yield return StartCoroutine(TypewriterEffect(text));
        yield return new WaitForSeconds(displayDuration);

        HideDialogue();
    }

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

    void SkipTypewriter()
    {
        StopAllCoroutines();
        dialogueText.text = currentFullText;
        isTyping = false;

        dialogueCoroutine = StartCoroutine(WaitAndHide());
    }

    IEnumerator WaitAndHide()
    {
        yield return new WaitForSeconds(displayDuration);
        HideDialogue();
    }

    void HideDialogue()
    {
        isDialogueActive = false;
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }
}