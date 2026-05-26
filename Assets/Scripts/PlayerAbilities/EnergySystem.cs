using UnityEngine;

public class EnergySystem : MonoBehaviour
{
    [SerializeField] float maxEnergy = 100f;
    [SerializeField] float drainRate = 20f;      // energy per second while firing
    [SerializeField] float rechargeRate = 15f;   // energy per second when not firing
    [SerializeField] float rechargeDelay = 1f;   // seconds before recharge starts

    float currentEnergy;
    float rechargeTimer;
    bool isDraining;

    public float CurrentEnergy => currentEnergy;
    public float MaxEnergy => maxEnergy;
    public bool HasEnergy => currentEnergy > 0;

    void Start()
    {
        currentEnergy = maxEnergy;
    }

    void Update()
    {
        if (!isDraining)
        {
            rechargeTimer += Time.deltaTime;
            if (rechargeTimer >= rechargeDelay)
                currentEnergy = Mathf.MoveTowards(currentEnergy, maxEnergy, rechargeRate * Time.deltaTime);
        }

        isDraining = false; // reset each frame, abilities set it true
    }

    public bool DrainEnergy()
    {
        if (currentEnergy <= 0) return false;

        currentEnergy -= drainRate * Time.deltaTime;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        isDraining = true;
        rechargeTimer = 0f;
        return true;
    }
}