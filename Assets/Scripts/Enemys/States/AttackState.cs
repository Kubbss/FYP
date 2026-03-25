using JetBrains.Annotations;
using UnityEngine;

public class AttackState : BaseState
{
    private float moveTimer;
    private float losePlayerTimer;
    private float damageTimer;
    
    public override void Enter()
    {
        enemy.Agent.speed = 7f;
        
        enemy.ChangeEyeColour(Color.red);
    }

    public override void Perform()
    {
        damageTimer += Time.deltaTime;
        
        if (enemy.CanSeePlayer())
        {
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            //enemy.transform.LookAt(enemy.Player.transform);
            if (moveTimer > 0.5)
            {
                enemy.Agent.SetDestination(enemy.Player.transform.position);
                moveTimer = 0;
            }
            enemy.LastKnownPos = enemy.Player.transform.position;

            if (enemy.GetPlayerDistance() < enemy.attackRange)
            {
                if (damageTimer > enemy.damageInterval)
                {
                    enemy.SwingSword();
                    damageTimer = 0;
                }
            }
        }
        else
        {
            losePlayerTimer += Time.deltaTime;

            if (enemy.GetPlayerDistance() <= enemy.hearDistance)
            {
                if (enemy.PlayerSprint.IsSprinting)
                {
                    enemy.LastKnownPos = enemy.Player.transform.position;
                
                    Debug.Log("Heard Player at: " + enemy.LastKnownPos);
                }
            }
            
            if (enemy.Agent.remainingDistance < 1f)
            {
                enemy.RotateTowardsTransform(enemy.Player.transform);
            }
            
            if (losePlayerTimer > 1)
            {
                enemy.LastKnownPos = enemy.Player.transform.position;
                stateMachine.ChangeState(new SearchState());
            }
        }
    }

    public override void Exit()
    {
        
    }
}
