using UnityEngine;

public class Projectile : MonoBehaviour
{
    float damage;
    float speed = 20f;
    Transform target;
    bool isHit;

    public void Init(Transform target, float damage, float speed)
    {
        this.target = target;
        this.damage = damage;
        this.speed = speed;
    }
    void Start()
    {
        // ignore collision with all characters so it doesnt push
        CharacterController[] controllers = FindObjectsByType<CharacterController>(FindObjectsSortMode.None);
        Collider projCol = GetComponent<Collider>();
        foreach (CharacterController cc in controllers)
        {
            Collider col = cc.GetComponent<Collider>();
            if (col != null && projCol != null)
                Physics.IgnoreCollision(projCol, col);
        }

        Destroy(gameObject, 5f);
    }
    void Update()
    {
        if (target == null || isHit)
        {
            Destroy(gameObject);
            return;
        }

        // move toward target
        Vector3 direction = (target.position + Vector3.up * 1f) - transform.position;
        transform.position += direction.normalized * speed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(direction);

        // check if close enough to hit
        if (direction.magnitude < 0.3f)
            Hit();
    }

    void Hit()
    {
        if (isHit) return;
        isHit = true;

        HealthSystem health = target.GetComponent<HealthSystem>();
        if (health != null && !health.IsDead)
            health.TakeDamage(damage);

        Destroy(gameObject);
    }
}