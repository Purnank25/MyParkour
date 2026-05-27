using UnityEngine;

public class PayloadController : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float detectionRadius = 5f;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundOffset = 0.5f;
    [SerializeField] float groundCheckDistance = 5f;
    [SerializeField] float groundSnapSpeed = 10f;

    int currentWaypointIndex = 0;
    public Vector3 CurrentVelocity { get; private set; }
    public bool IsMoving { get; private set; }

    void Update()
    {
        SnapToGround();

        if (IsPlayerNearby())
            MovePayload();
        else
            IsMoving = false;
    }

    void SnapToGround()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 2f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, groundCheckDistance, groundLayer))
        {
            float targetY = hit.point.y + groundOffset;
            Vector3 pos = transform.position;
            pos.y = Mathf.Lerp(pos.y, targetY, groundSnapSpeed * Time.deltaTime);
            transform.position = pos;

            Quaternion terrainRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            Quaternion targetRotation = terrainRotation * Quaternion.Euler(0, transform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, groundSnapSpeed * Time.deltaTime);
        }
    }

    bool IsPlayerNearby()
    {
        Collider[] players = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
        return players.Length > 0;
    }

    void MovePayload()
    {
        if (currentWaypointIndex >= waypoints.Length)
        {
            IsMoving = false;
            return;
        }

        IsMoving = true;

        Transform target = waypoints[currentWaypointIndex];
        Vector3 prevPosition = transform.position;

        Vector3 targetPosition = new Vector3(
            target.position.x,
            transform.position.y,
            target.position.z
        );

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        CurrentVelocity = (transform.position - prevPosition) / Time.deltaTime;

        Vector3 dir = new Vector3(
            target.position.x - transform.position.x,
            0,
            target.position.z - transform.position.z
        );

        if (dir.magnitude > 0.01f)
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.LookRotation(dir),
                200f * Time.deltaTime
            );

        if (Vector3.Distance(
            new Vector3(transform.position.x, 0, transform.position.z),
            new Vector3(target.position.x, 0, target.position.z)) < 0.1f)
            currentWaypointIndex++;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, detectionRadius);

        if (waypoints == null) return;
        Gizmos.color = Color.yellow;
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            if (waypoints[i] != null && waypoints[i + 1] != null)
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }
    }
}