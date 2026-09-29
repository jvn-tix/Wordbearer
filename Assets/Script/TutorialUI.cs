using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    public static TutorialUI Instance { get; private set; }

    [Header("UI References")]
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

    // UBAH MENJADI STATIC AGAR STATUS PROGRESS TERPADA JIKA RELOAD SCENE / NEXT DAY
    public static QuestStep currentStep = QuestStep.EnterOffice;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Auto-find jika reference di Inspector tidak di-drag manual
        FindUIReferences();

        // JIKA TUTORIAL SUDAH SELESAI (MISAL DI NEXT DAY), LANGSUNG SEMBUNYIKAN PANEL
        if (currentStep == QuestStep.TutorialComplete)
        {
            HidePanelImmediately();
        }
        else
        {
            UpdateQuestTextUI();
        }
    }

    public void FindUIReferences()
    {
        if (questCanvasGroup == null || questText == null)
        {
            GameObject panelObj = GameObject.FindWithTag("TutorialPanel");
            if (panelObj != null)
            {
                if (questCanvasGroup == null) questCanvasGroup = panelObj.GetComponent<CanvasGroup>();
                if (questText == null) questText = panelObj.GetComponentInChildren<TextMeshProUGUI>();
            }
        }
    }

    public void UpdateQuestStep(QuestStep newStep)
    {
        currentStep = newStep;

        if (currentStep == QuestStep.TutorialComplete)
        {
            if (gameObject.activeInHierarchy && questCanvasGroup != null)
            {
                StartCoroutine(HideQuestPanel());
            }
            else
            {
                HidePanelImmediately();
            }
        }
        else
        {
            UpdateQuestTextUI();
        }
    }

    private void UpdateQuestTextUI()
    {
        FindUIReferences();

        if (currentStep == QuestStep.TutorialComplete)
        {
            HidePanelImmediately();
            return;
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

        HidePanelImmediately();
    }

    private void HidePanelImmediately()
    {
        if (questCanvasGroup != null)
        {
            questCanvasGroup.alpha = 0f;
            questCanvasGroup.blocksRaycasts = false;
            questCanvasGroup.gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public QuestStep GetCurrentQuestStep()
    {
        return currentStep;
    }
}