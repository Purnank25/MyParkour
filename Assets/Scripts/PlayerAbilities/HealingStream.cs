using UnityEngine;

public class HealingStream : MonoBehaviour
{
    [SerializeField] float healPerSecond = 140f;    // 130-150 range midpoint
    [SerializeField] float range = 20f;
    [SerializeField] float fireRate = 0.1f;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject healingProjectilePrefab;
    [SerializeField] LayerMask allyLayer;
    [SerializeField] LineRenderer streamLine;

    float nextFireTime;
    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        streamLine.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            streamLine.enabled = true;
            streamLine.SetPosition(0, firePoint.position);

            Ray ray = mainCamera.ScreenPointToRay(
                new Vector3(Screen.width / 2, Screen.height / 2, 0));

            if (Physics.Raycast(ray, out RaycastHit hit, range, allyLayer))
            {
                // show stream line to ally
                streamLine.SetPosition(1, hit.point);

                // heal ally
                HealthSystem health = hit.collider.GetComponentInParent<HealthSystem>();
                if (health != null)
                    health.Heal(healPerSecond * Time.deltaTime);
            }
            else
            {
                // no ally hit, show stream to max range
                streamLine.SetPosition(1, ray.GetPoint(range));
            }
        }
        else
        {
            streamLine.enabled = false;
        }
    }
}