using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AudioSource))]

public class Door : Interactable
{
    [SerializeField]
    private GameObject door;

    [Header("Door Setup")]
    [SerializeField] private bool isLocked = false;
    [SerializeField] private bool hasKey = false;
    [SerializeField] private bool canReuse = false;
    
    [Header("Door SFX")]
    
    [SerializeField] private AudioClip doorInteractClip;
    [SerializeField] private float doorInteractVolume = 1f;
    
    [Header("UI Elements")]
    [SerializeField] private GameObject keyIcon;
    
    private AudioSource audioSource;
    private bool doorOpen;
    private Animator doorAnimator;
    private string originalText;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        doorAnimator = door.GetComponent<Animator>();
        
        originalText = this.promptMessage;

        if (isLocked)
            promptMessage = "Door is locked";
        
        if (!hasKey)
        {
            if (keyIcon != null)
                keyIcon.SetActive(false);
        }
    }

    public void UnlockDoor()
    {
        if (keyIcon != null)
            keyIcon.SetActive(false);
        
        isLocked = false;
        promptMessage = originalText;
    }

    public void PlayerGotKey()
    {
        if (isLocked)
        {
            if (keyIcon != null)
                keyIcon.SetActive(true);
            
            hasKey = true;
            promptMessage = "[E] Unlock Door";
        }
    }
    
    protected override void Interact()
    {
        if (isLocked)
        {
            if (hasKey)
            {
                UnlockDoor();
                
                doorOpen = !doorOpen;
                doorAnimator.SetBool("IsOpen", doorOpen);
                PlayDoorInteractSFX();
                
                if (!canReuse)
                    gameObject.layer = LayerMask.NameToLayer("Default");
            }
            return;
        }

        if (!canReuse)
            gameObject.layer = LayerMask.NameToLayer("Default");
                
                
                
        doorOpen = !doorOpen;        
        doorAnimator.SetBool("IsOpen", doorOpen);
        PlayDoorInteractSFX();
    }

    public void GuardOpenDoor()
    {
        if (doorOpen)
            return;
        
        doorOpen = true;
        doorAnimator.SetBool("IsOpen", doorOpen);
        PlayDoorInteractSFX();
    }

    public void GuardCloseDoor()
    {
        if (!doorOpen)
            return;

        if (isLocked)
        {
            doorOpen = false;
            doorAnimator.SetBool("IsOpen", doorOpen);
            PlayDoorInteractSFX();
        }
        else if (canReuse)
        {
            doorOpen = false;
            doorAnimator.SetBool("IsOpen", doorOpen);
            PlayDoorInteractSFX();
        }
    }
    
    private void PlayDoorInteractSFX()
    {
        if (audioSource == null || doorInteractClip == null)
            return;

        audioSource.PlayOneShot(doorInteractClip, doorInteractVolume);
    }
}
