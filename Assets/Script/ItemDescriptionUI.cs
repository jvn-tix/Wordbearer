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
        backpackManager = FindObjectOfType<BackpackManager>();
        deliveryButton.SetActive(false); 
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

            if (backpackManager.isDeliveryMode == true)
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

    // PERUBAHAN BESAR: Logika Pengiriman dan Rating
    public void DeliverItem()
    {
        if (activeSlot != null && activeSlot.itemInSlot != null)
        {
            // Ambil nama tujuan dari surat dan nama kotak pos yang sedang dibuka
            string targetSurat = activeSlot.itemInSlot.targetHouse;
            string namaKotakPos = backpackManager.currentMailbox.houseID;

            // Pengecekan
            if (targetSurat == namaKotakPos)
            {
                // BENAR! Tambah 10 poin
                backpackManager.ChangeRating(10);
                Debug.Log("PENGIRIMAN SUKSES! Rating Naik.");
            }
            else
            {
                // SALAH! Kurangi 5 poin
                backpackManager.ChangeRating(-5);
                Debug.Log("SALAH ALAMAT! Rating Turun.");
            }

            activeSlot.ClearItem(); 
            ClearDescription();     
            
            // Opsional: Tutup tas otomatis setelah menekan tombol kirim
            backpackManager.CloseBackpack(); 
        }
    }

    public void ClearDescription()
    {
        activeSlot = null;
        nameText.text = "";
        descriptionText.text = "";
        iconImage.gameObject.SetActive(false);
        deliveryButton.SetActive(false); 
    }
}