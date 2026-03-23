using UnityEngine;

public class DoorGuardTrigger : MonoBehaviour
{
    [SerializeField] GameObject doorObject;

    private Door door;

    void Start()
    {
        door = doorObject.GetComponent<Door>();   
    }
    
    
    private int guardsInTrigger = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("EnemyMesh"))
            return;
        
        guardsInTrigger++;
    
        TryOpenDoor();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("EnemyMesh"))
            return;

        guardsInTrigger--;

        if (guardsInTrigger <= 0)
        {
            guardsInTrigger = 0;

            TryCloseDoor();
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("EnemyMesh"))
            return;

        if (guardsInTrigger > 0)
        {
            TryOpenDoor();
        }
    }

    private void TryOpenDoor()
    {
        door.GuardOpenDoor();
    }

    private void TryCloseDoor()
    {
        door.GuardCloseDoor();
    }
}
