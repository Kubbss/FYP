using UnityEngine;

public class AttackState : BaseState
{
    private float moveTimer;
    private float losePlayerTimer;
    
    
    public override void Enter()
    {
        
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            enemy.transform.LookAt(enemy.Player.transform);
            if (moveTimer > 0.5)
            {
                enemy.Agent.SetDestination(enemy.Player.transform.position);
                moveTimer = 0;
            }
            enemy.LastKnownPos = enemy.Player.transform.position;
        }
        else
        {
            losePlayerTimer += Time.deltaTime;
            if (losePlayerTimer > 4)
            {
                stateMachine.ChangeState(new SearchState());
            }
        }
    }

    public override void Exit()
    {
        
    }
}
