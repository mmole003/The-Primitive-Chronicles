using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    public SwordAttack swordAttack;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            swordAttack.DamageEnemy(other.gameObject);
        }
    }
}