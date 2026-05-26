using UnityEngine;

public class HeavyShot : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float projectileSpeed = 15f;
    [SerializeField] float damage = 40f;
    [SerializeField] float fireRate = 0.3f;
    [SerializeField] float rechargeDelay = 3f;
    [SerializeField] int maxAmmo = 4;

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
        // recharge
        if (isRecharging)
        {
            rechargeTimer += Time.deltaTime;
            if (rechargeTimer >= rechargeDelay)
            {
                currentAmmo = maxAmmo;
                isRecharging = false;
                rechargeTimer = 0f;
                Debug.Log("Heavy shot recharged");
            }
            return; // cant fire while recharging
        }

        if (Input.GetMouseButtonDown(1) && Time.time >= nextFireTime && currentAmmo > 0)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Vector3 targetPoint = Physics.Raycast(ray, out RaycastHit hit, 100f)
            ? hit.point
            : ray.GetPoint(100f);

        Vector3 direction = (targetPoint - firePoint.position).normalized;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(direction));

        // pass owner to ignore self collision
        HeavyProjectile heavyProj = proj.GetComponent<HeavyProjectile>();
        if (heavyProj != null)
            heavyProj.Init(damage, gameObject);

        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = direction * projectileSpeed;

        Destroy(proj, 6f);

        currentAmmo--;
        if (currentAmmo <= 0)
        {
            isRecharging = true;
            rechargeTimer = 0f;
        }
    }
}
