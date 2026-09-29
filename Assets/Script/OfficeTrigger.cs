using UnityEngine;

public class OfficeTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (TutorialUI.Instance != null &&
                TutorialUI.Instance.GetCurrentQuestStep() == TutorialUI.QuestStep.EnterOffice)
            {
                TutorialUI.Instance.UpdateQuestStep(TutorialUI.QuestStep.PickUpMails);
            }
        }
    }
}