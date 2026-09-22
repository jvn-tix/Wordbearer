using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour, IInteractable
{
    [SerializeField] private string sceneName;
    [SerializeField] private string promptMessage = "Press E to Enter";

    public string GetPrompt()
    {
        return promptMessage;
    }

    public void Interact()
    {
        LoadScene();
    }

    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Scene Name belum diisi pada Inspector " + gameObject.name);
        }
    }
}