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
    public NavMeshAgent Agent {get => agent;}
    public GameObject Player => player;
    public Vector3 LastKnownPos { get => lastKnownPos; set => lastKnownPos = value; }

    [SerializeField]
    private string currentState;

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
    }

    
    void Update()
    {
        CanSeePlayer();
        currentState = stateMachine.activeState.ToString();
    }

    public bool CanSeePlayer()
    {
        if (player is not null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= sightDistance)
            {
                Vector3 tartetDirection = player.transform.position - transform.position - (Vector3.up * eyeHeight);
                float angleToPlayer = Vector3.Angle(tartetDirection, transform.forward);
                if (angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
                {
                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), tartetDirection);
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
}
