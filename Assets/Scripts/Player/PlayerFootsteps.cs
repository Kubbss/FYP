using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class PlayerFootsteps : MonoBehaviour
{
    [Header("Footstep Sounds")]
    public AudioClip[] footstepClips;

    [Header("Step Settings")]
    public float stepDistance = 2f;
    public float minMoveSpeed = 0.1f;

    [Header("Pitch Randomness")]
    public float minPitch = 0.95f;
    public float maxPitch = 1.05f;

    private CharacterController controller;
    private AudioSource audioSource;
    private Vector3 lastPosition;
    private float distanceTravelled;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        lastPosition = transform.position;
    }

    void Update()
    {
        Vector3 currentPosition = transform.position;
        Vector3 movement = currentPosition - lastPosition;
        movement.y = 0f;

        distanceTravelled += movement.magnitude;
        lastPosition = currentPosition;

        if (controller.isGrounded && controller.velocity.magnitude > minMoveSpeed)
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