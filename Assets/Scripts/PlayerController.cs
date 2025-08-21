using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    CameraController cameraController;
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

        var moveInput = new Vector3(h, 0, v).normalized;
        var movedir = cameraController.PlanarRotation * moveInput;
        transform.position +=  movedir * moveSpeed * Time.deltaTime;
    }
}
