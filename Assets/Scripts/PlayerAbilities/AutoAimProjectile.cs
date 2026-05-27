using UnityEngine;

public class AutoAimProjectile : MonoBehaviour
{
    float speed;
    float damage;
    Vector3 direction;
    GameObject owner;

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

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, speed * Time.deltaTime + 0.1f))
        {
            // ignore owner
            if (owner != null && hit.collider.transform.IsChildOf(owner.transform)) return;

            // deal damage if target has health
            HealthSystem health = hit.collider.GetComponentInParent<HealthSystem>();
            if (health != null)
                health.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}