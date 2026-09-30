using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;

    [Header("Knockback")]
    public float knockbackForce = 3f;
    public float knockbackDuration = 0.15f;

    [Header("Hit Flash")]
    public float flashDuration = 0.1f;

    private int currentHealth;

    private NavMeshAgent agent;

    private Renderer[] renderers;

    private Material[] originalMaterials;
    private Material flashMaterial;

    void Start()
    {
        currentHealth = maxHealth;

        // Find NavMeshAgent if the enemy has one
        agent = GetComponent<NavMeshAgent>();

        // Find all renderers on the enemy and its children
        renderers = GetComponentsInChildren<Renderer>();

        // Store the original materials
        originalMaterials = new Material[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].material;
        }

        // Create a white material for the flash
        flashMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        flashMaterial.color = Color.white;
    }

        public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log("Enemy took " + damage + " damage!");

        // Flash white
        StartCoroutine(FlashWhite());

        // Knock enemy backward
        StartCoroutine(Knockback());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator FlashWhite()
    {
        // Change every renderer to the white material
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = flashMaterial;
        }

        yield return new WaitForSeconds(flashDuration);

        // Restore original materials
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = originalMaterials[i];
        }
    }

    IEnumerator Knockback()
    {
        if (agent == null)
        {
            yield break;
        }

        // Stop the NavMeshAgent from controlling the enemy
        agent.isStopped = true;

        Vector3 direction = transform.position - GameObject.FindGameObjectWithTag("Player").transform.position;

        direction.y = 0f;

        direction.Normalize();

        float elapsed = 0f;

        while (elapsed < knockbackDuration)
        {
            elapsed += Time.deltaTime;

            float progress = elapsed / knockbackDuration;

            // Smoothly move backwards
            transform.position += direction * knockbackForce * Time.deltaTime;

            yield return null;
        }

        agent.isStopped = false;
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        Destroy(gameObject);
    }
}