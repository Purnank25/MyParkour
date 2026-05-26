using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform followTarget;
    [SerializeField] float distance = 5;
    [SerializeField] float minVerticalAngle = -45;
    [SerializeField] float maxVerticalAngle = 45;
    [SerializeField] Vector2 framingOffset;
    [SerializeField] float rotationSpeed = 2f;
    [SerializeField] bool invertX;
    [SerializeField] bool invertY;
    [SerializeField] LayerMask cameraCollisionMask;
    [SerializeField] float followSpeed = 5f;
    float rotationY;
    float rotationX;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
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