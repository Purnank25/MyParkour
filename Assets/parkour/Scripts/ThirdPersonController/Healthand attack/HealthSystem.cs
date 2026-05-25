using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;
    float currentHealth;

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

        Debug.Log(gameObject.name + " took " + damage + " damage. HP: " + currentHealth);

        if (IsDead)
            Die();
    }

    void Die()
    {
        Debug.Log(gameObject.name + " died");
        // play death animation or destroy
        gameObject.SetActive(false);
    }
}