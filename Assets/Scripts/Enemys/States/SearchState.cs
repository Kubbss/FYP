using UnityEngine;
using System.Collections.Generic;
using Unity.MLAgents;

public class SearchState : BaseState
{
    private float searchTimer;
    private float moveTimer;
    private float waitTimer;
    
    private List<GameObject> barrelList;
    
    private int barrelIndex;
    private bool isBarrelSearching;
    private bool onABarrel;
    private int barrelSearchTimes;
    
    private GameObject currentBarrel;
    
    public override void Enter()
    {
        enemy.Agent.SetDestination(enemy.LastKnownPos);
        enemy.Agent.speed = 6f;
        onABarrel = false;
        barrelIndex = 0;
        barrelSearchTimes = 0;
        
        enemy.ChangeEyeColour(Color.red);
    }

    public override void Perform()
    {
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

        if (enemy.Agent.remainingDistance < enemy.Agent.stoppingDistance && !isBarrelSearching)
        {
            searchTimer += Time.deltaTime;
            moveTimer += Time.deltaTime;
            
            CheckForBarrelsInArea();
            
            if (isBarrelSearching)
                return;
            
            if (moveTimer > Random.Range(2, 5))
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 10));
                moveTimer = 0;
            }

            if (searchTimer > 15)
            {
                stateMachine.ChangeState(new PatrolState());
            }
        } 
        else if (isBarrelSearching)
        {
            SearchNearbyBarrels();
        }
        
    }

    public override void Exit()
    {
        
    }

    private void CheckForBarrelsInArea()
    {
        if (barrelSearchTimes >= 1)
            return;
        
        barrelList = enemy.ListVisibleBarrels();

        if (barrelList is not null && barrelList.Count > 0)
        {
            isBarrelSearching = true;
            
            Debug.Log("Barrel search possible with [" + barrelList.Count + "] barrels");
        }
    }

    private void SearchNearbyBarrels()
    {
        if (!onABarrel)
        {
            if ((barrelList.Count - 1) >= barrelIndex)
            {
                Debug.Log("Beginning Barrel Search of barrel [" + barrelIndex + "] out of [" + barrelList.Count + "] barrels");
                
                
                currentBarrel = barrelList[barrelIndex];
                onABarrel = true;
                waitTimer = 0;
                enemy.Agent.SetDestination(currentBarrel.transform.position + (currentBarrel.transform.forward * 1.4f)); 
            }
            else
            {
                Debug.Log("Finished Barrel Searching");
                
                onABarrel = false;
                isBarrelSearching = false;
                barrelIndex = 0;
                waitTimer = 0;
                barrelSearchTimes++;
            }
        }
        else
        {
            if (enemy.Agent.stoppingDistance < 2)
            {
                waitTimer += Time.deltaTime;
                
                
                enemy.RotateTowardsTransform(currentBarrel.transform);

                if (waitTimer > 3)
                {
                    if (enemy.CheckIfPlayerInBarrel(currentBarrel))
                    {
                        if (GameManager.gameOver)
                            return;
                        
                        Debug.Log("Found Player In Barrel");
                        
                        GameManager.instance.KillPlayer();
                    }
                    else
                    {
                        Debug.Log("No Player in Barrel [" + barrelIndex + "]");
                        
                        onABarrel = false;
                        barrelIndex++;
                    }
                }
            }
        }
    }
}
