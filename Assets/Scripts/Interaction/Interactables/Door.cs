using UnityEngine;
using UnityEngine.AI;

public class Door : Interactable
{
    [SerializeField]
    private GameObject door;

    [SerializeField] private bool isLocked = false;
    [SerializeField] private bool hasKey = false;
    [SerializeField] private bool canReuse = false;

    [SerializeField] private GameObject keyIcon;
    
    private bool doorOpen;
    private Animator doorAnimator;
    private string originalText;
    
    void Start()
    {
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
                
                if (!canReuse)
                    gameObject.layer = LayerMask.NameToLayer("Default");
            }
            return;
        }

        if (!canReuse)
            gameObject.layer = LayerMask.NameToLayer("Default");
                
                
                
        doorOpen = !doorOpen;        
        doorAnimator.SetBool("IsOpen", doorOpen);
    }

    public void GuardOpenDoor()
    {
        if (doorOpen)
            return;
        
        doorOpen = true;
        doorAnimator.SetBool("IsOpen", doorOpen);
    }

    public void GuardCloseDoor()
    {
        if (!doorOpen)
            return;

        if (isLocked)
        {
            doorOpen = false;
            doorAnimator.SetBool("IsOpen", doorOpen);
        }
        else if (canReuse)
        {
            doorOpen = false;
            doorAnimator.SetBool("IsOpen", doorOpen);
        }
    }
}
