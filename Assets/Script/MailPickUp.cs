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

    private BackpackManager backpackManager;

    private void Start()
    {
        backpackManager = FindFirstObjectByType<BackpackManager>();
    }

    // Dipanggil oleh PlayerInteractor untuk menampilkan petunjuk UI
    public string GetPrompt()
    {
        return promptMessage;
    }

    // Dipanggil otomatis oleh PlayerInteractor saat tombol interaksi ditekan
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

        if (backpackManager == null)
        {
            Debug.LogError("BackpackManager tidak ditemukan di Scene!");
            return;
        }

        // 1. Tentukan berapa banyak surat yang akan diambil (acak antara min dan max)
        int amountToPickup = Random.Range(minPickup, maxPickup + 1);
        amountToPickup = Mathf.Min(amountToPickup, possibleMails.Length);

        // 2. Buat "Salinan" dari daftar surat ke dalam bentuk List
        List<ItemData> availableMails = new List<ItemData>(possibleMails);

        for (int i = 0; i < amountToPickup; i++)
        {
            int randomIndex = Random.Range(0, availableMails.Count);
            ItemData selectedMail = availableMails[randomIndex];

            // Masukkan ke tas
            backpackManager.AddItemToInventory(selectedMail);

            // Hapus surat dari List agar tidak terduplikasi
            availableMails.RemoveAt(randomIndex);
        }

        if (TutorialUI.Instance != null &&
            TutorialUI.Instance.GetCurrentQuestStep() == TutorialUI.QuestStep.PickUpMails)
        {
            TutorialUI.Instance.UpdateQuestStep(TutorialUI.QuestStep.DeliverMails);
        }

        Debug.Log("Berhasil mengambil " + amountToPickup + " surat unik!");
        Destroy(gameObject);
    }
}