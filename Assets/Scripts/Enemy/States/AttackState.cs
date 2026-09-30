using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private float moveTimer;
    private float losePlayerTimer;

    public override void Enter()
    {
    }

    public override void Exit()
    {
        // stop movement when leaving state
        if (enemy != null && enemy.Agent != null)
            enemy.Agent.isStopped = true;
    }

    public override void Perform()
    {
        if (enemy.Player == null)
        {
            // If player reference is lost, return to patrol
            stateMachine.ChangeState(new PatrolState());
            return;
        }

        if (enemy.CanSeePlayer())
        {
            losePlayerTimer = 0f;
            moveTimer += Time.deltaTime;

            // Ensure agent is active and chase the player's position
            enemy.Agent.isStopped = false;
            enemy.Agent.SetDestination(enemy.Player.transform.position);

            // Face the player while chasing
            var lookPos = enemy.Player.transform.position;
            lookPos.y = enemy.transform.position.y;
            enemy.transform.LookAt(lookPos);

            // Optional: small evasive/random movement every few seconds
            if (moveTimer > Random.Range(3f, 7f))
            {
                // add a small random offset while still moving toward the player
                Vector3 randomOffset = Random.insideUnitSphere * 1.5f;
                randomOffset.y = 0f;
                enemy.Agent.SetDestination(enemy.Player.transform.position + randomOffset);
                moveTimer = 0f;
            }
        }
        else
        {
            enemy.Agent.isStopped = false;
            losePlayerTimer += Time.deltaTime;
            if (losePlayerTimer > 8f)
            {
                // Lost the player -> go back to patrol
                stateMachine.ChangeState(new PatrolState());
            }
        }
    }
}