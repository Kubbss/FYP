using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    private int waypointIndex;
    private float waitTimer;
    private float lookTimer;
    
    private float barrelSearchTimer;
    private float barrelChanceCheckFreqTimer = 6;
    private int barrelChance = 5; // 1/X chance to search a barrel;
    
    private bool canSearchBarrel = false;
    private bool isSearchingBarrel = false;

    private GameObject currentBarrel;

    private List<GameObject> barrelList;
    
    public override void Enter()
    {
        ShuffleList(enemy.path.waypoints);
        enemy.Agent.speed = 4f;
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }
        
        //Debug.Log(enemy.GetPlayerDistance());
        
        if (enemy.GetPlayerDistance() <= enemy.hearDistance)
        {
            if (enemy.PlayerSprint.IsSprinting)
            {
                enemy.LastKnownPos = enemy.Player.transform.position;
                
                Debug.Log("Heard Player at: " + enemy.LastKnownPos+ ": Going to investigate");
                stateMachine.ChangeState(new SearchState());
            }
        }
        
        CheckIfCanSearchBarrel();
        PatrolCycle();
    }

    public override void Exit()
    {
        
    }

    private void PatrolCycle()
    {
        if (canSearchBarrel)
        {
            GoSearchBarrel();
        }
        else if (enemy.Agent.remainingDistance < 1f)
        {
            if (lookTimer == 0)
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 2f));
                lookTimer = 0.1f;
            }    
            else if (lookTimer > 0)
            {
                lookTimer += Time.deltaTime;
            }
            
            waitTimer += Time.deltaTime;
            
            if (waitTimer > 3)
            {
                if (waypointIndex < enemy.path.waypoints.Count - 1)
                {
                    waypointIndex++;
                }
                else
                {
                    waypointIndex = 0;
                    ShuffleList(enemy.path.waypoints);
                }
                
                if (lookTimer > 0.6)
                {
                    enemy.Agent.SetDestination(enemy.path.waypoints[waypointIndex].position);
                    waitTimer = 0;
                    lookTimer = 0;
                }
            }
        }
    }

    private void CheckIfCanSearchBarrel()
    {
        if (!canSearchBarrel)
        {
            barrelSearchTimer += Time.deltaTime;
            barrelChanceCheckFreqTimer += Time.deltaTime;

            if (barrelSearchTimer > 30f && barrelChanceCheckFreqTimer > 5f)
            {
                barrelList = enemy.ListVisibleBarrels();

                if (barrelList is not null && barrelList.Count > 0)
                {
                    if (1 == Random.Range(1, barrelChance))
                    {
                        canSearchBarrel = true;
                        barrelSearchTimer = 0;
                        barrelChanceCheckFreqTimer = 0f;
                        
                        Debug.Log("Successful Check Chance");
                    }
                    else
                    {
                        barrelChanceCheckFreqTimer = 0f;
                        
                        Debug.Log("Failed Check Chance");
                    }
                }
            }
        }
    }

    private void GoSearchBarrel()
    {
        if (!isSearchingBarrel)
        {
            currentBarrel = barrelList[Random.Range(0, barrelList.Count - 1)];
            enemy.Agent.SetDestination(currentBarrel.transform.position + (currentBarrel.transform.forward * 1.4f));
            isSearchingBarrel = true;
            waitTimer = 0;
            lookTimer = 1;
        }
        else if (enemy.Agent.remainingDistance < 2)
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
                    Debug.Log("No Player in Barrel");
                }
                
                enemy.Agent.SetDestination(enemy.path.waypoints[waypointIndex].position);
                canSearchBarrel = false;
                isSearchingBarrel = false;
            }
        }
    }

    private void ShuffleList(List<Transform> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
