using UnityEngine;
using TMPro;

public class RuneCounterUI : MonoBehaviour
{
    public static RuneCounterUI Instance { get; private set; }

    [Header("UI References")]
    public TextMeshProUGUI runeCountText;
    public GameObject runeIcon;

    [Header("Animation Settings")]
    public bool animateOnCollect = true;
    public float punchScale = 1.3f;
    public float animDuration = 0.3f;

    private Vector3 originalScale;
    private int lastCount = 0;

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
        if (runeCountText != null)
        {
            originalScale = runeCountText.transform.localScale;
        }

        UpdateDisplay();
    }

    void Update()
    {
        // Check if rune count changed
        if (Rune.runesCollected != lastCount)
        {
            lastCount = Rune.runesCollected;
            UpdateDisplay();

            if (animateOnCollect)
            {
                AnimatePunch();
            }
        }
    }

    void UpdateDisplay()
    {
        if (runeCountText != null)
        {
            runeCountText.text = $"{Rune.runesCollected}/{Rune.totalRunesNeeded}";
        }
    }

    void AnimatePunch()
    {
        if (runeCountText == null) return;

        StopAllCoroutines();
        StartCoroutine(PunchAnimation());
    }

    System.Collections.IEnumerator PunchAnimation()
    {
        float elapsed = 0f;

        // Scale up
        while (elapsed < animDuration / 2)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / (animDuration / 2);
            runeCountText.transform.localScale = Vector3.Lerp(originalScale, originalScale * punchScale, progress);
            yield return null;
        }

        elapsed = 0f;

        // Scale back down
        while (elapsed < animDuration / 2)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / (animDuration / 2);
            runeCountText.transform.localScale = Vector3.Lerp(originalScale * punchScale, originalScale, progress);
            yield return null;
        }

        runeCountText.transform.localScale = originalScale;
    }
}