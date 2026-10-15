using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Game sedang ditutup..."); // Untuk memunculkan teks di Console saat di-test di Editor

        Application.Quit(); // Perintah utama untuk menutup aplikasi

        // Tambahan khusus jika kamu mengetesnya langsung di Unity Editor:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}