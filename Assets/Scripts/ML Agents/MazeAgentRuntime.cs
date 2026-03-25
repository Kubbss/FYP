using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;

public class MazeAgentRuntime : Agent
{
    [SerializeField] private GameObject playerTarget;
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float rotationSpeed = 180f;

    [SerializeField] private Renderer eye1;
    [SerializeField] private Renderer eye2;
    
    public float damage = 30f;
    
    private Animator swordAnimator;
    private bool isSwinging;
    private PlayerHealth playerHealth;

    private float damageTimer;
    
    public override void Initialize()
    {
        if (!Academy.Instance.IsCommunicatorOn)
        {
            MaxStep = 0;
        }
        
        swordAnimator = GetComponentInChildren<Animator>();
        isSwinging = false;
        
        playerHealth = playerTarget.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        damageTimer += Time.deltaTime;
        
        if (Vector3.Distance(transform.position, playerTarget.transform.position) < 3f)
        {
            if (damageTimer > 1f)
            {
                SwingSword();
                damageTimer = 0;
            }
        }

        if (CanSeePlayer())
        {
            eye1.material.color = Color.red;
            eye2.material.color = Color.red;
        }
        else
        {
            eye1.material.color = Color.limeGreen;
            eye2.material.color = Color.limeGreen;
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        MoveAgent(actionBuffers.DiscreteActions);
    }

    private void MoveAgent(ActionSegment<int> actionSegment)
    {
        var moveAction = actionSegment[0];

        switch (moveAction)
        {
            case 1:
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
                break;
            case 2:
                transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
                break;
            case 3:
                transform.Rotate(0f, -rotationSpeed * Time.deltaTime, 0f);
                break;
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
    
    private void DamagePlayer()
    {
        playerHealth.TakeDamage(damage);
    }
    
    public bool CanSeePlayer()
    {
        if (playerTarget is not null)
        {
            if (Vector3.Distance(transform.position, playerTarget.transform.position) <= 40)
            {
                Vector3 targetDirection = playerTarget.transform.position - transform.position - (Vector3.up * 0.6f);
                float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
                if (angleToPlayer >= -85 && angleToPlayer <= 85)
                {
                    Ray ray = new Ray(transform.position + (Vector3.up * 0.6f), targetDirection);
                    RaycastHit hitInfo = new RaycastHit();
                    if (Physics.Raycast(ray, out hitInfo, 40))
                    {
                        if (hitInfo.transform.gameObject == playerTarget)
                        {
                            Debug.DrawRay(ray.origin, ray.direction * 40, Color.red);
                            return true;
                        }
                    }
                    
                }
            }
        }

        return false;
    }
}