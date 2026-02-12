using System.Collections;
using UnityEngine;

public class MasterBarrel : MonoBehaviour
{
    
    private GameObject player;
    private Camera playerCam;
    private PlayerMotor motor;
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
    
    public void TryLockPlayer()
    {
        motor.LockPlayer();
    }
    
    public IEnumerator TryUnlockPlayer()
    {
        yield return new WaitForSeconds(0.1f);
        motor.UnlockPlayer();
    }
    
}
