using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;
    float currentHealth;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsDead => currentHealth <= 0;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (IsDead) return;
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (IsDead)
            Die();
    }

    void Die()
    {
        gameObject.SetActive(false);
    }
    public void Heal(float amount)
    {
        if (IsDead) return;
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
}