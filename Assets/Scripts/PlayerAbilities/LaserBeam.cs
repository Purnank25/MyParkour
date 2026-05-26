using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [SerializeField] float aimRadius = 10f;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] float damagePerSecond = 15f;
    [SerializeField] Transform firePoint;
    [SerializeField] LineRenderer laserLine;

    EnergySystem energySystem;

    void Awake()
    {
        energySystem = GetComponent<EnergySystem>();
        laserLine.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && energySystem.HasEnergy)
        {
            Transform target = FindNearestEnemy();
            if (target != null)
            {
                bool drained = energySystem.DrainEnergy();
                if (drained)
                {
                    FireLaser(target);
                    return;
                }
            }
        }

        // no target or no energy
        laserLine.enabled = false;
    }

    Transform FindNearestEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, aimRadius, enemyLayer);
        float closest = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach (Collider hit in hits)
        {
            if (hit.transform.root == transform.root) continue;

            HealthSystem health = hit.GetComponent<HealthSystem>();
            if (health == null || health.IsDead) continue;

            float dist = Vector3.Distance(transform.position, hit.transform.position);
            if (dist < closest)
            {
                closest = dist;
                nearestEnemy = hit.transform;
            }
        }

        return nearestEnemy;
    }

    void FireLaser(Transform target)
    {
        // damage
        HealthSystem health = target.GetComponent<HealthSystem>();
        if (health != null && !health.IsDead)
            health.TakeDamage(damagePerSecond * Time.deltaTime);

        // visual line
        laserLine.enabled = true;
        laserLine.SetPosition(0, firePoint.position);
        laserLine.SetPosition(1, target.position + Vector3.up);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 1, 0.2f);
        Gizmos.DrawSphere(transform.position, aimRadius);
    }
}