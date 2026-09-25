using UnityEngine;
using TMPro; // Untuk UI Teks Rating

public class BackpackManager : MonoBehaviour
{
    public GameObject backpackWindow; 
    public GameObject backpackIconBtn; 
    public SlotUI[] inventorySlots; 
    
    [HideInInspector] public bool isDeliveryMode = false; 
    
    // BARU: Menyimpan kotak pos mana yang sedang diakses
    [HideInInspector] public MailboxInteract currentMailbox; 

    // BARU: UI Teks untuk menampilkan rating
    public TextMeshProUGUI ratingTextUI;

    public static ItemData[] savedItems = new ItemData[10]; 
    
    // Memori permanen untuk rating (misal mulai dari angka 50)
    public static int playerRating = 50; 

    // BARU: Fungsi ini otomatis dipanggil oleh Unity tepat SEBELUM game dimulai (saat tombol Play ditekan)
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void ResetStaticData()
    {
        savedItems = new ItemData[10]; // Menghapus sisa barang dari test sebelumnya
        playerRating = 50;             // Mereset rating kembali ke 50
    }

    private void Start()
    {
        backpackWindow.SetActive(false);
        backpackIconBtn.SetActive(true);
        
        UpdateRatingUI(); // Tampilkan rating saat mulai

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
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (i < savedItems.Length) savedItems[i] = inventorySlots[i].itemInSlot;
        }
    }

    // --- FUNGSI RATING BARU ---
    public void ChangeRating(int amount)
    {
        playerRating += amount; // Tambah/kurang rating
        
        // Jaga agar rating tidak kurang dari 0 atau lebih dari 100 (opsional)
        playerRating = Mathf.Clamp(playerRating, 0, 100); 
        
        UpdateRatingUI();
    }

    private void UpdateRatingUI()
    {
        if (ratingTextUI != null)
        {
            ratingTextUI.text = "Rating: " + playerRating.ToString();
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
        currentMailbox = null; // Kosongkan
        OpenBackpack();
    }

    // PERUBAHAN: Menerima data kotak pos yang dibuka
    public void OpenBackpackMailbox(MailboxInteract mailbox)
    {
        isDeliveryMode = true; 
        currentMailbox = mailbox; // Simpan data kotak pos
        OpenBackpack();
    }

    private void OpenBackpack()
    {
        backpackWindow.SetActive(true);
        backpackIconBtn.SetActive(false);
        
        ItemDescriptionUI descUI = FindObjectOfType<ItemDescriptionUI>();
        if(descUI != null) descUI.ClearDescription();
    }

    public void CloseBackpack()
    {
        backpackWindow.SetActive(false);
        backpackIconBtn.SetActive(true);
        isDeliveryMode = false; 
        currentMailbox = null; 
    }
}