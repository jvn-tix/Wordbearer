using System.Collections;
using UnityEngine;

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
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Sembunyikan panel di awal secara aman via CanvasGroup Alpha
        InitCanvasGroup(successCanvasGroup);
        InitCanvasGroup(failCanvasGroup);
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
        if (successCanvasGroup != null)
        {
            StopAllCoroutines();
            StartCoroutine(FadeRoutine(successCanvasGroup));
        }
    }

    public void ShowFailNotification()
    {
        if (failCanvasGroup != null)
        {
            StopAllCoroutines();
            StartCoroutine(FadeRoutine(failCanvasGroup));
        }
    }

    private IEnumerator FadeRoutine(CanvasGroup cg)
    {
        // 1. Fade In (Muncul)
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            cg.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null;
        }
        cg.alpha = 1f;

        // 2. Tahan Sebentar di Layar
        yield return new WaitForSeconds(displayDuration);

        // 3. Fade Out (Hilang)
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            cg.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            yield return null;
        }
        cg.alpha = 0f;
    }
}