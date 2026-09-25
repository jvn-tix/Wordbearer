using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    public ItemData itemInSlot; 
    public Image slotIcon;      
    
    private ItemDescriptionUI descriptionPanel;

    private void Start()
    {
        descriptionPanel = FindObjectOfType<ItemDescriptionUI>();
        RefreshSlot(); // Cek dan atur gambar saat mulai
    }

    // Fungsi untuk memperbarui tampilan slot
    public void RefreshSlot()
    {
        if (itemInSlot != null)
        {
            slotIcon.sprite = itemInSlot.itemIcon;
            slotIcon.enabled = true;
        }
        else
        {
            slotIcon.sprite = null;
            slotIcon.enabled = false;
        }
    }

    // Fungsi untuk mengisi slot dengan item baru
    public void SetItem(ItemData newItem)
    {
        itemInSlot = newItem;
        RefreshSlot();
    }

    // Fungsi untuk membuang/menghapus item dari slot
    public void ClearItem()
    {
        itemInSlot = null;
        RefreshSlot();
    }

    public void OnSlotClicked()
    {
        if (descriptionPanel != null && itemInSlot != null)
        {
            // Perubahan: Sekarang kita mengirim SLOT ini sendiri (this), bukan cuma Data-nya
            descriptionPanel.UpdateDescription(this);
        }
    }
}