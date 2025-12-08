using UnityEngine;

public class ButtonInteraction : MonoBehaviour
{
    // Hubungkan skrip OilSpawner dari OilSpawnPoint di Inspector
    public OilSpawner oilSpawner;

    // Tombol interaksi yang diinginkan
    public KeyCode interactKey = KeyCode.F;

    // Status apakah pemain berada di dalam trigger
    private bool playerIsNearby = false;

    void Update()
    {
        // Cek apakah pemain dekat DAN menekan tombol F
        if (playerIsNearby && Input.GetKeyDown(interactKey))
        {
            // Panggil fungsi spawner
            if (oilSpawner != null)
            {
                oilSpawner.SpawnOil();
                Debug.Log("Tombol F ditekan, Oil di-spawn!");
                // Anda bisa menambahkan animasi/suara tombol di sini
            }
        }
    }

    // Dipanggil saat objek lain (Pemain) masuk ke area Trigger
    private void OnTriggerEnter(Collider other)
    {
        // Tag "Player" harus diterapkan ke objek Player Anda
        if (other.CompareTag("Player"))
        {
            playerIsNearby = true;
            Debug.Log("Pemain dekat dengan tombol. Tekan F.");
            // Optional: Tampilkan prompt UI "Tekan F"
        }
    }

    // Dipanggil saat objek lain (Pemain) keluar dari area Trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNearby = false;
            // Optional: Sembunyikan prompt UI
        }
    }
}