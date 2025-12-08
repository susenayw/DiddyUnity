using UnityEngine;

public class MirrorReflection : MonoBehaviour
{
    // Hubungkan di Inspector
    public Camera mainCamera;
    public Transform mirrorSurface;

    void Update()
    {
        if (mainCamera == null || mirrorSurface == null) return;

        // Hitung posisi dan rotasi refleksi
        Vector3 position = mirrorSurface.InverseTransformPoint(mainCamera.transform.position);
        position.x *= -1; // Cerminkan sumbu X
        transform.position = mirrorSurface.TransformPoint(position);

        Vector3 forward = mirrorSurface.InverseTransformDirection(mainCamera.transform.forward);
        forward.x *= -1;
        transform.forward = mirrorSurface.TransformDirection(forward);
    }
}