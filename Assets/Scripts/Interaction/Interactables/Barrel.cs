using System;
using Unity.VisualScripting;
using UnityEngine;

public class Barrel : Interactable
{
    private bool isPlayerInside = false;
    
    [SerializeField]
    private MasterBarrel masterBarrel;

    private void Start()
    {
        this.promptMessage = "Hide";
        masterBarrel = GameObject.FindGameObjectWithTag("BarrelController").GetComponent<MasterBarrel>();
    }

    protected override void Interact()
    {
        if (isPlayerInside)
        {
            masterBarrel.TrySetPlayerLocation(this.transform.position + (Vector3.forward * 1.4f) + (Vector3.up * 0.4f));
            masterBarrel.ResetPlayerCam();
            masterBarrel.TryUnlockPlayer();
            isPlayerInside = false;
        }
        else
        {
            if (masterBarrel.TryLockPlayer(transform.parent.gameObject))
            {
                masterBarrel.TrySetPlayerLocation(transform.position - (Vector3.up * 100));
                masterBarrel.TrySetPlayerCamLocation(transform.position);
                isPlayerInside = true;
            }
        }
    }
}
