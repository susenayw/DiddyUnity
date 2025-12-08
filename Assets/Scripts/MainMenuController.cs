using UnityEngine;
using UnityEngine.SceneManagement; // Wajib diimpor untuk berpindah scene

public class MainMenuController : MonoBehaviour
{
    // Nama scene utama yang akan dimuat saat tombol ditekan
    public string mainGameSceneName = "Game2";

    // Fungsi yang dipanggil saat tombol PLAY ditekan
    public void StartGame()
    {
        // Pastikan scene utama sudah ditambahkan ke Build Settings
        SceneManager.LoadScene(mainGameSceneName);
    }

    // Fungsi opsional untuk keluar dari aplikasi (hanya berfungsi di build)
    public void QuitGame()
    {
        Application.Quit();
    }
}