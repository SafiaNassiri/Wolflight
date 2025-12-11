using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI storyText;
    public GameObject continuePrompt;          // "Press SPACE to continue"

    [Header("Story")]
    [TextArea(5, 15)]
    public string[] segments;

    [Header("Timing")]
    public float typeSpeed = 0.03f;
    public float segmentDelay = 2f;

    [Header("Audio")]
    public AudioClip musicForThisCutscene;     // drag openingCutsceneMusic here
    public bool stopMusicOnFinish = true;

    [Header("Progression")]
    public string nextSceneName = "MainLevel"; // whatever comes after this cutscene

    private int currentSegment = 0;
    private bool isTyping;
    private bool canContinue;

    /* --------------------------------------------------------------------- */

    void Start()
    {
        if (continuePrompt) continuePrompt.SetActive(false);

        // fire music
        if (musicForThisCutscene && AudioManager.Instance)
            AudioManager.Instance.PlayMusic(musicForThisCutscene);

        StartCoroutine(PlayRoutine());
    }

    void Update()
    {
        if (!canContinue || UnityEngine.InputSystem.Keyboard.current == null) return;

        if (UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (isTyping) { SkipCurrentSegment(); }
            else { Advance(); }
        }
    }

    /* --------------------------------------------------------------------- */

    IEnumerator PlayRoutine()
    {
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(ShowSegment());
    }

    IEnumerator ShowSegment()
    {
        if (currentSegment >= segments.Length) { Finish(); yield break; }

        if (continuePrompt) continuePrompt.SetActive(false);

        yield return StartCoroutine(TypeText(segments[currentSegment]));

        yield return new WaitForSeconds(segmentDelay);

        if (continuePrompt) continuePrompt.SetActive(true);
        canContinue = true;
    }

    IEnumerator TypeText(string txt)
    {
        isTyping = true;
        storyText.text = "";
        foreach (char c in txt) { storyText.text += c; yield return new WaitForSeconds(typeSpeed); }
        isTyping = false;
    }

    void SkipCurrentSegment()
    {
        StopAllCoroutines();
        storyText.text = segments[currentSegment];
        isTyping = false;
        StartCoroutine(ShowSegment());
    }

    void Advance()
    {
        canContinue = false;
        currentSegment++;
        StartCoroutine(ShowSegment());
    }

    void Finish()
    {
        if (stopMusicOnFinish) AudioManager.Instance.StopMusic();
        SceneManager.LoadScene(nextSceneName);
    }
}