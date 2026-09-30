using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;

    private int currentHealth;
    public bool IsDead { get; private set; } = false;

    [Header("Health Bar")]
    public Slider healthBar;

    [Header("Game Over")]
    public GameOver gameOver;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Player took " + damage + " damage!");

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        IsDead = true;

        Debug.Log("Player died!");

        if (gameOver != null)
        {
            gameOver.ShowGameOver();
        }

        Time.timeScale = 0f;
    }
}