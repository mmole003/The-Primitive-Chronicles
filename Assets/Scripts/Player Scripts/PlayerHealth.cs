using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour

{
    //Creates an observer pattern for the health bar to listen for changes in the player's health
    public event System.Action<int, int> OnHealthChanged;

    [Header("Health")]
    public int maxHealth = 100;

    private int currentHealth;

    public int CurrentHealth => currentHealth;

    //Checks if the player is dead or not, and allows other scripts to access this information
    public bool IsDead { get; private set; } = false;

    [Header("Health Bar")]
    public Slider healthBar;

    [Header("Game Over")]
    public GameOver gameOver;

    void Start()
    {
            currentHealth = maxHealth;

            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        //debug
        Debug.Log("Player took " + damage + " damage!");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //Script to handle player death
        IsDead = true;

        Debug.Log("Player died!");

        if (gameOver != null)
        {
            gameOver.ShowGameOver();
        }

        Time.timeScale = 0f;
    }
}