using UnityEngine;

public class VanguardProjectile : MonoBehaviour
{
    float damage;
    float aoeRadius;
    GameObject owner;
    [SerializeField] GameObject explosionEffect;

    public void Init(float damage, float aoeRadius, Vector3 direction, float speed, GameObject owner)
    {
        this.damage = damage;
        this.aoeRadius = aoeRadius;
        this.owner = owner;
        GetComponent<Rigidbody>().linearVelocity = direction * speed;
        Destroy(gameObject, 3f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (owner != null && other.transform.IsChildOf(owner.transform)) return;
        if (other.isTrigger) return;

        Explode();
    }

  
    void Explode()
    {
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Collider[] hits = Physics.OverlapSphere(transform.position, aoeRadius);
        foreach (Collider hit in hits)
        {
            if (owner != null && hit.transform.IsChildOf(owner.transform)) continue;

            // check if hit shield first
            ShieldHealth shield = hit.GetComponent<ShieldHealth>();
            if (shield != null)
            {
                shield.AbsorbDamage(damage);
                continue; // shield absorbed, dont damage player
            }

            HealthSystem health = hit.GetComponentInParent<HealthSystem>();
            if (health != null)
                health.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0.5f, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, aoeRadius);
    }
}