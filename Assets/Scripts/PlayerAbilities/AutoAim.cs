using UnityEngine;

public class AutoAim : MonoBehaviour
{
    [SerializeField] float aimRadius = 10f;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float damage = 10f;
    [SerializeField] float fireRate = 1f; // shots per second
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 20f;
    [SerializeField] Transform firePoint; // empty GameObject at gun/hand position
    float nextFireTime;
    Transform currentTarget;

    void Update()
    {
        FindTarget();

        if (currentTarget != null && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + (1f / fireRate);
        }
    }

    void FindTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, aimRadius, playerLayer);

        float closestDistance = Mathf.Infinity;
        currentTarget = null;

        foreach (Collider hit in hits)
        {
            // skip yourself
            if (hit.transform.root == transform.root) continue;

            float dist = Vector3.Distance(transform.position, hit.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                currentTarget = hit.transform;
            }
        }
    }

    void Shoot()
    {
        if (currentTarget == null) return;

        HealthSystem health = currentTarget.GetComponent<HealthSystem>();
        if (health == null || health.IsDead) return;

        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position + Vector3.up;
        GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        Projectile projectile = proj.GetComponent<Projectile>();
        projectile.Init(currentTarget, damage, projectileSpeed);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, aimRadius);
    }
}