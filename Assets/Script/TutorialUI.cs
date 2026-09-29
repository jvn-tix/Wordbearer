using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    public static TutorialUI Instance;

    [Header("UI References (Akan terisi otomatis jika pakai Tag / Auto-Find)")]
    [SerializeField] private CanvasGroup questCanvasGroup;
    [SerializeField] private TextMeshProUGUI questText;

    [Header("Timing Settings")]
    [SerializeField] private float fadeDuration = 0.5f;

    public enum QuestStep
    {
        EnterOffice,       // 1. Masuk kantor
        PickUpMails,       // 2. Ambil surat
        DeliverMails,      // 3. Antarkan surat
        TutorialComplete   // 4. Selesai (Sembunyikan UI)
    }

    private QuestStep currentStep = QuestStep.EnterOffice;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Supaya state quest tidak hilang saat pindah scene
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        // Daftarkan event saat scene selesai di-load
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unregister event saat objek dimatikan/dihancurkan
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Fungsi ini dipanggil OTOMATIS setiap kali pindah ke scene baru
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Jika tutorial sudah selesai, tidak perlu mencari UI lagi
        if (currentStep == QuestStep.TutorialComplete) return;

        // Cari UI Tutorial di scene baru yang baru di-load
        FindUIReferences();

        // Perbarui teks UI di scene baru sesuai progres quest saat ini
        UpdateQuestTextUI();
    }

    public void FindUIReferences()
    {
        // Cara 1: Cari objek berdasarkan Tag "TutorialPanel"
        GameObject panelObj = GameObject.FindWithTag("TutorialPanel");
        if (panelObj != null)
        {
            questCanvasGroup = panelObj.GetComponent<CanvasGroup>();
            questText = panelObj.GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    public void UpdateQuestStep(QuestStep newStep)
    {
        currentStep = newStep;
        UpdateQuestTextUI();
    }

    private void UpdateQuestTextUI()
    {
        // Pastikan referensi UI tidak missing sebelum dipakai
        if (questCanvasGroup == null || questText == null)
        {
            FindUIReferences();
        }

        switch (currentStep)
        {
            case QuestStep.EnterOffice:
                if (questText != null) questText.text = "- Enter the post office";
                break;

            case QuestStep.PickUpMails:
                if (questText != null) questText.text = "- Pick up mails from the office";
                break;

            case QuestStep.DeliverMails:
                if (questText != null) questText.text = "- Deliver mails to the correct houses based from the description";
                break;

            case QuestStep.TutorialComplete:
                if (questCanvasGroup != null)
                {
                    StartCoroutine(HideQuestPanel());
                }
                break;
        }
    }

    private IEnumerator HideQuestPanel()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            if (questCanvasGroup != null)
                questCanvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            yield return null;
        }

        if (questCanvasGroup != null)
        {
            questCanvasGroup.alpha = 0f;
            questCanvasGroup.gameObject.SetActive(false);
        }
    }

    public QuestStep GetCurrentQuestStep()
    {
        return currentStep;
    }
}