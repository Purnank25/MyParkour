using UnityEngine;

public class HeavyProjectile : MonoBehaviour
{
    float damage;
    GameObject owner;

    public void Init(float damage, GameObject owner)
    {
        this.damage = damage;
        this.owner = owner;

        // ignore all colliders on owner and its children
        Collider[] ownerColliders = owner.GetComponentsInChildren<Collider>();
        Collider projCollider = GetComponent<Collider>();
        foreach (Collider col in ownerColliders)
            Physics.IgnoreCollision(projCollider, col);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        if (owner != null && other.transform.IsChildOf(owner.transform)) return;

        HealthSystem health = other.GetComponentInParent<HealthSystem>();
        if (health != null)
            health.TakeDamage(damage);

        Destroy(gameObject);
    }
}