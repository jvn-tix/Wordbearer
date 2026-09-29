using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NotificationUI : MonoBehaviour
{
    public static NotificationUI Instance;

    [Header("Notification UI Panels")]
    [SerializeField] private CanvasGroup successCanvasGroup; // Drag CanvasGroup dari SuccessPanel
    [SerializeField] private CanvasGroup failCanvasGroup;    // Drag CanvasGroup dari FailPanel

    [Header("Notification Timings")]
    [SerializeField] private float fadeDuration = 0.4f;    // Durasi Fade In & Fade Out
    [SerializeField] private float displayDuration = 1.5f; // Durasi Notifikasi Diam di Layar

    private void Awake()
    {
        // Setup Singleton agar NotificationUI mudah dipanggil dari mana saja
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
        // Hentikan coroutine saat script disable agar tidak mengeksekusi UI yang sudah destroyed
        StopAllCoroutines();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Hentikan coroutine dari scene sebelumnya
        StopAllCoroutines();

        // Bersihkan reference scene lama
        successCanvasGroup = null;
        failCanvasGroup = null;

        FindNotificationPanels();
    }

    public void FindNotificationPanels()
    {
        GameObject successObj = GameObject.FindWithTag("SuccessPanel");
        if (successObj != null)
        {
            successCanvasGroup = successObj.GetComponent<CanvasGroup>();
            InitCanvasGroup(successCanvasGroup);
        }

        GameObject failObj = GameObject.FindWithTag("FailPanel");
        if (failObj != null)
        {
            failCanvasGroup = failObj.GetComponent<CanvasGroup>();
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
            cg.gameObject.SetActive(true); // GameObject-nya harus tetap Active
        }
    }

    public void ShowSuccessNotification()
    {
        // Cari ulang jika reference missing/null
        if (successCanvasGroup == null) FindNotificationPanels();

        if (successCanvasGroup != null)
        {
            StopAllCoroutines();
            StartCoroutine(FadeRoutine(successCanvasGroup));
        }
    }

    public void ShowFailNotification()
    {
        // Cari ulang jika reference missing/null
        if (failCanvasGroup == null) FindNotificationPanels();

        if (failCanvasGroup != null)
        {
            StopAllCoroutines();
            StartCoroutine(FadeRoutine(failCanvasGroup));
        }
    }

    private IEnumerator FadeRoutine(CanvasGroup cg)
    {
        if (cg == null) yield break;

        // 1. Fade In (Muncul)
        float timer = 0f;
        while (timer < fadeDuration)
        {
            // Null check setiap frame agar tidak throw MissingReferenceException saat scene reload
            if (cg == null) yield break;

            timer += Time.deltaTime;
            cg.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null;
        }

        if (cg == null) yield break;
        cg.alpha = 1f;

        // 2. Tahan Sebentar di Layar
        yield return new WaitForSeconds(displayDuration);

        // 3. Fade Out (Hilang)
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