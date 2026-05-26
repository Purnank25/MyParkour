using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] Slider healthSlider;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] HealthSystem healthSystem;

    void Start()
    {
        healthSlider.maxValue = healthSystem.MaxHealth;
        healthSlider.value = healthSystem.MaxHealth;
    }
    void Update()
    {
        healthSlider.value = healthSystem.CurrentHealth;
        healthText.text = healthSystem.CurrentHealth + " / " + healthSystem.MaxHealth;
    }
}