using UnityEngine;

public class SofaInteraction : MonoBehaviour
{
    [Header("Settings")]
    public Transform sitPoint; // Tempat kamu akan menarik objek SitPoint
    public GameObject player;  // Tempat kamu akan menarik objek Player

    private bool playerDekat = false;
    private bool sedangDuduk = false;
    private Vector3 posisiBerdiri;

    void Start()
    {
        // Mencoba mencari objek player secara otomatis jika belum diisi di Inspector
        if (player == null) player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // Deteksi input hanya jika player dekat
        if (playerDekat && Input.GetKeyDown(KeyCode.E))
        {
            if (sedangDuduk == false)
            {
                Duduk();
            }
            else
            {
                Berdiri();
            }
        }
    }

    void Duduk()
    {
        posisiBerdiri = player.transform.position;

        // Matikan CharacterController agar tidak ada konflik fisika
        if (player.GetComponent<CharacterController>())
            player.GetComponent<CharacterController>().enabled = false;

        // Translasi Manual (Memindahkan posisi player secara instan)
        player.transform.position = sitPoint.position;
        player.transform.rotation = sitPoint.rotation;

        sedangDuduk = true;
    }

    void Berdiri()
    {
        // Kembalikan ke posisi semula
        player.transform.position = posisiBerdiri;

        // Nyalakan kembali CharacterController
        if (player.GetComponent<CharacterController>())
            player.GetComponent<CharacterController>().enabled = true;

        sedangDuduk = false;
    }

    // Fungsi mendeteksi player masuk area trigger sofa
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDekat = true;
            Debug.Log("Tekan E untuk Duduk");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDekat = false;
        }
    }
}