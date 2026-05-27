using UnityEngine;

public class ShieldHealth : MonoBehaviour
{
    VanguardShield vanguardShield;

    void Awake()
    {
        vanguardShield = GetComponentInParent<VanguardShield>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        // damage is handled by projectile hitting shield collider
    }

    // called by projectiles that hit the shield
    public void AbsorbDamage(float damage)
    {
        vanguardShield.TakeDamage(damage);
    }
}