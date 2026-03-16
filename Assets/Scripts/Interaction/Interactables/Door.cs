using UnityEngine;

public class Door : Interactable
{
    [SerializeField]
    private GameObject door;

    private bool doorOpen;
    
    private Animator doorAnimator;

    void Start()
    {
        doorAnimator = door.GetComponent<Animator>();
    }
    
    protected override void Interact()
    {
        doorOpen = !doorOpen;
        doorAnimator.SetBool("IsOpen", doorOpen);
    }
}
