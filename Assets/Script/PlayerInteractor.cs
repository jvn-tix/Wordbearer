using UnityEngine;
using TMPro;

public class PlayerInteractor : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private GameObject interactPanelUI;
    [SerializeField] private TextMeshProUGUI promptText;

    private IInteractable currentInteractable;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            currentInteractable = interactable;

            if (promptText != null) promptText.text = interactable.GetPrompt();
            if (interactPanelUI != null) interactPanelUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable) && interactable == currentInteractable)
        {
            currentInteractable = null;

            if (interactPanelUI != null) interactPanelUI.SetActive(false);
        }
    }

    // Fungsi ini dipanggil dari Unity Event pada komponen Player Input
    public void OnInteract()
    {
        if (currentInteractable != null)
        {
            currentInteractable.Interact();
            if (interactPanelUI != null) interactPanelUI.SetActive(false);
        }
    }
}