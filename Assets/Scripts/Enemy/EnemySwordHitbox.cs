using UnityEngine;

public class EnemySwordHitbox : MonoBehaviour
{
    public EnemyAttack enemyAttack;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerBlock playerBlock = other.GetComponentInChildren<PlayerBlock>();

            if (playerBlock != null && playerBlock.IsBlocking)
            {
                Debug.Log("Player blocked the attack!");
                return;
            }

            enemyAttack.DamagePlayer(other.gameObject);
        }
    }
}