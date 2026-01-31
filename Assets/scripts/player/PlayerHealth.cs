using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int Health;
    public int MaxHealth = 100;
    public bool IsAlive;

    void Start()
    {
        Health = MaxHealth;
        IsAlive = true;
    }

    void Update()
    {
        if (Health <= 0)
        {
            Health = 0;
            IsAlive = false;
        }
        else
        {
            IsAlive = true;
        }
    }

    public void TakeDamage(int Damage)
    {
        Health -= Damage;
        Debug.Log("Dano recebido! Vida atual: " + Health);
        if (Health <= 0) Die();
    }

    public void Cure(int amount)
    {
        Health += amount;
        if (Health > MaxHealth) Health = MaxHealth;
        Debug.Log("Cura recebida! Vida atual: " + Health);
    }

    void Die()
    {
        Health = 0;
        IsAlive = false;
        Debug.Log("O jogador morreu!");
    }
}