using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MasterBarrel : MonoBehaviour
{
    
    private GameObject player;
    private Camera playerCam;
    private PlayerMotor motor;
    
    private GameObject currentBarrel = null;

    public int playerBarrelUsage;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        motor = player.GetComponent<PlayerMotor>();
    }

    public void ResetPlayerCam()
    {
        playerCam.transform.position = player.transform.position + (Vector3.up * 0.6f);
    }
    
    public void TrySetPlayerCamLocation(Vector3 location)
    {
        playerCam.transform.position = location;
    }

    public void TrySetPlayerLocation(Vector3 location)
    {
        player.transform.position = location;
    }
    
    public bool TryLockPlayer(GameObject askingBarrel)
    {
        if (currentBarrel.IsUnityNull())
        {
            currentBarrel = askingBarrel;
            motor.LockPlayer();
            //Debug.Log(currentBarrel.name);
            //Debug.Log(currentBarrel.transform.position);
            playerBarrelUsage++;
            return true;
        }

        //Debug.Log("Can't enter another barrel while inside one");
        return false;
    }
    
    public IEnumerator DelayedUnlockPlayer()
    {
        yield return new WaitForSeconds(0.1f);
        motor.UnlockPlayer();
    }

    public void TryUnlockPlayer()
    {
        StartCoroutine(DelayedUnlockPlayer());
        //Debug.Log("Player leaving barrel : " + currentBarrel.name + " at: " +  currentBarrel.transform.position);
        currentBarrel = null;
    }
    
    public GameObject GetCurrentBarrel()
    {
        return currentBarrel;
    }
}
