using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic; // SANGAT PENTING: Wajib ditambahkan agar fitur 'List' bisa digunakan

public class MailPickUp : MonoBehaviour
{
    public ItemData[] possibleMails; 
    
    // BARU: Menentukan batas minimal dan maksimal jumlah surat yang terambil
    public int minPickup = 3;
    public int maxPickup = 9; 
    
    public GameObject interactPrompt; 
    
    private bool isPlayerNear = false;
    private BackpackManager backpackManager;

    private void Start()
    {
        backpackManager = FindObjectOfType<BackpackManager>();
        
        if (interactPrompt != null)
        {
            interactPrompt.SetActive(false);
        }
    }

    private void Update()
    {
        if (isPlayerNear && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (possibleMails.Length > 0)
            {
                // 1. Tentukan berapa banyak surat yang akan diambil (acak antara min dan max)
                // Catatan: Random.Range untuk angka bulat (int) harus ditambah 1 pada nilai maksimalnya
                int amountToPickup = Random.Range(minPickup, maxPickup + 1);
                
                // Mencegah error jika jumlah yang mau diambil ternyata lebih banyak dari total jenis surat yang Anda miliki
                amountToPickup = Mathf.Min(amountToPickup, possibleMails.Length);

                // 2. Buat "Salinan" dari daftar surat Anda ke dalam bentuk List
                List<ItemData> availableMails = new List<ItemData>(possibleMails);

                for (int i = 0; i < amountToPickup; i++)
                {
                    // Pilih angka acak berdasarkan sisa surat di dalam List saat ini
                    int randomIndex = Random.Range(0, availableMails.Count);
                    
                    // Ambil surat dari List
                    ItemData selectedMail = availableMails[randomIndex];
                    
                    // Masukkan ke tas
                    backpackManager.AddItemToInventory(selectedMail); 
                    
                    // 3. HAPUS surat tersebut dari List agar tidak bisa terpilih lagi (Anti-Double)
                    availableMails.RemoveAt(randomIndex);
                }
                
                Debug.Log("Berhasil mengambil " + amountToPickup + " surat unik!");
                Destroy(gameObject); 
            }
            else
            {
                Debug.LogWarning("Kotak Possible Mails di Inspector masih kosong!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = true;
            if (interactPrompt != null) interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = false;
            if (interactPrompt != null) interactPrompt.SetActive(false);
        }
    }
}