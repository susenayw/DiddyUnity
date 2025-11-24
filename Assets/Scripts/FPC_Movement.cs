using UnityEngine;

public class FPC_Movement : MonoBehaviour
{
    // Komponen CharacterController yang kita gunakan
    private CharacterController controller;

    // Kecepatan gerakan dan gravitasi
    public float movementSpeed = 8f;
    public float gravity = -9.81f * 2; // Nilai gravitasi dipercepat

    // Vektor untuk menyimpan kecepatan vertikal (gravitasi)
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

        // 3. GRAVITASI (Pergerakan Vertikal)
        // Cek apakah pemain menyentuh tanah (Controller.isGrounded)
        if (controller.isGrounded && velocity.y < 0)
        {
            // Reset kecepatan vertikal jika menyentuh tanah (nilai kecil mencegah terbang)
            velocity.y = -2f;
        }

        // Terapkan gravitasi secara konstan
        velocity.y += gravity * Time.deltaTime;

        // Terapkan kecepatan vertikal ke controller
        controller.Move(velocity * Time.deltaTime);
    }
}