using UnityEngine;

public class SofaInteraction : MonoBehaviour
{
    [Header("Connections")]
    public Transform sitPoint;
    public GameObject player;
    public KeyCode interactKey = KeyCode.E; // Tombol untuk DUDUK

    // Script FPC_Look pada Kamera
    public MonoBehaviour cameraLookScript;
    // Referensi TV ini TIDAK DIGUNAN LAGI di script ini, tapi biarkan saja terisi di Inspector.
    public TV_Controller tvController;

    // Referensi untuk mematikan/menghidupkan script pergerakan
    public MonoBehaviour playerMovementScript;
    public MonoBehaviour playerLookScript;

    [Header("Smooth Movement Settings")]
    public float smoothSpeed = 5f;

    private bool isPlayerNearby = false;

    // --- PERBAIKAN: DIUBAH MENJADI PUBLIC UNTUK DIAKSES OLEH TV_Controller ---
    public bool isSitting = false;
    // --- AKHIR PERBAIKAN ---

    private Vector3 originalPos;
    private Transform targetSitPoint = null;

    void Start()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // 1. Logika Pergerakan Smooth (Lerp)
        if (targetSitPoint != null)
        {
            player.transform.position = Vector3.Lerp(
                player.transform.position,
                targetSitPoint.position,
                smoothSpeed * Time.deltaTime
            );

            player.transform.rotation = Quaternion.Slerp(
                player.transform.rotation,
                targetSitPoint.rotation,
                smoothSpeed * Time.deltaTime
            );
        }

        // 2. Logika Deteksi Input DUDUK (Hanya E)
        if (isPlayerNearby && Input.GetKeyDown(interactKey))
        {
            if (!isSitting) { SitDown(); } else { StandUp(); }
        }
    }

    void SitDown()
    {
        originalPos = player.transform.position;
        if (player.GetComponent<CharacterController>())
            player.GetComponent<CharacterController>().enabled = false;

        if (playerMovementScript != null) playerMovementScript.enabled = false;
        if (playerLookScript != null) playerLookScript.enabled = false;

        if (cameraLookScript != null)
        {
            cameraLookScript.enabled = false;
        }

        targetSitPoint = sitPoint;
        isSitting = true;
        Debug.Log("Player Duduk (Movement Locked)");
    }

    void StandUp()
    {
        if (cameraLookScript != null)
        {
            cameraLookScript.enabled = true;
        }

        if (playerLookScript != null) playerLookScript.enabled = true;
        if (playerMovementScript != null) playerMovementScript.enabled = true;

        // Logika MATIKAN TV
        if (tvController != null)
        {
            // Catatan: Jika Anda menggunakan F untuk TV, baris ini mungkin dihapus,
            // tetapi biarkan saja untuk kompatibilitas jika Anda memilih untuk mengganti fungsi TV.
            // Biarkan saja, karena tidak mengganggu fungsi anti-miring.
        }

        targetSitPoint = null;

        if (player.GetComponent<CharacterController>())
            player.GetComponent<CharacterController>().enabled = true;

        // --------------------------------------------------------------------
        // --- KODE BARU: MENGATUR ULANG ROTASI AGAR TIDAK MIRING (OYONG) ---
        // --------------------------------------------------------------------

        // 1. Ambil rotasi saat ini
        Vector3 currentEulerAngles = player.transform.localEulerAngles;

        // 2. Terapkan rotasi baru: X=0, Z=0, Y dipertahankan
        player.transform.localRotation = Quaternion.Euler(
            0f,                     // Atur X menjadi 0
            currentEulerAngles.y,   // Pertahankan rotasi Y (agar tetap menghadap depan)
            0f                      // Atur Z menjadi 0
        );

        // --------------------------------------------------------------------

        player.transform.position = originalPos;
        isSitting = false;
        Debug.Log("Player Berdiri");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) { isPlayerNearby = true; Debug.Log("Tekan E untuk Duduk & F untuk TV"); }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) { isPlayerNearby = false; }
    }
}