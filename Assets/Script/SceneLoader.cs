using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour, IInteractable
{
    [SerializeField] private string sceneToLoad; // Nama scene tujuan (misal: "Main")
    [SerializeField] private string promptMessage = "Press E to Enter";

    public string GetPrompt()
    {
        return promptMessage;
    }

    // Dipanggil oleh PlayerInteractor saat tombol interaksi ditekan
    public void Interact()
    {
        LoadScene();
    }

    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("Scene To Load belum diisi di Inspector pada " + gameObject.name);
        }
    }
}