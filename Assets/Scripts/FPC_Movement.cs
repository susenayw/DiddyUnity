using UnityEngine;

public class FPC_Movement : MonoBehaviour
{
    // Komponen CharacterController yang kita gunakan
    private CharacterController controller;

    // Kecepatan gerakan, gravitasi, dan JUMP
    public float movementSpeed = 8f;
    public float gravity = -9.81f * 2; // Nilai gravitasi dipercepat
    public float jumpHeight = 2.0f; // Ketinggian lompatan yang diinginkan

    // Vektor untuk menyimpan kecepatan vertikal (gravitasi dan lompatan)
    private Vector3 velocity;

    void Start()
    {
        // Mendapatkan referensi komponen Character Controller saat game dimulai
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 1. INPUT
        float x = Input.GetAxis("Horizontal"); // A/D atau Panah Kiri/Kanan
        float z = Input.GetAxis("Vertical");   // W/S atau Panah Atas/Bawah

        // 2. TRANSLASI (Pergerakan Horizontal)
        // Menghitung arah pergerakan relatif terhadap orientasi Player
        Vector3 move = transform.right * x + transform.forward * z;

        // Menggunakan CharacterController untuk memindahkan objek
        controller.Move(move * movementSpeed * Time.deltaTime);

        // 3. LOGIKA GRAVITASI & LOMPATAN (Pergerakan Vertikal)

        // Cek apakah pemain menyentuh tanah (Controller.isGrounded)
        if (controller.isGrounded)
        {
            // --- DEBUG AKTIF ---
            // Debug.Log("Status Tanah: Is Grounded"); 

            // Reset kecepatan vertikal jika menyentuh tanah (nilai kecil mencegah terbang)
            if (velocity.y < 0)
            {
                velocity.y = -2f;
            }

            // INPUT LOMPATAN: Memicu lompatan hanya saat di tanah
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Rumus fisika dasar: v = sqrt(h * -2 * g)
                // Ditambah faktor pengali kecil (1.05f) untuk mengatasi gesekan CharacterController
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity) * 1.05f;
            }
        }
        else
        {
            // Debug.Log("Status Tanah: Di Udara"); 
        }

        // Terapkan gravitasi secara konstan
        // Rumus: V = V0 + a * t (V = kecepatan, a = percepatan (gravity), t = Time.deltaTime)
        velocity.y += gravity * Time.deltaTime;

        // Terapkan kecepatan vertikal ke controller
        controller.Move(velocity * Time.deltaTime);
    }
}