
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
    public bool IsAttacking => isAttacking;

    private Quaternion startingRotation;
    private Coroutine swingRoutine;

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

        if (GameManager.Instance != null &&
    GameManager.Instance.CurrentState != GameManager.GameState.Playing)
        {
            return;
        }
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame &&
            !isAttacking)
        {
            swingRoutine = StartCoroutine(SwingSword());
        }
    }

    IEnumerator SwingSword()
    {
        if (swordHitbox == null)
        {
            isAttacking = false;
            swingRoutine = null;
            yield break;
        }

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

        FinishAttack();
    }

    private void FinishAttack()
    {
        transform.localRotation = startingRotation;

        if (swordHitbox != null)
        {
            swordHitbox.enabled = false;
        }

        isAttacking = false;
        swingRoutine = null;
    }

    public void CancelAttack()
    {
        if (swingRoutine != null)
        {
            StopCoroutine(swingRoutine);
            swingRoutine = null;
        }

        FinishAttack();
    }

    public void DamageEnemy(GameObject enemy)
    {
        if (enemy == null || hitEnemies.Contains(enemy))
            return;

        hitEnemies.Add(enemy);

        EnemyHealth health = enemy.GetComponent<EnemyHealth>();

        if (health != null)
        {
            health.TakeDamage(damage);
        }
    }
}
