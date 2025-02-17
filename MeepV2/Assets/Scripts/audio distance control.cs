using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioDistanceControl : MonoBehaviour
{
    public Camera mainCamera; // Assign your main camera in the inspector
    public float maxDistance = 20f; // Maximum distance for full volume
    public float minDistance = 1f; // Minimum distance for max loudness
    public float muffledVolume = 0.2f; // Volume when muffled

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (mainCamera == null)
            return;

        // Calculate distance between camera and audio source
        float distance = Vector3.Distance(mainCamera.transform.position, transform.position);

        // Set volume based on distance
        if (distance < minDistance)
        {
            audioSource.volume = 1f; // Full volume
        }
        else if (distance > maxDistance)
        {
            audioSource.volume = 0f; // No volume
        }
        else
        {
            // Interpolate volume based on distance
            audioSource.volume = 1f - ((distance - minDistance) / (maxDistance - minDistance));
        }

        // Check for wall obstruction using a raycast
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, transform.position - mainCamera.transform.position, out hit, maxDistance))
        {
            if (hit.transform != transform) // If the ray hit something other than the audio source
            {
                audioSource.volume *= muffledVolume; // Apply muffled volume
            }
        }
    }
}
