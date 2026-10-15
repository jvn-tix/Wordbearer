using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Pastikan namespace ini ada

public class NotificationUI : MonoBehaviour
{
    public static NotificationUI Instance;

    [Header("Notification UI Panels")]
    [SerializeField] private CanvasGroup successCanvasGroup;
    [SerializeField] private CanvasGroup failCanvasGroup;

    [Header("Text References (Opsional untuk menampilkan angka)")]
    [SerializeField] private TextMeshProUGUI successText; // Drag teks di dalam SuccessPanel
    [SerializeField] private TextMeshProUGUI failText;    // Drag teks di dalam FailPanel

    [Header("Notification Timings")]
    [SerializeField] private float fadeDuration = 0.4f;
    [SerializeField] private float displayDuration = 1.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitCanvasGroup(successCanvasGroup);
        InitCanvasGroup(failCanvasGroup);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        StopAllCoroutines();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StopAllCoroutines();
        successCanvasGroup = null;
        failCanvasGroup = null;
        successText = null;
        failText = null;

        FindNotificationPanels();
    }

    public void FindNotificationPanels()
    {
        GameObject successObj = GameObject.FindWithTag("SuccessPanel");
        if (successObj != null)
        {
            successCanvasGroup = successObj.GetComponent<CanvasGroup>();
            successText = successObj.GetComponentInChildren<TextMeshProUGUI>();
            InitCanvasGroup(successCanvasGroup);
        }

        GameObject failObj = GameObject.FindWithTag("FailPanel");
        if (failObj != null)
        {
            failCanvasGroup = failObj.GetComponent<CanvasGroup>();
            failText = failObj.GetComponentInChildren<TextMeshProUGUI>();
            InitCanvasGroup(failCanvasGroup);
        }
    }

    private void InitCanvasGroup(CanvasGroup cg)
    {
        if (cg != null)
        {
            cg.alpha = 0f;
            cg.blocksRaycasts = false;
            cg.interactable = false;
            cg.gameObject.SetActive(true);
        }
    }

    // --- FUNGSI BARU DENGAN PARAMETER ANGKA RATING ---
    public void ShowSuccessNotification(int ratingGained)
    {
        if (successCanvasGroup == null) FindNotificationPanels();

        if (successText != null)
        {
            successText.text = $"Pengiriman Berhasil! (+{ratingGained}%)";
        }

        if (successCanvasGroup != null)
        {
            StopAllCoroutines();
            StartCoroutine(FadeRoutine(successCanvasGroup));
        }
    }

    public void ShowFailNotification(int ratingLost)
    {
        if (failCanvasGroup == null) FindNotificationPanels();

        if (failText != null)
        {
            failText.text = $"Salah Alamat! ({ratingLost}%)";
        }

        if (failCanvasGroup != null)
        {
            StopAllCoroutines();
            StartCoroutine(FadeRoutine(failCanvasGroup));
        }
    }

    private IEnumerator FadeRoutine(CanvasGroup cg)
    {
        if (cg == null) yield break;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            if (cg == null) yield break;
            timer += Time.deltaTime;
            cg.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null;
        }

        if (cg == null) yield break;
        cg.alpha = 1f;

        yield return new WaitForSeconds(displayDuration);

        timer = 0f;
        while (timer < fadeDuration)
        {
            if (cg == null) yield break;
            timer += Time.deltaTime;
            cg.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            yield return null;
        }

        if (cg != null)
        {
            cg.alpha = 0f;
        }
    }
}