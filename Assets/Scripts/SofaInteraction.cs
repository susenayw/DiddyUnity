using UnityEngine;

public class SofaInteraction : MonoBehaviour
{
    [Header("Connections")]
    public Transform sitPoint; // Masukkan object 'SitPoint'
    public GameObject player;  // Masukkan object 'Player'
    public KeyCode interactKey = KeyCode.E;

    [Header("Smooth Movement Settings")]
    public float smoothSpeed = 5f;

    private bool isPlayerNearby = false;
    private bool isSitting = false;
    private Vector3 originalPos;
    private Transform targetSitPoint = null; // Target posisi untuk Lerp

    void Start()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // 1. Logika Pergerakan Smooth (Lerp)
        if (targetSitPoint != null)
        {
            // Pindahkan player sedikit demi sedikit ke posisi target
            player.transform.position = Vector3.Lerp(
                player.transform.position,
                targetSitPoint.position,
                smoothSpeed * Time.deltaTime
            );

            // Putar player secara smooth
            player.transform.rotation = Quaternion.Slerp(
                player.transform.rotation,
                targetSitPoint.rotation,
                smoothSpeed * Time.deltaTime
            );
        }


        // 2. Logika Deteksi Input
        if (isPlayerNearby && Input.GetKeyDown(interactKey))
        {
            if (!isSitting)
            {
                SitDown();
            }
            else
            {
                StandUp();
            }
        }
    }

    void SitDown()
    {
        // 1. Simpan posisi berdiri
        originalPos = player.transform.position;

        // 2. Matikan CharacterController
        if (player.GetComponent<CharacterController>())
            player.GetComponent<CharacterController>().enabled = false;

        // 3. Set Target untuk gerakan smooth
        targetSitPoint = sitPoint;

        isSitting = true;
        Debug.Log("Player Duduk (Smooth)");
    }

    void StandUp()
    {
        // Matikan target smooth, Player kembali ke posisi berdiri
        targetSitPoint = null;

        // Nyalakan kembali kontroler dan set posisi berdiri (Translasi Manual)
        if (player.GetComponent<CharacterController>())
            player.GetComponent<CharacterController>().enabled = true;

        player.transform.position = originalPos; // Pastikan posisi kembali ke tempat berdiri

        isSitting = false;
        Debug.Log("Player Berdiri");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("Tekan E untuk Duduk");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}