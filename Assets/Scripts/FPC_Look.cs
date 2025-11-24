using UnityEngine;

public class FPC_Look : MonoBehaviour
{
    // Kecepatan sensitivitas mouse
    public float mouseSensitivity = 100f;

    // Referensi ke GameObject induk (Player) untuk rotasi horizontal
    public Transform playerBody;

    private float xRotation = 0f;

    void Start()
    {
        // Mengunci dan menyembunyikan kursor mouse saat game berjalan
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 1. INPUT MOUSE
        // Mengambil perubahan posisi mouse. Time.deltaTime digunakan untuk frame-rate independence.
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 2. ROTASI VERTIKAL (Kamera saja - Look Up/Down)
        xRotation -= mouseY;
        // Batasi pandangan vertikal agar tidak berputar 360 derajat (clamp)
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Menerapkan rotasi pada Kamera secara lokal (hanya sumbu X lokal)
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 3. ROTASI HORIZONTAL (Player Body - Belok Kiri/Kanan)
        // Rotasi diterapkan pada objek induk (Player)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}