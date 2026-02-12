using UnityEngine;

public class Button : Interactable
{
    [SerializeField]
    private GameObject door;
    private Animator doorAnimator;
    private bool doorOpen;

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
