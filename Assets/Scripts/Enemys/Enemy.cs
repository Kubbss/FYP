using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

public class Enemy : MonoBehaviour
{
    private StateMachine stateMachine;
    private NavMeshAgent agent;
    private GameObject player;
    private Vector3 lastKnownPos;

    [SerializeField]
    private List<GameObject> barrelList;
    public NavMeshAgent Agent {get => agent;}
    public GameObject Player => player;
    public Vector3 LastKnownPos { get => lastKnownPos; set => lastKnownPos = value; }

    [SerializeField]
    private string currentState;
    
    private MasterBarrel masterBarrel;

    public float sightDistance = 20f;
    public float fieldOfView = 85;
    public float eyeHeight;
    public Path path;
    
    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialise();
        player = GameObject.FindWithTag("Player");
        barrelList = new List<GameObject>();
        masterBarrel = GameObject.FindGameObjectWithTag("BarrelController").GetComponent<MasterBarrel>();
    }

    
    void Update()
    {
        currentState = stateMachine.activeState.ToString();

        ListVisibleBarrels();
    }

    public bool CanSeePlayer()
    {
        if (player is not null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= sightDistance)
            {
                Vector3 targetDirection = player.transform.position - transform.position - (Vector3.up * eyeHeight);
                float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
                if (angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
                {
                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);
                    RaycastHit hitInfo = new RaycastHit();
                    if (Physics.Raycast(ray, out hitInfo, sightDistance))
                    {
                        if (hitInfo.transform.gameObject == player)
                        {
                            Debug.DrawRay(ray.origin, ray.direction * sightDistance, Color.red);
                            return true;
                        }
                    }
                    
                }
            }
        }

        return false;
    }

    public List<GameObject> ListVisibleBarrels()
    {
        if (barrelList.Count <= 0)
            return null;

        List<GameObject> returnList = new List<GameObject>();
        
        foreach (GameObject barrel in barrelList)
        {
            if (barrel is not null)
            {
                Vector3 targetDirection = barrel.transform.position - transform.position - (Vector3.up * eyeHeight);
                
                Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);
                RaycastHit hitInfo = new RaycastHit();

                if (Physics.Raycast(ray, out hitInfo, sightDistance))
                {
                    if (hitInfo.transform == barrel.transform)
                    {
                        Debug.DrawRay(ray.origin, ray.direction * sightDistance, Color.yellow);
                        returnList.Add(barrel);
                    }
                }
            }
        }
        
        return returnList;
    }

    public bool CheckIfPlayerInBarrel(GameObject barrel)
    {
        GameObject tempBarrel = masterBarrel.GetCurrentBarrel();
        
        if (tempBarrel is not null)
            return tempBarrel.Equals(barrel.transform.parent.gameObject);
        
        return false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Barrel"))
            return;
        
        Debug.Log("Adding Barrel : " + other.transform.parent.name);
        
        barrelList.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Barrel"))
            return;
        
        Debug.Log("Removing Barrel : " + other.transform.parent.name);
        
        barrelList.Remove(other.gameObject);
    }
    
    
}
