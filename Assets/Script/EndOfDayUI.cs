using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndOfDayUI : MonoBehaviour
{
    public static EndOfDayUI Instance;

    [Header("UI References")]
    [SerializeField] private CanvasGroup endOfDayCanvasGroup;
    [SerializeField] private TextMeshProUGUI deliveredCountText;
    [SerializeField] private TextMeshProUGUI finalRatingText;
    [SerializeField] private Button nextDayButton;

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 0.5f;

    private int totalDelivered = 0;
    private int maxMails = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Jika mau bertahan antar scene, un-comment baris di bawah:
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitUI();
    }

    private void InitUI()
    {
        if (endOfDayCanvasGroup != null)
        {
            // WAJIB: Pastikan GameObject aktif dulu agar Coroutine bisa jalan!
            endOfDayCanvasGroup.gameObject.SetActive(true);
            endOfDayCanvasGroup.alpha = 0f;
            endOfDayCanvasGroup.blocksRaycasts = false;
            endOfDayCanvasGroup.interactable = false;
        }

        if (nextDayButton != null)
        {
            nextDayButton.onClick.RemoveAllListeners();
            nextDayButton.onClick.AddListener(OnNextDayClicked);
        }
    }

    // Panggil fungsi ini saat hari/shift selesai
    public void ShowEndOfDayReport(int delivered, int totalMails, int currentRating)
    {
        totalDelivered = delivered;
        maxMails = totalMails;

        if (deliveredCountText != null)
            deliveredCountText.text = $"Surat Terkirim: {totalDelivered} / {maxMails}";

        if (finalRatingText != null)
            finalRatingText.text = $"Total Rating: {currentRating}%";

        // Pastikan GameObject aktif sebelum mulai Coroutine
        if (endOfDayCanvasGroup != null)
        {
            endOfDayCanvasGroup.gameObject.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(FadeInPanel());
        }
        else
        {
            Debug.LogError("[EndOfDayUI] CanvasGroup belum di-assign di Inspector!");
        }
    }

    private IEnumerator FadeInPanel()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            if (endOfDayCanvasGroup != null)
            {
                endOfDayCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            }
            yield return null;
        }

        if (endOfDayCanvasGroup != null)
        {
            endOfDayCanvasGroup.alpha = 1f;
            endOfDayCanvasGroup.blocksRaycasts = true;
            endOfDayCanvasGroup.interactable = true;
        }
    }

    private void OnNextDayClicked()
    {
        if (SceneFader.Instance != null)
        {
            SceneFader.Instance.FadeToScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}