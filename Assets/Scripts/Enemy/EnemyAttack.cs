using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public int damage = 10;

    [Header("Weapon")]
    public Transform weaponPivot;
    public float swingDuration = 0.3f;
    public float swingAngle = 120f;

    private Transform player;
    private bool canAttack = true;
    private Quaternion startingRotation;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        if (weaponPivot != null)
        {
            startingRotation = weaponPivot.localRotation;
        }
    }

    void Update()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        if (distance <= attackRange && canAttack)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        canAttack = false;

        if (weaponPivot != null)
        {
            float elapsed = 0f;

            while (elapsed < swingDuration)
            {
                elapsed += Time.deltaTime;

                float progress = elapsed / swingDuration;

                float angle = Mathf.Lerp(
                    -swingAngle / 2f,
                    swingAngle / 2f,
                    progress
                );

                weaponPivot.localRotation =
                    startingRotation *
                    Quaternion.Euler(0f, angle, 0f);

                yield return null;
            }

            weaponPivot.localRotation = startingRotation;
        }


        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }

    public void DamagePlayer(GameObject player)
    {
        PlayerHealth health = player.GetComponent<PlayerHealth>();

        if (health != null)
        {
            health.TakeDamage(damage);
        }
    }
}