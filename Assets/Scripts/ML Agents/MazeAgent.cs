using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using System.Collections;

public class MazeAgent : Agent
{
    [SerializeField] private GameObject playerTarget;
    [SerializeField] private Renderer groundRenderer;
    [SerializeField] private GameObject trainingArea;
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] private float distanceRewardScale = 0.01f;
    [SerializeField] private float firstSightReward = 0.5f;
    [SerializeField] private AgentSightReward agentSightReward;
    
    public Transform[] SpawnPoints;
    
    private PlayerNavmeshAgent agent;
    
    private float previousDistanceToPlayer;
    private bool sawPlayerThisEpisode;
    private float bestDistanceToPlayer;
    
    private Coroutine flashGroundCoroutine;
    private Color defaultGroundColor;
    
    public float wallHitPenalty = 0.05f;
    public float totalPenaltyOverTime = 2f;
    public float reward = 2f;

    private int currentEpisode;
    [SerializeField]
    private float cumalitiveReward;
    
    public override void Initialize()
    {
        defaultGroundColor = groundRenderer.material.color;
        currentEpisode = 0;
        cumalitiveReward = 0f;
        
        if (groundRenderer is not null)
        {
            defaultGroundColor = groundRenderer.material.color;
        }
        
        agent = playerTarget.GetComponent<PlayerNavmeshAgent>();
    }

    public override void OnEpisodeBegin()
    {
        FlashLastColor();
        
        currentEpisode++;
        cumalitiveReward = 0f;

        ResetArea();
        
        agent.Reset();
        
        float startDistance = Vector3.Distance(transform.position, playerTarget.transform.position);
        previousDistanceToPlayer = startDistance;
        bestDistanceToPlayer = startDistance;

        sawPlayerThisEpisode = false;

        if (agentSightReward != null)
        {
            agentSightReward.ResetSight();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var moveActions = actionsOut.DiscreteActions;

        moveActions[0] = 0;
        
        if (Input.GetKey(KeyCode.W))
        {
            moveActions[0] = 1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveActions[0] = 3;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveActions[0] = 2;
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        MoveAgent(actionBuffers.DiscreteActions);
        
        AddReward(-totalPenaltyOverTime / MaxStep);
        
        float currentDistanceToPlayer = Vector3.Distance(transform.position, playerTarget.transform.position);
        
        if (currentDistanceToPlayer < bestDistanceToPlayer)
        {
            float improvement = bestDistanceToPlayer - currentDistanceToPlayer;
            AddReward(improvement * distanceRewardScale);
            bestDistanceToPlayer = currentDistanceToPlayer;
        }

        previousDistanceToPlayer = currentDistanceToPlayer;
        
        cumalitiveReward = GetCumulativeReward();
    }

    private void MoveAgent(ActionSegment<int> actionSegment)
    {
        var moveAction = actionSegment[0];

        switch (moveAction)
        {
            case 1:
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
                break;
            case 3:
                transform.Rotate(0f, -rotationSpeed * Time.deltaTime, 0f);
                break;
            case 2:
                transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
                break;
        }
    }

    private void ResetArea()
    {
        transform.localRotation = Quaternion.identity;
        transform.localPosition = new Vector3(18f, 0.1f, -18f);
        
        float randomAngle = Random.Range(0f, 360f);
        transform.localRotation = Quaternion.Euler(0f, randomAngle, 0f);
        
        //playerTarget.localPosition = new Vector3(-18f, 0.3f, 18f);
        SpawnPlayerTargetRandom();
        
        trainingArea.transform.localRotation = Quaternion.Euler(0f, Random.Range(0, 360f), 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GoalReached();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            AddReward(-wallHitPenalty);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            AddReward((-wallHitPenalty/5) * Time.fixedDeltaTime);
        }
    }

    private void GoalReached()
    {
        groundRenderer.material.color = Color.lawnGreen;
        
        AddReward(reward);
        cumalitiveReward = GetCumulativeReward();
        
        EndEpisode();
    }

    public void FlashLastColor()
    {
        if (groundRenderer is not null && cumalitiveReward is not 0f)
        {
            Color flashcolor = (cumalitiveReward > 0f) ? Color.green : Color.red;

            if (flashGroundCoroutine is not null)
            {
                StopCoroutine(flashGroundCoroutine);
            }

            flashGroundCoroutine = StartCoroutine(FlashGround(flashcolor, 3.0f));
        }
    }

    private IEnumerator FlashGround(Color targetColor, float time)
    {
        float elapsedTime = 0f;

        groundRenderer.material.color = targetColor;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            groundRenderer.material.color = Color.Lerp(targetColor, defaultGroundColor, elapsedTime / time);
            yield return null;
        }
    }

    public void SpawnPlayerTargetRandom()
    {
        Transform sp = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
        
        playerTarget.transform.position = sp.position;
    }
    
    public void RewardFirstSight()
    {
        if (!sawPlayerThisEpisode)
        {
            sawPlayerThisEpisode = true;
            AddReward(firstSightReward);
        }
    }
}
