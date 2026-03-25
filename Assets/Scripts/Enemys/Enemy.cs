using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Enemy : MonoBehaviour
{
    private StateMachine stateMachine;
    private NavMeshAgent agent;
    private GameObject player;
    private PlayerSprint playerSprint;
    private Vector3 lastKnownPos;
    
    private PlayerHealth playerHealth;

    [SerializeField]
    private List<GameObject> barrelList;
    public NavMeshAgent Agent {get => agent;}
    public GameObject Player => player;
    
    public PlayerSprint PlayerSprint => playerSprint;
    public Vector3 LastKnownPos { get => lastKnownPos; set => lastKnownPos = value; }

    [SerializeField]
    private string currentState;

    [SerializeField] private Renderer eye1;
    [SerializeField] private Renderer eye2;
    
    private MasterBarrel masterBarrel;
    
    private Animator swordAnimator;
    private bool isSwinging;

    [Header("Guard Values")]
    public float sightDistance = 20f;
    public int hearDistance = 20;
    public float fieldOfView = 85f;
    public float eyeHeight = 0.6f;
    public float damage = 30f;
    public float damageInterval = 1f;
    public float attackRange = 3f;
    public int barrelSearchChance = 8;
    
    [Header("Path")]
    public Path path;
    
    
    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialise();
        player = GameObject.FindWithTag("Player");
        playerSprint = player.GetComponent<PlayerSprint>();
        barrelList = new List<GameObject>();
        masterBarrel = GameObject.FindGameObjectWithTag("BarrelController").GetComponent<MasterBarrel>();
        playerHealth = player.GetComponent<PlayerHealth>();
        swordAnimator = GetComponentInChildren<Animator>();
        isSwinging = false;
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
                        //Debug.DrawRay(ray.origin, ray.direction * sightDistance, Color.yellow);
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

    public float GetPlayerDistance()
    {
        return Vector3.Distance(transform.position, player.transform.position);
    }
    
    public void RotateTowardsTransform(Transform trans)
    {
        Vector3 direction = trans.transform.position - agent.transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        agent.transform.rotation = Quaternion.RotateTowards(
            agent.transform.rotation,
            targetRotation,
            agent.angularSpeed * Time.deltaTime
        );
    }

    public void DamagePlayer()
    {
        playerHealth.TakeDamage(damage);
    }

    public void SearchLocation(Vector3 location)
    {
        if (stateMachine.activeState is PatrolState)
        {
            Debug.Log("Enemy available for player area check : " + location);
            
            LastKnownPos = location;
            stateMachine.ChangeState(new SearchState());
        }
        else
        {
            Debug.Log("This enemy is currently unavailable");
        }
    }

    public void SwingSword()
    {
        StartCoroutine(SwingCouroutine());
    }

    private IEnumerator SwingCouroutine()
    {
        isSwinging = true;
        swordAnimator.SetBool("IsSwinging", isSwinging);
        
        yield return new WaitForSeconds(0.5f);
        
        DamagePlayer();
        
        isSwinging = false;
        swordAnimator.SetBool("IsSwinging", isSwinging);
    }

    public void ChangeEyeColour(Color newColor)
    {
        eye1.material.color = newColor;
        eye2.material.color = newColor;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Barrel"))
        {
           //Debug.Log("Adding Barrel : " + other.transform.parent.name);
                   
            barrelList.Add(other.gameObject); 
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Barrel"))
        {
            //Debug.Log("Removing Barrel : " + other.transform.parent.name);
                    
            barrelList.Remove(other.gameObject);
        }
    }
}
