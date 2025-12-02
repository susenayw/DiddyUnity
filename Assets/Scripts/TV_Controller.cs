using UnityEngine;

public class TV_Controller : MonoBehaviour
{
    [Header("TV Components")]
    public Renderer layarTV;

    [Header("Interaction Checks")]
    public SofaInteraction sofaInteraction;
    public KeyCode tvToggleKey = KeyCode.F;

    private bool tvNyala = false;

    void Update()
    {
        // Cek 1: Apakah tombol F ditekan?
        if (Input.GetKeyDown(tvToggleKey))
        {
            // Cek 2: Apakah pemain sedang duduk?
            // Kita asumsikan sofaInteraction sudah terhubung di Inspector
            if (sofaInteraction != null && sofaInteraction.isSitting)
            {
                // Jika ya, ubah status TV
                UbahStatusTV(!tvNyala);
            }
            else
            {
                Debug.Log("Anda harus duduk dulu untuk menyalakan TV!");
            }
        }
    }

    public void NyalakanTV() { UbahStatusTV(true); }
    public void MatikanTV() { UbahStatusTV(false); }

    // Ubah UbahStatusTV (Menggunakan metode URP/Lit Shader)
    private void UbahStatusTV(bool status)
    {
        tvNyala = status;

        if (layarTV == null || layarTV.material == null)
        {
            Debug.LogError("Material layar TV hilang!");
            return;
        }

        if (tvNyala)
        {
            Color emissionColor = Color.white;
            float intensity = 5.0f; // Tingkat kecerahan

            // 1. Aktifkan Emission Keyword (Wajib untuk URP)
            layarTV.material.EnableKeyword("_EMISSION");

            // 2. Set warna (putih dikalikan intensitas 5.0f)
            // Ini menggantikan layarTV.material.SetFloat("_EmissiveIntensity", 5f);
            layarTV.material.SetColor("_EmissionColor", emissionColor * intensity);

            Debug.Log("TV Dinyalakan dengan Tombol F.");
        }
        else
        {
            // 1. Matikan Emission (Set warna ke hitam/nol)
            layarTV.material.SetColor("_EmissionColor", Color.black);

            // 2. Nonaktifkan Emission Keyword 
            layarTV.material.DisableKeyword("_EMISSION");

            Debug.Log("TV Dimatikan dengan Tombol F.");
        }
    }
}