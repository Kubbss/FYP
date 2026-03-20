using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField]
    private GameObject chest;

    private bool chestOpen;
    
    private Animator chestAnimator;

    void Start()
    {
        chestAnimator = chest.GetComponent<Animator>();
    }

    public void ToggleChest()
    {
        chestOpen = !chestOpen;
        chestAnimator.SetBool("IsOpen", chestOpen);
    }
}
