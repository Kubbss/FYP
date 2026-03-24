using UnityEngine;

public class AgentSightReward : MonoBehaviour
{
    [SerializeField] private MazeAgent mazeAgent;
    [SerializeField] private Transform playerTarget;
    [SerializeField] private float sightDistance = 40f;
    [SerializeField] private float eyeHeight = 1f;
    [SerializeField] private float fieldOfView = 90f;

    private bool couldSeePlayerLastFrame = false;

    private void Update()
    {
        bool canSeePlayer = CheckCanSeePlayer();

        if (canSeePlayer && !couldSeePlayerLastFrame)
        {
            mazeAgent.RewardFirstSight();
        }

        couldSeePlayerLastFrame = canSeePlayer;
    }

    private bool CheckCanSeePlayer()
    {
        if (mazeAgent == null || playerTarget == null)
            return false;

        Vector3 origin = transform.position + Vector3.up * eyeHeight;
        Vector3 target = playerTarget.position + Vector3.up * eyeHeight;
        Vector3 dir = target - origin;

        float distanceToPlayer = dir.magnitude;

        if (distanceToPlayer > sightDistance)
            return false;

        float angleToPlayer = Vector3.Angle(transform.forward, dir);
        if (angleToPlayer > fieldOfView * 0.5f)
            return false;

        if (Physics.Raycast(origin, dir.normalized, out RaycastHit hit, distanceToPlayer))
        {
            return hit.transform.CompareTag("Player");
        }

        return false;
    }

    public void ResetSight()
    {
        couldSeePlayerLastFrame = false;
    }
}