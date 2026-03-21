using UnityEngine;
using System.Collections.Generic;
using Unity.MLAgents;

public class SearchState : BaseState
{
    private float searchTimer;
    private float moveTimer;
    private float waitTimer;
    
    private List<GameObject> barrelList;
    
    private float barrelSearchTimer;
    private int barrelIndex = 0;
    private bool isSearching;
    private GameObject currentBarrel;
    
    public override void Enter()
    {
        enemy.Agent.SetDestination(enemy.LastKnownPos);
        enemy.Agent.speed = 6f;
        barrelSearchTimer = 11f;
    }

    public override void Perform()
    {
        SearchBarrelsInArea();
        
        if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }
        
        if (enemy.GetPlayerDistance() <= enemy.hearDistance)
        {
            if (enemy.PlayerSprint.IsSprinting)
            {
                enemy.LastKnownPos = enemy.Player.transform.position;
                
                Debug.Log("Heard Player at: " + enemy.LastKnownPos+ ": Going to investigate");
                stateMachine.ChangeState(new SearchState());
            }
        }

        if (enemy.Agent.remainingDistance < enemy.Agent.stoppingDistance && !isSearching)
        {
            searchTimer += Time.deltaTime;
            moveTimer += Time.deltaTime;

            if (moveTimer > Random.Range(2, 5))
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 10));
                moveTimer = 0;
            }

            if (searchTimer > 10)
            {
                stateMachine.ChangeState(new PatrolState());
            }
        }
    }

    public override void Exit()
    {
        
    }

    private void SearchBarrelsInArea()
    {
        barrelList = enemy.ListVisibleBarrels();

        foreach (GameObject barrel in barrelList)
        {
            
        }
    }
}
