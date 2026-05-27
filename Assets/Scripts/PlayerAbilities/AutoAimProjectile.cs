using UnityEngine;

public class AutoAimProjectile : MonoBehaviour
{
    float speed;
    float damage;
    Vector3 direction;
    GameObject owner;
    public float Damage => damage;
    public void Init(Vector3 direction, float speed, float damage, GameObject owner)
    {
        this.direction = direction;
        this.speed = speed;
        this.damage = damage;
        this.owner = owner;
    }

    void Start()
    {
        Destroy(gameObject, 6f);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        // QueryTriggerInteraction.Collide makes raycast detect triggers
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit,
            speed * Time.deltaTime + 0.1f, Physics.AllLayers,
            QueryTriggerInteraction.Collide))
        {
            if (owner != null && hit.collider.transform.IsChildOf(owner.transform)) return;

            // check if hit shield first
            ShieldHealth shield = hit.collider.GetComponent<ShieldHealth>();
            if (shield != null)
            {
                shield.AbsorbDamage(damage);
                Destroy(gameObject);
                return;
            }

            // check if hit hitbox
            HealthSystem health = hit.collider.GetComponentInParent<HealthSystem>();
            if (health != null)
                health.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}