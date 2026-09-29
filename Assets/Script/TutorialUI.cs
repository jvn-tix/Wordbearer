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
            DontDestroyOnLoad(gameObject); // Bertahan antar-scene sesuai struktur game kamu
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Paksa reset reference UI dari scene lama yang sudah destroyed oleh Unity
        questCanvasGroup = null;
        questText = null;

        // Cari UI di scene baru
        FindUIReferences();

        // Jika tutorial sudah lengkap (misal masuk Next Day), sembunyikan UI di scene baru ini
        if (currentStep == QuestStep.TutorialComplete)
        {
            if (questCanvasGroup != null)
            {
                questCanvasGroup.alpha = 0f;
                questCanvasGroup.blocksRaycasts = false;
                questCanvasGroup.gameObject.SetActive(false);
            }
            return;
        }

        // Jika belum selesai, tampilkan UI dan perbarui teks
        UpdateQuestTextUI();
    }

    public void FindUIReferences()
    {
        // Cari objek berdasarkan Tag "TutorialPanel" di scene yang sedang aktif
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
        // Cek apakah reference missing/null, lalu cari ulang
        if (questCanvasGroup == null || questText == null)
        {
            FindUIReferences();
        }

        if (questCanvasGroup != null)
        {
            questCanvasGroup.alpha = 1f;
            questCanvasGroup.blocksRaycasts = true;
            questCanvasGroup.gameObject.SetActive(true);
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
                if (questText != null) questText.text = "- Deliver mails to the correct houses based from description";
                break;

            case QuestStep.TutorialComplete:
                if (questCanvasGroup != null && gameObject.activeInHierarchy)
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
            questCanvasGroup.blocksRaycasts = false;
            questCanvasGroup.gameObject.SetActive(false);
        }
    }

    public QuestStep GetCurrentQuestStep()
    {
        return currentStep;
    }
}