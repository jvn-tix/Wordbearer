using UnityEngine;
using UnityEngine.InputSystem;

public class MailboxInteract : MonoBehaviour
{
    // BARU: Identitas rumah ini (Misal isi dengan "Rumah_A" di Inspector)
    public string houseID; 
    
    private bool isPlayerNear = false;
    private BackpackManager backpackManager;
    public GameObject interactPrompt;

    [Header("Distance Settings")]
    [SerializeField] private Transform postOfficeTransform;
    [SerializeField] private float longDistanceThreshold = 25f;

    private void Start()
    {
        backpackManager = FindFirstObjectByType<BackpackManager>();
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

    public bool CheckIsLongDistance()
    {
        if (postOfficeTransform == null)
        {
            GameObject po = GameObject.FindGameObjectWithTag("PostOffice");
            if (po != null) postOfficeTransform = po.transform;
        }

        if (postOfficeTransform != null)
        {
            float distance = Vector3.Distance(postOfficeTransform.position, transform.position);
            return distance >= longDistanceThreshold;
        }
        return false;
    }
}