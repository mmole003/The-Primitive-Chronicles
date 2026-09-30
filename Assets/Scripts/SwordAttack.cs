using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float swingDuration = 0.3f;
    public float swingAngle = 120f;
    public int damage = 25;

    [Header("References")]
    public Collider swordHitbox;

    private bool isAttacking = false;
    private Quaternion startingRotation;

    private HashSet<GameObject> hitEnemies = new HashSet<GameObject>();

    void Start()
    {
        startingRotation = transform.localRotation;

        if (swordHitbox != null)
        {
            swordHitbox.enabled = false;
        }
        else
        {
            Debug.LogError("SwordAttack: Sword Hitbox has not been assigned!");
        }
    }

    void Update()
    {
        // Left mouse button
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame &&
            !isAttacking)
        {
            StartCoroutine(SwingSword());
        }
    }

    IEnumerator SwingSword()
    {
        isAttacking = true;

        hitEnemies.Clear();

        swordHitbox.enabled = true;

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

            transform.localRotation =
                startingRotation *
                Quaternion.Euler(0f, angle, 0f);

            yield return null;
        }

        transform.localRotation = startingRotation;

        swordHitbox.enabled = false;

        isAttacking = false;
    }

    public void DamageEnemy(GameObject enemy)
    {
        if (hitEnemies.Contains(enemy))
            return;

        hitEnemies.Add(enemy);

        EnemyHealth health = enemy.GetComponent<EnemyHealth>();

        if (health != null)
        {
            health.TakeDamage(damage);
        }
    }
}