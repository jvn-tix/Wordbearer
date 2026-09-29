using System.Collections.Generic;
using UnityEngine;

public class MailPickUp : MonoBehaviour, IInteractable
{
    [Header("Mail Settings")]
    public ItemData[] possibleMails;
    public int minPickup = 3;
    public int maxPickup = 9;

    [Header("Interaction Settings")]
    [SerializeField] private string promptMessage = "Press E to Pick Up Mails";

    public string GetPrompt()
    {
        return promptMessage;
    }

    public void Interact()
    {
        PickUpMails();
    }

    private void PickUpMails()
    {
        if (possibleMails == null || possibleMails.Length == 0)
        {
            Debug.LogWarning("Kotak Possible Mails di Inspector masih kosong!");
            return;
        }

        // Ambil instance BackpackManager secara langsung saat interaksi
        BackpackManager backpack = BackpackManager.Instance != null ? BackpackManager.Instance : FindFirstObjectByType<BackpackManager>();

        if (backpack == null)
        {
            Debug.LogError("[MailPickUp] BackpackManager tidak ditemukan di Scene!");
            return;
        }

        // 1. Tentukan berapa banyak surat yang akan diambil (acak antara min dan max)
        int amountToPickup = Random.Range(minPickup, maxPickup + 1);
        amountToPickup = Mathf.Min(amountToPickup, possibleMails.Length);

        backpack.SetDailyMails(amountToPickup);

        // 2. Buat "Salinan" dari daftar surat
        List<ItemData> availableMails = new List<ItemData>(possibleMails);
        int actualAddedCount = 0;

        for (int i = 0; i < amountToPickup; i++)
        {
            int randomIndex = Random.Range(0, availableMails.Count);
            ItemData selectedMail = availableMails[randomIndex];

            // Masukkan ke tas
            backpack.AddItemToInventory(selectedMail);
            actualAddedCount++;

            // Hapus surat dari List agar tidak terduplikasi
            availableMails.RemoveAt(randomIndex);
        }

        // Set target harian sesuai jumlah surat yang benar-benar diambil dan dimasukkan
        backpack.SetDailyMails(actualAddedCount);

        if (TutorialUI.Instance != null &&
            TutorialUI.Instance.GetCurrentQuestStep() == TutorialUI.QuestStep.PickUpMails)
        {
            TutorialUI.Instance.UpdateQuestStep(TutorialUI.QuestStep.DeliverMails);
        }

        Debug.Log($"[MailPickUp] Berhasil mengambil {actualAddedCount} surat! Target EndOfDay diset ke: {actualAddedCount}");
        Destroy(gameObject);
    }
}