using System.Collections;
using UnityEngine;

public class PlayerDamageFlash : MonoBehaviour
{
    public float flashDuration = 0.15f;

    private Renderer[] renderers;
    private Material[] materials;
    private Color[] originalColors;

    private int previousHealth;

    private PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();

        if (playerHealth == null)
        {
            Debug.LogError("PlayerDamageFlash: No PlayerHealth found.");
            return;
        }

        // Find all renderers on the player and its children
        renderers = GetComponentsInChildren<Renderer>();

        materials = new Material[renderers.Length];
        originalColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            materials[i] = renderers[i].material;
            originalColors[i] = materials[i].color;
        }

        // Player starts at max health
        previousHealth = playerHealth.maxHealth;

        // Subscribe to the health event
        playerHealth.OnHealthChanged += OnHealthChanged;
    }

    private void OnHealthChanged(int currentHealth, int maxHealth)
    {
        // Check whether health went down
        if (currentHealth < previousHealth)
        {
            StartCoroutine(Flash());
        }

        // Remember the new health value
        previousHealth = currentHealth;
    }

    private IEnumerator Flash()
    {
        // Turn every part of the player white
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = Color.white;
        }

        yield return new WaitForSeconds(flashDuration);

        // Restore the original colors
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= OnHealthChanged;
        }
    }
}