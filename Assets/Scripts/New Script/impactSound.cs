using UnityEngine;

// Requires the object to have both components
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class ImpactSound : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip impactClip;
    public float velocityThreshold = 1.0f; // Minimum speed required to trigger a sound
    public float pitchVariation = 0.1f;    // Random pitch adjustment for natural sound

    private AudioSource audioSource;

    void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false; // Prevent playing sound immediately when scene loads
    }

    // Called when this object hits another object using physics
    private void OnCollisionEnter(Collision collision)
    {
        // Calculate the speed of the collision
        // collision.relativeVelocity is the difference in velocity between the two colliding objects
        float impactSpeed = collision.relativeVelocity.magnitude;

        // Check if the impact speed is above the required threshold
        if (impactSpeed > velocityThreshold && impactClip != null)
        {
            // 1. Calculate Volume and Pitch based on impact speed
            // Use a linear map (or similar) to calculate volume based on impact speed
            float impactVolume = Mathf.Clamp01(impactSpeed / 10.0f); // Volume scales up to max at speed 10

            // 2. Apply Pitch Variation
            float randomPitch = Random.Range(1.0f - pitchVariation, 1.0f + pitchVariation);

            // 3. Play the sound
            audioSource.pitch = randomPitch;
            audioSource.PlayOneShot(impactClip, impactVolume);

            // Optional: Debugging the impact
            Debug.Log($"Impact! Speed: {impactSpeed:F2}, Volume: {impactVolume:F2}");
        }
    }
}