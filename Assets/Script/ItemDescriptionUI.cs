using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDescriptionUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public Image iconImage;
    public GameObject deliveryButton;

    private BackpackManager backpackManager;
    private SlotUI activeSlot;

    private void Start()
    {
        backpackManager = FindFirstObjectByType<BackpackManager>();
        if (deliveryButton != null) deliveryButton.SetActive(false);
    }

    public void UpdateDescription(SlotUI slot)
    {
        if (slot != null && slot.itemInSlot != null)
        {
            activeSlot = slot;
            ItemData item = slot.itemInSlot;

            nameText.text = item.itemName;
            descriptionText.text = item.itemDescription;
            iconImage.sprite = item.itemIcon;
            iconImage.gameObject.SetActive(true);

            if (backpackManager != null && backpackManager.isDeliveryMode)
            {
                deliveryButton.SetActive(true);
            }
            else
            {
                deliveryButton.SetActive(false);
            }
        }
        else
        {
            ClearDescription();
        }
    }

    public void DeliverItem()
    {
        if (activeSlot != null && activeSlot.itemInSlot != null && backpackManager != null)
        {
            ItemData currentItem = activeSlot.itemInSlot;

            string targetSurat = currentItem.targetHouse;
            string namaKotakPos = (backpackManager.currentMailbox != null) ? backpackManager.currentMailbox.houseID : "";

            bool isCorrectAddress = (targetSurat == namaKotakPos);
            float timeTaken = Time.time - currentItem.timePickedUp;

            bool isLongDistance = false;
            if (backpackManager.currentMailbox != null)
            {
                isLongDistance = backpackManager.currentMailbox.CheckIsLongDistance();
            }

            // --- HITUNG RATING DAN SIMPAN BESARAN PERUBAHANNYA ---
            int ratingBefore = BackpackManager.playerRating;
            backpackManager.CalculateAndApplyRating(isCorrectAddress, timeTaken, isLongDistance);
            int ratingChange = BackpackManager.playerRating - ratingBefore; // Selisih poin yang didapat/hilang

            // Tampilkan Notifikasi Dinamis
            if (isCorrectAddress)
            {
                if (NotificationUI.Instance != null)
                    NotificationUI.Instance.ShowSuccessNotification(Mathf.Abs(ratingChange)); // Kirim angka perolehan

                if (TutorialUI.Instance != null && TutorialUI.Instance.GetCurrentQuestStep() == TutorialUI.QuestStep.DeliverMails)
                {
                    TutorialUI.Instance.UpdateQuestStep(TutorialUI.QuestStep.TutorialComplete);
                }
            }
            else
            {
                if (NotificationUI.Instance != null)
                    NotificationUI.Instance.ShowFailNotification(ratingChange); // Kirim angka penalti (minus)
            }

            activeSlot.ClearItem();
            ClearDescription();

            BackpackManager.deliveredMailsCount++;

            if (backpackManager.IsAllMailsProcessed())
            {
                int finalDelivered = BackpackManager.deliveredMailsCount;
                int totalMails = BackpackManager.totalMailsToday;
                int currentRating = backpackManager.GetRating();

                if (EndOfDayUI.Instance != null)
                {
                    EndOfDayUI.Instance.ShowEndOfDayReport(finalDelivered, totalMails, currentRating);
                }
            }

            backpackManager.CloseBackpack();
        }
    }

    public void ClearDescription()
    {
        activeSlot = null;
        if (nameText != null) nameText.text = "";
        if (descriptionText != null) descriptionText.text = "";
        if (iconImage != null) iconImage.gameObject.SetActive(false);
        if (deliveryButton != null) deliveryButton.SetActive(false);
    }
}