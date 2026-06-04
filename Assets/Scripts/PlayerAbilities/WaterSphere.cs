using UnityEngine;

public class WaterSphere : MonoBehaviour
{
    float directDamage;
    float splashDamage;
    float splashRadius;
    Vector3 direction;
    float speed;
    GameObject owner;

    public float Damage => directDamage;

    public void Init(float directDamage, float splashDamage, float splashRadius,
        Vector3 direction, float speed, GameObject owner)
    {
        this.directDamage = directDamage;
        this.splashDamage = splashDamage;
        this.splashRadius = splashRadius;
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

            // check shield
            ShieldHealth shield = hit.collider.GetComponent<ShieldHealth>();
            if (shield != null)
            {
                shield.AbsorbDamage(directDamage);
                Destroy(gameObject);
                return;
            }

            Burst(hit.point);
        }
    }

    void Burst(Vector3 point)
    {
        // direct hit damage
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit,
            speed * Time.deltaTime + 0.1f, Physics.AllLayers,
            QueryTriggerInteraction.Collide))
        {
            HealthSystem directHealth = hit.collider.GetComponentInParent<HealthSystem>();
            if (directHealth != null)
                directHealth.TakeDamage(directDamage);
        }

        // splash damage in 3m radius
        Collider[] hits = Physics.OverlapSphere(point, splashRadius);
        foreach (Collider col in hits)
        {
            if (owner != null && col.transform.IsChildOf(owner.transform)) continue;
            if (col.isTrigger) continue;

            HealthSystem health = col.GetComponentInParent<HealthSystem>();
            if (health != null)
                health.TakeDamage(splashDamage);
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 0.5f, 1, 0.3f);
        Gizmos.DrawSphere(transform.position, splashRadius);
    }
}