using UnityEngine;

public class PayloadController : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float detectionRadius = 5f;
    [SerializeField] LayerMask playerLayer;
    public Vector3 CurrentVelocity { get; private set; }
    int currentWaypointIndex = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(IsPlayerNearby())
        {
            MovePayload();
        }
    }
    bool IsPlayerNearby()
    {
        Collider[] players = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
        return players.Length > 0;
    }
    void MovePayload()
    {
        if (currentWaypointIndex >= waypoints.Length) return;

        Transform target = waypoints[currentWaypointIndex];
        Vector3 prevPosition = transform.position;

        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        CurrentVelocity = (transform.position - prevPosition) / Time.deltaTime;

        
      
        Vector3 dir = (target.position - transform.position);
        dir.y = 0;
        if (dir.magnitude > 0.01f)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), 200f * Time.deltaTime);

        
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
            currentWaypointIndex++;
    }
    private void OnDrawGizmosSelected()
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
