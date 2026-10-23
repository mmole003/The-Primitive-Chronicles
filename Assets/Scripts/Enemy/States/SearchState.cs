
using UnityEngine;
using UnityEngine.AI;

public class SearchState : BaseState
{
    private float searchTimer;
    private float moveTimer;

    private float searchDuration = 10f;
    private float moveInterval;

    public override void Enter()
    {
        searchTimer = 0f;
        moveTimer = 0f;
        moveInterval = Random.Range(2f, 4f);

        enemy.Agent.isStopped = false;
        enemy.Agent.SetDestination(enemy.LastKnownPlayerPosition);
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
            return;
        }

        // Keep the search timer running even while moving.
        searchTimer += Time.deltaTime;
        moveTimer += Time.deltaTime;

        // Once the enemy reaches the last known position,
        // start moving between nearby search locations.
        if (!enemy.Agent.pathPending &&
            enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
        {
            if (moveTimer >= moveInterval)
            {
                MoveToRandomSearchPosition();

                moveTimer = 0f;
                moveInterval = Random.Range(2f, 4f);
            }
        }

        // Give up searching after the time limit.
        if (searchTimer >= searchDuration)
        {
            stateMachine.ChangeState(new PatrolState());
            return;
        }
    }

    private void MoveToRandomSearchPosition()
    {
        // Try several random nearby positions.
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomOffset = Random.insideUnitSphere * 8f;
            randomOffset.y = 0f;

            Vector3 candidatePosition =
                enemy.transform.position + randomOffset;

            // Snap the candidate onto a nearby walkable NavMesh position.
            if (NavMesh.SamplePosition(
                candidatePosition,
                out NavMeshHit hit,
                8f,
                NavMesh.AllAreas))
            {
                if (enemy.Agent.SetDestination(hit.position))
                {
                    return;
                }
            }
        }
    }

    public override void Exit()
    {
        if (enemy != null && enemy.Agent != null)
        {
            enemy.Agent.isStopped = true;
        }
    }
}
