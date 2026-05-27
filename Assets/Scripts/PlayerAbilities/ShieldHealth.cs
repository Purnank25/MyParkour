using UnityEngine;

public class ShieldHealth : MonoBehaviour
{
    VanguardShield vanguardShield;

    void Awake()
    {
        vanguardShield = GetComponentInParent<VanguardShield>();
    }

    public void AbsorbDamage(float damage)
    {
        if (!vanguardShield.IsActive) return;
        vanguardShield.TakeDamage(damage);
    }
}