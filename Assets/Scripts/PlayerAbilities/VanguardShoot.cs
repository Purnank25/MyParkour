using UnityEngine;

public class VanguardShoot : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float projectileSpeed = 20f;
    [SerializeField] float damage = 30f;
    [SerializeField] float aoeRadius = 3f;
    [SerializeField] float maxRange = 20f;
    [SerializeField] float fireRate = 0.5f;

    float nextFireTime;
    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        Ray ray = mainCamera.ScreenPointToRay(
            new Vector3(Screen.width / 2, Screen.height / 2, 0));

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out RaycastHit hit, maxRange))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(maxRange); // max 20 meters

        Vector3 direction = (targetPoint - firePoint.position).normalized;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position,
            Quaternion.LookRotation(direction));

        VanguardProjectile vProj = proj.GetComponent<VanguardProjectile>();
        if (vProj != null)
            vProj.Init(damage, aoeRadius, direction, projectileSpeed, gameObject);
    }
}