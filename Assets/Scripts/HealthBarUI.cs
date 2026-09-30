using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    //Listener for the PlayerHealth script to update the health bar when the player's health changes
    public PlayerHealth playerHealth;
    public Slider healthBar;

    //Enables the health bar to listen for changes in the player's health when the script is enabled, and disables it when the script is disabled
    private void OnEnable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHealthBar;
        }
    }
    //Disables the health bar from listening for changes in the player's health when the script is disabled
    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthBar;
        }
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }
}