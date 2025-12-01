using UnityEngine;

public class TV_Controller : MonoBehaviour
{
    [Header("TV Components")]
    public Renderer layarTV; // Masukkan objek Screen ke sini
    public Light lampuRuangan; // Masukkan objek Light ke sini (opsional)

    [Header("Raycast Settings")]
    public float jarakInteraksi = 3f;

    private bool tvNyala = false;

    void Update()
    {
        // Pengecekan keamanan: Pastikan ada kamera utama
        if (Camera.main == null) return;

        Ray sinar = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit kenaSesuatu;

        // Coba deteksi tabrakan sinar
        if (Physics.Raycast(sinar, out kenaSesuatu, jarakInteraksi))
        {
            // Cek: Objek yang kena adalah "Screen", tombol E ditekan, dan variabel layarTV terisi
            if (kenaSesuatu.collider != null && // Tambahan: Pastikan terkena collider
                kenaSesuatu.collider.gameObject.name.Contains("Screen") &&
                Input.GetKeyDown(KeyCode.E) &&
                layarTV != null)
            {
                UbahStatusTV();
            }
        }
    }

    void UbahStatusTV()
    {
        // Pengecekan keamanan: Pastikan material ada sebelum diubah
        if (layarTV.material == null)
        {
            Debug.LogError("Material layar TV hilang! Pastikan Mat_LayarTV sudah terpasang.");
            return;
        }

        tvNyala = !tvNyala; // Balik kondisi ON/OFF

        if (tvNyala)
        {
            // Mengubah nilai "_EmissiveIntensity" pada material TV menjadi 5 (Nyala)
            layarTV.material.SetFloat("_EmissiveIntensity", 5f);

            // Nyalakan lampu plafon
            if (lampuRuangan) lampuRuangan.enabled = true;
        }
        else
        {
            // Mengubah nilai "_EmissiveIntensity" pada material TV menjadi 0 (Mati)
            layarTV.material.SetFloat("_EmissiveIntensity", 0f);

            // Matikan lampu plafon
            if (lampuRuangan) lampuRuangan.enabled = false;
        }
    }
}