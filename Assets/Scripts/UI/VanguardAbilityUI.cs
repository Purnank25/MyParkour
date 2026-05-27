using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VanguardAbilityUI : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] Slider healthSlider;
    [SerializeField] TextMeshProUGUI healthText;

    [Header("Shield")]
    [SerializeField] Slider shieldSlider;
    [SerializeField] TextMeshProUGUI shieldText;
    [SerializeField] Slider shieldCooldownSlider;
    [SerializeField] TextMeshProUGUI shieldStatusText;

    [Header("References")]
    [SerializeField] HealthSystem healthSystem;
    [SerializeField] VanguardShield vanguardShield;

    void Start()
    {
        // health
        healthSlider.maxValue = healthSystem.MaxHealth;
        healthSlider.value = healthSystem.MaxHealth;

        // shield
        shieldSlider.maxValue = vanguardShield.MaxShieldHealth;
        shieldSlider.value = vanguardShield.MaxShieldHealth;
        shieldCooldownSlider.maxValue = 1f;
        shieldCooldownSlider.value = 0f;
    }

    void Update()
    {
        // health
        healthSlider.value = healthSystem.CurrentHealth;
        healthText.text = (int)healthSystem.CurrentHealth + " / " + (int)healthSystem.MaxHealth;

        // shield
        shieldSlider.value = vanguardShield.CurrentShieldHealth;
        shieldText.text = (int)vanguardShield.CurrentShieldHealth + " / " + (int)vanguardShield.MaxShieldHealth;

        // cooldown bar and status text
        if (vanguardShield.IsOnCooldown)
        {
            shieldCooldownSlider.value = vanguardShield.CooldownProgress;
            shieldStatusText.text = "Shield Recharging...";
            shieldStatusText.color = Color.red;
        }
        else if (vanguardShield.IsActive)
        {
            shieldCooldownSlider.value = 1f;
            shieldStatusText.text = "Shield Active";
            shieldStatusText.color = Color.cyan;
        }
        else
        {
            shieldCooldownSlider.value = 1f;
            shieldStatusText.text = "Shield Ready";
            shieldStatusText.color = Color.green;
        }
    }
}