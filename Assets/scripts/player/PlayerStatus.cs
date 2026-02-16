using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus instance;

    [Header("Stamina")]
    public float Stamina;
    public float MaxStamina = 100f;
    public bool Cooldown_Stamina = false;

    [Header("Health")]
    public float Health;
    public float MaxHealth = 100f;

    [Header("Sleep")]
    public float Sleep;
    public float MaxSleep = 100f;

    [Header("Panic")]
    public float Panic;
    public float MaxPanic = 100f;

    [Header("Hallucination")]
    public float Hallucination;
    public float MaxHallucination = 100f;

    void Awake()
    {
        if (instance == null) {
            instance = this;
            Debug.Log("<color=white>[SYSTEM]</color> Status initialized successfully.");
        } else {
            Destroy(gameObject);
        }
        
        // Initialize status
        Health = MaxHealth;
        Sleep = 0f; 
        Panic = 0f; 
        Hallucination = 0f; 
    }

    void Update()
    {
        // Regenerate stamina when not in use
        if (!Cooldown_Stamina && Stamina < MaxStamina)
        {
            Stamina += 10f * Time.deltaTime;
            Stamina = Mathf.Clamp(Stamina, 0f, MaxStamina);
        }
    }

    // Stamina methods
    public void ConsumeStamina(float amount)
    {
        Stamina -= amount * Time.deltaTime;
        if (Stamina <= 0)
        {
            Cooldown_Stamina = true;
        }
        Stamina = Mathf.Clamp(Stamina, 0f, MaxStamina);
    }

    public void RegenerateStamina(float amount)
    {
        Stamina += amount * Time.deltaTime;
        if (Stamina >= MaxStamina)
        {
            Cooldown_Stamina = false;
        }
        Stamina = Mathf.Clamp(Stamina, 0f, MaxStamina);
    }

    public bool CanUseStamina()
    {
        return Stamina > 0 && !Cooldown_Stamina;
    }

    // Health methods
    public void TakeDamage(float damage)
    {
        Health -= damage;
        Health = Mathf.Clamp(Health, 0f, MaxHealth);
        
        if (Health <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        Health += amount;
        Health = Mathf.Clamp(Health, 0f, MaxHealth);
    }

    void Die()
    {
        Debug.Log("Player died!");
    }

    // Sleep methods
    public void IncreaseSleep(float amount)
    {
        Sleep += amount;
        Sleep = Mathf.Clamp(Sleep, 0f, MaxSleep);
    }

    public void DecreaseSleep(float amount)
    {
        Sleep -= amount;
        Sleep = Mathf.Clamp(Sleep, 0f, MaxSleep);
    }

    // Panic methods
    public void IncreasePanic(float amount)
    {
        Panic += amount;
        Panic = Mathf.Clamp(Panic, 0f, MaxPanic);
    }

    public void DecreasePanic(float amount)
    {
        Panic -= amount;
        Panic = Mathf.Clamp(Panic, 0f, MaxPanic);
    }

    // Hallucination methods
    public void IncreaseHallucination(float amount)
    {
        Hallucination += amount;
        Hallucination = Mathf.Clamp(Hallucination, 0f, MaxHallucination);
    }

    public void DecreaseHallucination(float amount)
    {
        Hallucination -= amount;
        Hallucination = Mathf.Clamp(Hallucination, 0f, MaxHallucination);
    }

}
