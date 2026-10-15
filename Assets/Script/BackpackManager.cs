using UnityEngine;
using TMPro;

public class BackpackManager : MonoBehaviour
{
    public static BackpackManager Instance { get; private set; }

    public GameObject backpackWindow;
    public GameObject backpackIconBtn;
    public SlotUI[] inventorySlots;

    [HideInInspector] public bool isDeliveryMode = false;
    [HideInInspector] public MailboxInteract currentMailbox;

    public TextMeshProUGUI ratingTextUI;

    // --- STATIC DATA (PERSISTEN ANTAR SCENE) ---
    public static ItemData[] savedItems = new ItemData[10];
    public static int playerRating = 0;
    public static int totalMailsToday = 0;
    public static int deliveredMailsCount = 0;

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

    // Fungsi ini dipanggil manual atau saat Next Day untuk mereset progres harian & rating jika diinginkan
    public static void ResetAllGameData()
    {
        savedItems = new ItemData[10];
        playerRating = 0; // Reset rating ke 0 (atau ke nilai awal misal 50 jika mau)
        totalMailsToday = 0;
        deliveredMailsCount = 0;
    }

    // --- KHUSUS RESET SAAT NEXT DAY ---
    public void ResetForNextDay()
    {
        totalMailsToday = 0;
        deliveredMailsCount = 0;
        playerRating = 0; // Reset rating ke 0 (atau ke nilai awal misal 50 jika mau)
        UpdateRatingUI();
    }

    private void Start()
    {
        if (backpackWindow != null) backpackWindow.SetActive(false);
        if (backpackIconBtn != null) backpackIconBtn.SetActive(true);

        UpdateRatingUI();

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (i < savedItems.Length && savedItems[i] != null)
            {
                inventorySlots[i].SetItem(savedItems[i]);
            }
            else
            {
                inventorySlots[i].ClearItem();
            }
        }
    }

    private void OnDestroy()
    {
        if (inventorySlots != null)
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (i < savedItems.Length && inventorySlots[i] != null)
                {
                    savedItems[i] = inventorySlots[i].itemInSlot;
                }
            }
        }
    }

    // --- FUNGSI TRACKING SURAT HARI INI ---
    public void SetDailyMails(int randomAmount)
    {
        totalMailsToday = randomAmount;
        deliveredMailsCount = 0;
        Debug.Log($"[BackpackManager] SetDailyMails dipanggil. Total Mails Hari Ini: {totalMailsToday}");
    }

    public bool IsAllMailsProcessed()
    {
        Debug.Log($"[BackpackManager Cek Status] Terkirim: {deliveredMailsCount} / Target: {totalMailsToday}");
        return totalMailsToday > 0 && deliveredMailsCount >= totalMailsToday;
    }

    public int GetRating()
    {
        return playerRating;
    }

    // --- SISTEM RATING DINAMIS (TIDAK MONOTON KELIPATAN 5) ---
    public void CalculateAndApplyRating(bool isSuccess, float deliveryTimeSeconds, bool isLongDistance)
    {
        int basePoints = isSuccess ? 10 : -15; // Poin dasar sukses / gagal

        if (isSuccess)
        {
            // 1. Faktor Kecepatan: Makin cepat antar, makin besar bonusnya (maksimal +5)
            float speedBonus = Mathf.Max(0, (30f - deliveryTimeSeconds) / 6f);

            // 2. Faktor Jarak: Rumah jauh diberi bonus ekstra (+3)
            int distanceBonus = isLongDistance ? 3 : 0;

            // 3. Variasi Acak agar tidak monoton (angka acak antara -1 sampai +2)
            int randomVariance = Random.Range(-1, 3);

            // Total penambahan dihitung dinamis
            int finalAdd = Mathf.RoundToInt(basePoints + speedBonus + distanceBonus + randomVariance);

            playerRating += finalAdd;
        }
        else
        {
            // Jika salah kirim, berikan penalti dengan variasi acak kecil
            int randomPenalty = Random.Range(-2, 3);
            playerRating += (basePoints + randomPenalty);
        }

        // Batasi rating selalu di rentang 0 sampai 100
        playerRating = Mathf.Clamp(playerRating, 0, 100);
        UpdateRatingUI();
    }

    // (Opsional jika masih butuh fungsi lama untuk testing langsung)
    public void ChangeRating(int amount)
    {
        playerRating += amount;
        playerRating = Mathf.Clamp(playerRating, 0, 100);
        UpdateRatingUI();
    }

    private void UpdateRatingUI()
    {
        if (ratingTextUI != null)
        {
            ratingTextUI.text = "Rating: " + playerRating.ToString() + "%";
        }
    }

    public void AddItemToInventory(ItemData itemToAdd)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].itemInSlot == null)
            {
                inventorySlots[i].SetItem(itemToAdd);
                return;
            }
        }
    }

    public void OpenBackpackNormal()
    {
        isDeliveryMode = false;
        currentMailbox = null;
        OpenBackpack();
    }

    public void OpenBackpackMailbox(MailboxInteract mailbox)
    {
        isDeliveryMode = true;
        currentMailbox = mailbox;
        OpenBackpack();
    }

    private void OpenBackpack()
    {
        if (backpackWindow != null) backpackWindow.SetActive(true);
        if (backpackIconBtn != null) backpackIconBtn.SetActive(false);

        ItemDescriptionUI descUI = FindFirstObjectByType<ItemDescriptionUI>();
        if (descUI != null) descUI.ClearDescription();
    }

    public void CloseBackpack()
    {
        if (backpackWindow != null) backpackWindow.SetActive(false);
        if (backpackIconBtn != null) backpackIconBtn.SetActive(true);
        isDeliveryMode = false;
        currentMailbox = null;
    }
}