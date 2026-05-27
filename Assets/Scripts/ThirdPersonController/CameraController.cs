using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float distance = 5;
    [SerializeField] float minVerticalAngle = -45;
    [SerializeField] float maxVerticalAngle = 45;
    [SerializeField] Vector2 framingOffset;
    [SerializeField] float rotationSpeed = 2f;
    [SerializeField] bool invertX;
    [SerializeField] bool invertY;
    [SerializeField] LayerMask cameraCollisionMask;

    Transform followTarget;
    float rotationY;
    float rotationX;

    void Awake()
    {
        // auto find player by tag
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            // look for CameraTarget child
            Transform cameraTarget = player.transform.Find("CameraTarget");
            followTarget = cameraTarget != null ? cameraTarget : player.transform;
        }
        else
        {
            Debug.LogError("No GameObject with tag Player found");
        }
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (followTarget == null) return;

        float invertXVal = invertX ? -1 : 1;
        float invertYVal = invertY ? -1 : 1;

        rotationY += Input.GetAxis("Mouse X") * rotationSpeed * invertXVal;
        rotationX += Input.GetAxis("Mouse Y") * rotationSpeed * invertYVal;
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);

        var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);
        var focusPosition = followTarget.position + new Vector3(framingOffset.x, framingOffset.y);

        Vector3 desiredPosition = focusPosition - targetRotation * new Vector3(0, 0, distance);

        if (Physics.Raycast(focusPosition, desiredPosition - focusPosition, out RaycastHit hit, distance, cameraCollisionMask))
            transform.position = hit.point + (focusPosition - desiredPosition).normalized * 0.2f;
        else
            transform.position = desiredPosition;

        transform.rotation = targetRotation;
    }

    public Quaternion PlanarRotation => Quaternion.Euler(0, rotationY, 0);
}