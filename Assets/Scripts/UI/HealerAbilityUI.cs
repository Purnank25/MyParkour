using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealerUI : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] Slider healthSlider;
    [SerializeField] TextMeshProUGUI healthText;

    [Header("Water Sphere Ammo")]
    [SerializeField] Slider rechargeSlider;
    [SerializeField] TextMeshProUGUI ammoText;

    [Header("References")]
    [SerializeField] HealthSystem healthSystem;
    [SerializeField] WaterSphereShooter waterSphereShooter;

    void Start()
    {
        healthSlider.maxValue = healthSystem.MaxHealth;
        rechargeSlider.maxValue = 1f;
    }

    void Update()
    {
        // health
        healthSlider.value = healthSystem.CurrentHealth;
        healthText.text = (int)healthSystem.CurrentHealth + " / " + (int)healthSystem.MaxHealth;

        // ammo
        rechargeSlider.value = waterSphereShooter.RechargeProgress;
        ammoText.text = waterSphereShooter.IsRecharging
            ? "Recharging..."
            : waterSphereShooter.CurrentAmmo + " / " + waterSphereShooter.MaxAmmo;
    }
}