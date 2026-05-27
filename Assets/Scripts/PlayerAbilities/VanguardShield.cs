using UnityEngine;

public class VanguardShield : MonoBehaviour
{
    [SerializeField] GameObject shieldObject;   // shield visual GameObject
    [SerializeField] float maxShieldHealth = 100f;
    [SerializeField] float cooldownDuration = 5f;

    float currentShieldHealth;
    bool isOnCooldown;
    float cooldownTimer;
    bool isActive;

    public float CurrentShieldHealth => currentShieldHealth;
    public float MaxShieldHealth => maxShieldHealth;
    public bool IsOnCooldown => isOnCooldown;
    public float CooldownProgress => cooldownTimer / cooldownDuration;
    public bool IsActive => isActive;

    void Start()
    {
        currentShieldHealth = maxShieldHealth;
        shieldObject.SetActive(false);
    }

    void Update()
    {
        if (isOnCooldown)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= cooldownDuration)
            {
                isOnCooldown = false;
                cooldownTimer = 0f;
                currentShieldHealth = maxShieldHealth;
                Debug.Log("Shield recharged");
            }
            return;
        }

        // hold right click to keep shield up
        if (Input.GetMouseButton(1) && !isOnCooldown && currentShieldHealth > 0)
            ActivateShield();
        else
            DeactivateShield();
    }

    void ActivateShield()
    {
        isActive = true;
        shieldObject.SetActive(true);
    }

    void DeactivateShield()
    {
        isActive = false;
        shieldObject.SetActive(false);
    }

    public void TakeDamage(float damage)
    {
        if (!isActive) return;

        currentShieldHealth -= damage;
        currentShieldHealth = Mathf.Clamp(currentShieldHealth, 0, maxShieldHealth);

        Debug.Log("Shield health: " + currentShieldHealth);

        if (currentShieldHealth <= 0)
            BreakShield();
    }

    void BreakShield()
    {
        DeactivateShield();
        isOnCooldown = true;
        cooldownTimer = 0f;
        Debug.Log("Shield broken, cooling down");
    }
}