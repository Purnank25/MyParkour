using UnityEngine;

public class WaterSphereShooter : MonoBehaviour
{
    [SerializeField] GameObject waterSpherePrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float projectileSpeed = 25f;
    [SerializeField] float directDamage = 25f;
    [SerializeField] float splashDamage = 42f;      // 40-45 midpoint
    [SerializeField] float splashRadius = 3f;
    [SerializeField] float fireRate = 0.3f;
    [SerializeField] int maxAmmo = 4;
    [SerializeField] float rechargeDelay = 3f;

    int currentAmmo;
    float nextFireTime;
    float rechargeTimer;
    bool isRecharging;
    Camera mainCamera;

    public int CurrentAmmo => currentAmmo;
    public int MaxAmmo => maxAmmo;
    public bool IsRecharging => isRecharging;
    public float RechargeProgress => isRecharging ? rechargeTimer / rechargeDelay : 1f;

    void Start()
    {
        currentAmmo = maxAmmo;
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (isRecharging)
        {
            rechargeTimer += Time.deltaTime;
            if (rechargeTimer >= rechargeDelay)
            {
                currentAmmo = maxAmmo;
                isRecharging = false;
                rechargeTimer = 0f;
            }
            return;
        }

        if (Input.GetMouseButtonDown(1) && Time.time >= nextFireTime && currentAmmo > 0)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        Ray ray = mainCamera.ScreenPointToRay(
            new Vector3(Screen.width / 2, Screen.height / 2, 0));

        Vector3 targetPoint = Physics.Raycast(ray, out RaycastHit hit, 100f)
            ? hit.point
            : ray.GetPoint(100f);

        Vector3 direction = (targetPoint - firePoint.position).normalized;

        GameObject proj = Instantiate(waterSpherePrefab, firePoint.position,
            Quaternion.LookRotation(direction));

        WaterSphere sphere = proj.GetComponent<WaterSphere>();
        if (sphere != null)
            sphere.Init(directDamage, splashDamage, splashRadius,
                direction, projectileSpeed, gameObject);

        currentAmmo--;
        if (currentAmmo <= 0)
        {
            isRecharging = true;
            rechargeTimer = 0f;
        }
    }
}