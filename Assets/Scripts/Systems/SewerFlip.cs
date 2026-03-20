using UnityEngine;

public class SewerFlip : MonoBehaviour
{
    [SerializeField] private GameObject sewer;

    private void FlipSewerUp()
    {
        sewer.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    private void FlipSewerDown()
    {
        sewer.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
    }
    
    
    
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FlipSewerDown();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FlipSewerUp();
        }
    }
}
