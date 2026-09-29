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
            // Ambil nama tujuan dari surat dan nama kotak pos (dengan penanganan null)
            string targetSurat = activeSlot.itemInSlot.targetHouse;
            string namaKotakPos = (backpackManager.currentMailbox != null) ? backpackManager.currentMailbox.houseID : "";

            // Pengecekan Alamat
            if (targetSurat == namaKotakPos)
            {
                backpackManager.ChangeRating(10);
                if (NotificationUI.Instance != null) NotificationUI.Instance.ShowSuccessNotification();

                if (TutorialUI.Instance != null && TutorialUI.Instance.GetCurrentQuestStep() == TutorialUI.QuestStep.DeliverMails)
                {
                    TutorialUI.Instance.UpdateQuestStep(TutorialUI.QuestStep.TutorialComplete);
                }
                Debug.Log("PENGIRIMAN SUKSES! Rating Naik.");
            }
            else
            {
                backpackManager.ChangeRating(-5);
                if (NotificationUI.Instance != null) NotificationUI.Instance.ShowFailNotification();
                Debug.Log("SALAH ALAMAT! Rating Turun.");
            }

            activeSlot.ClearItem();
            ClearDescription();

            // 1. Tambah jumlah surat terkirim (akses static langsung lewat nama Class)
            BackpackManager.deliveredMailsCount++;

            // 2. Cek apakah semua surat hari ini sudah selesai diantar
            if (backpackManager.IsAllMailsProcessed())
            {
                int finalDelivered = BackpackManager.deliveredMailsCount;
                int totalMails = BackpackManager.totalMailsToday;
                int currentRating = backpackManager.GetRating();

                // Tampilkan Laporan End Of Day
                if (EndOfDayUI.Instance != null)
                {
                    EndOfDayUI.Instance.ShowEndOfDayReport(finalDelivered, totalMails, currentRating);
                }
            }

            Debug.Log($"[DELIVER] Terkirim: {BackpackManager.deliveredMailsCount} / Target: {BackpackManager.totalMailsToday}");

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