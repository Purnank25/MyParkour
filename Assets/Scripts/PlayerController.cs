using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float roationspeed = 500f;
    CameraController cameraController;
    Quaternion targetRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void Awake()
    {
       cameraController = Camera.main.GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
       float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float moveAmount = Mathf.Abs(h) + Mathf.Abs(v);
        var moveInput = new Vector3(h, 0, v).normalized;
        var movedir = cameraController.PlanarRotation * moveInput;
       if(moveAmount > 0)
        {
            transform.position += movedir * moveSpeed * Time.deltaTime;
            targetRotation = Quaternion.LookRotation(movedir);
        }
       transform.rotation =  Quaternion.RotateTowards(transform.rotation, targetRotation,roationspeed * Time.deltaTime);
    }
}
