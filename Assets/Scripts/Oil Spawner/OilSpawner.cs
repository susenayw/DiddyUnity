using UnityEngine;

public class OilSpawner : MonoBehaviour
{
    // Prefab Oil (dulunya Diddys.fbx) yang sudah ada Rigidbody & Collider
    public GameObject oilPrefab; // Nama variabel diubah untuk konsistensi

    // PASTIKAN FUNGSI INI BERSIFAT PUBLIK
    public void SpawnOil()
    {
        // Mendapatkan posisi dari GameObject yang memiliki skrip ini (OilSpawnPoint)
        Vector3 spawnPosition = transform.position;

        // Memunculkan (Instantiate) Prefab Oil pada lokasi SpawnPoint
        GameObject newOil = Instantiate(oilPrefab, spawnPosition, Quaternion.identity);

        Debug.Log("Baby Oil berhasil di-spawn!");
    }
}