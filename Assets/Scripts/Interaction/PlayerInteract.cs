using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera cam;
    private float distance = 3f;
    [SerializeField]
    private LayerMask layerMask;
    
    void Start()
    {
        cam = GetComponent<PlayerLook>().cam;
    }

    
    void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance, layerMask))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Debug.Log(hitInfo.collider.GetComponent<Interactable>().promptMessage);
            }
        }
    }
}
