using UnityEngine;

public class CameraController : MonoBehaviour
{

   [SerializeField] Transform followTarget;
    [SerializeField] float distance = 5;
    [SerializeField] float minVerticalAngle = -45;
    [SerializeField] float maxVerticalAngle = 45;
    [SerializeField]   Vector2 framingoffest;
    [SerializeField] float rotationSpeed = 2f;
    [SerializeField] bool invertx;
    [SerializeField] bool inverty;
    float rotaionY ;
    float rotationX;
    float invertxval;
    float invertyval;
    void Start()
    {
      Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        invertxval = (invertx) ? -1 : 1;
        invertyval = (inverty) ? -1 : 1;
        rotaionY += Input.GetAxis("Mouse X") * rotationSpeed;
        rotationX = Mathf.Clamp(rotationX,minVerticalAngle, maxVerticalAngle);
        rotationX += Input.GetAxis("Mouse Y") * rotationSpeed;
        var targetRotation = Quaternion.Euler(rotationX, rotaionY, 0);

        var focusPosition = followTarget.position + new Vector3( framingoffest.x,framingoffest.y);
        transform.position = focusPosition - targetRotation * new Vector3(0,0,distance);
        transform.rotation = targetRotation;

    }
    public Quaternion PlanarRotation => Quaternion.Euler(0, rotaionY, 0);
}
