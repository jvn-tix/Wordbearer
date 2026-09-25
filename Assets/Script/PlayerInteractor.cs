using UnityEngine;
using TMPro;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Default UI Reference")]
    [SerializeField] private GameObject defaultInteractPanel;
    [SerializeField] private TextMeshProUGUI defaultPromptText;

    [Header("Door UI Reference")]
    [SerializeField] private GameObject doorInteractPanel;
    [SerializeField] private TextMeshProUGUI doorPromptText;

    private IInteractable currentInteractable;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            currentInteractable = interactable;

            // Cek apakah objek yang didekati adalah SceneLoader (Pintu)
            if (interactable is SceneLoader)
            {
                if (doorPromptText != null) doorPromptText.text = interactable.GetPrompt();
                if (doorInteractPanel != null) doorInteractPanel.SetActive(true);
            }
            else
            {
                if (defaultPromptText != null) defaultPromptText.text = interactable.GetPrompt();
                if (defaultInteractPanel != null) defaultInteractPanel.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable) && interactable == currentInteractable)
        {
            currentInteractable = null;

            if (defaultInteractPanel != null) defaultInteractPanel.SetActive(false);
            if (doorInteractPanel != null) doorInteractPanel.SetActive(false);
        }
    }

    public void OnInteract()
    {
        if (currentInteractable != null)
        {
            currentInteractable.Interact();
            if (defaultInteractPanel != null) defaultInteractPanel.SetActive(false);
            if (doorInteractPanel != null) doorInteractPanel.SetActive(false);
        }
    }
}