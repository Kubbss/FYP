using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GuardFootsteps : MonoBehaviour
{
    [Header("Footstep Sounds")]
    public AudioClip[] footstepClips;

    [Header("Step Settings")]
    public float stepDistance = 2f;
    public float minMoveDistancePerFrame = 0.001f;

    [Header("Pitch Randomness")]
    public float minPitch = 0.8f;
    public float maxPitch = 1.2f;

    [Header("Muffling")]
    public Transform player;
    public float rayHeight = 0.8f;
    public float normalVolume = 1f;
    public float muffledVolume = 0.35f;
    public float normalCutoff = 22000f;
    public float muffledCutoff = 1200f;

    private AudioSource audioSource;
    private AudioLowPassFilter lowPassFilter;
    private Vector3 lastPosition;
    private float distanceTravelled;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        lowPassFilter = GetComponent<AudioLowPassFilter>();
        lastPosition = transform.position;
    }

    void FixedUpdate()
    {
        Vector3 currentPosition = transform.position;
        Vector3 movement = currentPosition - lastPosition;
        movement.y = 0f;

        float movedThisFrame = movement.magnitude;
        distanceTravelled += movedThisFrame;

        lastPosition = currentPosition;

        if (movedThisFrame > minMoveDistancePerFrame)
        {
            if (distanceTravelled >= stepDistance)
            {
                PlayRandomFootstep();
                distanceTravelled = 0f;
            }
        }
    }

    void PlayRandomFootstep()
    {
        if (footstepClips == null || footstepClips.Length == 0)
            return;

        if (player != null)
        {
            Vector3 origin = transform.position + Vector3.up * rayHeight;
            Vector3 target = player.position + Vector3.up * rayHeight;
            Vector3 direction = target - origin;
            float distanceToPlayer = direction.magnitude;

            if (distanceToPlayer > 0.01f &&
                Physics.Raycast(origin, direction.normalized, out RaycastHit hit, distanceToPlayer))
            {
                if (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("Floor") || hit.collider.CompareTag("Door"))
                {
                    audioSource.volume = muffledVolume;

                    if (lowPassFilter != null)
                        lowPassFilter.cutoffFrequency = muffledCutoff;
                }
                else
                {
                    audioSource.volume = normalVolume;

                    if (lowPassFilter != null)
                        lowPassFilter.cutoffFrequency = normalCutoff;
                }
            }
            else
            {
                audioSource.volume = normalVolume;

                if (lowPassFilter != null)
                    lowPassFilter.cutoffFrequency = normalCutoff;
            }
        }
        else
        {
            audioSource.volume = normalVolume;

            if (lowPassFilter != null)
                lowPassFilter.cutoffFrequency = normalCutoff;
        }

        int randomIndex = Random.Range(0, footstepClips.Length);
        AudioClip clipToPlay = footstepClips[randomIndex];

        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(clipToPlay);
    }
}