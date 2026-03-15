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

    private AudioSource audioSource;
    private Vector3 lastPosition;
    private float distanceTravelled;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        lastPosition = transform.position;
    }

    void Update()
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
        else
        {
            distanceTravelled = 0f;
        }
    }

    void PlayRandomFootstep()
    {
        if (footstepClips == null || footstepClips.Length == 0)
            return;

        int randomIndex = Random.Range(0, footstepClips.Length);
        AudioClip clipToPlay = footstepClips[randomIndex];

        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(clipToPlay);
    }
}