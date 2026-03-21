using UnityEngine;

public class EscapeTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.PlayerEscape();
        }
    }
    
}
