using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityUI : MonoBehaviour
{
    [SerializeField] Slider energySlider;
    [SerializeField] Slider rechargeSlider;
    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] EnergySystem energySystem;
    [SerializeField] HeavyShot heavyShot;

    void Start()
    {
        energySlider.maxValue = energySystem.MaxEnergy;
        rechargeSlider.maxValue = 1f;
    }

    void Update()
    {
        energySlider.value = energySystem.CurrentEnergy;
        ammoText.text = heavyShot.IsRecharging
            ? "Recharging..."
            : heavyShot.CurrentAmmo + " / " + heavyShot.MaxAmmo;
        rechargeSlider.value = heavyShot.RechargeProgress;
    }
}