using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Nama scene utama yang akan dimuat saat tombol ditekan
    public string mainGameSceneName = "Game2";
    [SerializeField] private AudioClip Diddyblud; // Suara yang akan dimainkan
    private AudioSource audioSource;

    void Start()
    {
        // CRITICAL FIX: Ensure the cursor is visible and unlocked when the main menu loads.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        // Play the menu music
        if (Diddyblud != null)
        {
            audioSource.clip = Diddyblud;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    // Fungsi yang dipanggil saat tombol PLAY ditekan
    public void StartGame()
    {
        // Optionally lock the cursor immediately when starting the game
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Pastikan scene utama sudah ditambahkan ke Build Settings
        SceneManager.LoadScene(mainGameSceneName);
    }

    // Fungsi opsional untuk keluar dari aplikasi (hanya berfungsi di build)
    public void QuitGame()
    {
        Application.Quit();
    }
}