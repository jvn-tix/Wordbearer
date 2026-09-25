using UnityEngine;
using UnityEngine.InputSystem;

public class MailboxInteract : MonoBehaviour
{
    // BARU: Identitas rumah ini (Misal isi dengan "Rumah_A" di Inspector)
    public string houseID; 
    
    private bool isPlayerNear = false;
    private BackpackManager backpackManager;
    public GameObject interactPrompt;

    private void Start()
    {
        backpackManager = FindObjectOfType<BackpackManager>();
        if (interactPrompt != null) interactPrompt.SetActive(false);
    }

    private void Update()
    {
        if (isPlayerNear && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            // PERUBAHAN: Sekarang kita mengirimkan data kotak pos ini (this) ke Manager
            backpackManager.OpenBackpackMailbox(this);
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