using UnityEngine;

public class PlayerHealth : MonoBehaviour, HealthComponent
{
    public Healthbar healthUI;
    private PlayerStats playerStats;

    public float currentHealth; //Actual health value
    public float maxHealth; //Max health value

    //The Health property from the interface
    public float Health
    {
        get { return currentHealth; }
        set { currentHealth = Mathf.Clamp(value, 0f, maxHealth);}
    }

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();

        Health = playerStats.MaxHealth;
        healthUI.SetMaxHealth(Health);

    }

    public void takeDamage(float amount) {
        Health -= CalculateDamgeTaken(amount);
        healthUI.SetHealth(Health);
    }

    public float CalculateDamgeTaken(float amount) {
        return Mathf.Max(amount - playerStats.Defense);
    }

    public void AddHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        healthUI.SetHealth(Health);
        // Optionally update UI here if needed
        Debug.Log("Player Health: " + currentHealth); // Example
    }

}
