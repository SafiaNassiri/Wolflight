using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

// Manages a simple cutscene with typewriter text, optional music, and automatic or manual progression through story segments.
public class CutsceneManager : MonoBehaviour
{
    public TextMeshProUGUI storyText;                       // Text field showing cutscene story
    public GameObject continuePrompt;                       // UI prompt to show "Press Space to continue"
    [TextArea(5, 15)]
    public string[] segments;                               // Array of story segments to display sequentially
    public float typeSpeed = 0.03f;                         // Delay between each character (typewriter effect)
    public float segmentDelay = 2f;                         // Wait time after typing before allowing continuation
    public AudioClip musicForThisCutscene;                  // Optional music to play during cutscene
    public bool stopMusicOnFinish = true;                   // Stop music when cutscene ends
    public int nextSceneIndex = SceneIndex.MAIN_LEVEL;      // Scene to load after cutscene

    private int currentSegment = 0;                         // Tracks which segment is currently active
    private bool isTyping;                                  // True while typewriter effect is in progress
    private bool canContinue;                               // True if player can press space to advance

    void Start()
    {
        // Hide "continue" prompt initially
        if (continuePrompt) continuePrompt.SetActive(false);

        // Play music if assigned
        if (musicForThisCutscene && AudioManager.Instance)
            AudioManager.Instance.PlayMusic(musicForThisCutscene);

        // Start the cutscene routine
        StartCoroutine(PlayRoutine());
    }

    void Update()
    {
        // Only check input if player can continue and keyboard is available
        if (!canContinue || UnityEngine.InputSystem.Keyboard.current == null)
            return;

        // Detect spacebar press for progression
        if (UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (isTyping)
                SkipCurrentSegment();   // Finish typing instantly
            else
                Advance();              // Move to next segment
        }
    }

    // Initial cutscene routine, small delay then show first segment
    IEnumerator PlayRoutine()
    {
        yield return new WaitForSeconds(1f);         // Optional pre-cutscene pause
        yield return StartCoroutine(ShowSegment());
    }

    // Handles showing a single story segment, including typewriter and delays
    IEnumerator ShowSegment()
    {
        // If all segments finished, end cutscene
        if (currentSegment >= segments.Length)
        {
            Finish();
            yield break;
        }

        // Hide the continue prompt while typing
        if (continuePrompt) continuePrompt.SetActive(false);

        // Type out the text
        yield return StartCoroutine(TypeText(segments[currentSegment]));

        // Wait a bit after finishing typing
        yield return new WaitForSeconds(segmentDelay);

        // Show "press space to continue" prompt
        if (continuePrompt) continuePrompt.SetActive(true);
        canContinue = true;
    }

    // Typewriter effect for a single segment
    IEnumerator TypeText(string txt)
    {
        isTyping = true;
        storyText.text = "";

        foreach (char c in txt)
        {
            storyText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
    }

    // Instantly finish typing the current segment
    void SkipCurrentSegment()
    {
        StopAllCoroutines();
        storyText.text = segments[currentSegment];
        isTyping = false;

        // Show the segment and allow continuation
        StartCoroutine(ShowSegment());
    }

    // Move to the next story segment
    void Advance()
    {
        canContinue = false;
        currentSegment++;
        StartCoroutine(ShowSegment());
    }

    // Ends the cutscene and loads the next scene
    void Finish()
    {
        // Stop music if desired
        if (stopMusicOnFinish && AudioManager.Instance)
            AudioManager.Instance.StopMusic();

        // Load the next scene
        SceneManager.LoadScene(nextSceneIndex);
    }
}
