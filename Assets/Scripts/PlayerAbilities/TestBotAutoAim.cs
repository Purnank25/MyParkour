using UnityEngine;

public class TestBotAutoAim : MonoBehaviour
{
    [SerializeField] float aimRadius = 10f;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] float damagePerSecond = 10f;
    [SerializeField] float fireRate = 1f;
    [SerializeField] float projectileSpeed = 15f;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject projectilePrefab;

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
        Collider[] hits = Physics.OverlapSphere(transform.position, aimRadius, targetLayer);
        float closest = Mathf.Infinity;
        currentTarget = null;

        foreach (Collider hit in hits)
        {
            if (hit.transform.root == transform.root) continue;

            float dist = Vector3.Distance(transform.position, hit.transform.position);
            if (dist < closest)
            {
                closest = dist;
                currentTarget = hit.transform;
            }
        }
    }

    void Shoot()
    {
        if (currentTarget == null) return;

        Vector3 targetPos = currentTarget.position + Vector3.up;
        Vector3 direction = (targetPos - firePoint.position).normalized;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(direction));

        AutoAimProjectile autoProj = proj.GetComponent<AutoAimProjectile>();
        if (autoProj != null)
            autoProj.Init(direction, projectileSpeed, damagePerSecond, gameObject);

        Destroy(proj, 6f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, aimRadius);
    }
}