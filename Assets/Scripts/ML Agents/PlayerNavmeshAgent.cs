using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerNavmeshAgent : MonoBehaviour
{
    [SerializeField] private Path path;

    private NavMeshAgent agent;
    private List<Transform> shuffledWaypoints = new List<Transform>();
    private int currentWaypointIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        shuffledWaypoints = new List<Transform>(path.waypoints);
        ShuffleWaypoints();

        agent.SetDestination(shuffledWaypoints[currentWaypointIndex].position);
    }

    void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= shuffledWaypoints.Count)
            {
                currentWaypointIndex = 0;
                ShuffleWaypoints();
            }

            agent.SetDestination(shuffledWaypoints[currentWaypointIndex].position);
        }
    }

    private void ShuffleWaypoints()
    {
        for (int i = 0; i < shuffledWaypoints.Count; i++)
        {
            int randomIndex = Random.Range(i, shuffledWaypoints.Count);

            (shuffledWaypoints[i], shuffledWaypoints[randomIndex]) = (shuffledWaypoints[randomIndex], shuffledWaypoints[i]);
        }
    }

    public void Reset()
    {
        currentWaypointIndex = 0;
        
        shuffledWaypoints = new List<Transform>(path.waypoints);
        ShuffleWaypoints();
        
        agent.SetDestination(shuffledWaypoints[currentWaypointIndex].position);
    }
}