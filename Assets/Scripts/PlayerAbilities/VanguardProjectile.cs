using UnityEngine;

public class VanguardProjectile : MonoBehaviour
{
    float damage;
    float aoeRadius;
    float speed;
    Vector3 direction;
    GameObject owner;

    [SerializeField] GameObject explosionEffect;

    public float Damage => damage;

    public void Init(float damage, float aoeRadius, Vector3 direction, float speed, GameObject owner)
    {
        this.damage = damage;
        this.aoeRadius = aoeRadius;
        this.direction = direction;
        this.speed = speed;
        this.owner = owner;
    }

    void Start()
    {
        Destroy(gameObject, 6f);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit,
            speed * Time.deltaTime + 0.1f, Physics.AllLayers,
            QueryTriggerInteraction.Collide))
        {
            if (owner != null && hit.collider.transform.IsChildOf(owner.transform)) return;

            // check shield first
            ShieldHealth shield = hit.collider.GetComponent<ShieldHealth>();
            if (shield != null)
            {
                shield.AbsorbDamage(damage);
                Destroy(gameObject);
                return;
            }

            // hit something — explode
            Explode();
        }
    }

    void Explode()
    {
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Collider[] hits = Physics.OverlapSphere(transform.position, aoeRadius);
        foreach (Collider hit in hits)
        {
            if (owner != null && hit.transform.IsChildOf(owner.transform)) continue;
            if (hit.isTrigger) continue;

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