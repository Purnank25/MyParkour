using UnityEngine;

public class HeavyProjectile : MonoBehaviour
{
    float damage;
    float speed;
    Vector3 direction;
    GameObject owner;

    public float Damage => damage;

    public void Init(float damage, float speed, Vector3 direction, GameObject owner)
    {
        this.damage = damage;
        this.speed = speed;
        this.direction = direction;
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

            // then check health
            HealthSystem health = hit.collider.GetComponentInParent<HealthSystem>();
            if (health != null)
                health.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}