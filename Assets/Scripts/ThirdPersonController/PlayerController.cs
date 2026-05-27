using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float roationspeed = 500f;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset ;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float jumpForce = 5f;
    bool isgrounded;
    float yspeed ;
    bool hasControl =  true;
    bool isOnPayload;
    PayloadController payload;
    CameraController cameraController;
    Quaternion targetRotation;
    Animator animator;
    CharacterController characterController;
    
    void Start()
    {
        
    }
    void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));
        var moveInput = new Vector3(h, 0, v).normalized;
        var movedir = cameraController.PlanarRotation * moveInput;

        GroundCheck();

        if (isgrounded)
        {
            yspeed = -0.5f;
            if (hasControl && Input.GetButtonDown("Jump"))
                yspeed = jumpForce;
        }
        else
        {
            yspeed += Physics.gravity.y * Time.deltaTime;
        }

        var velocity = Vector3.zero;
        velocity.y = yspeed;
        characterController.Move(velocity * Time.deltaTime);

        if (!hasControl) return;

        if (isOnPayload && payload != null && isgrounded)
            characterController.Move(payload.CurrentVelocity * Time.deltaTime);

        velocity = movedir * moveSpeed;
        velocity.y = yspeed;
        characterController.Move(velocity * Time.deltaTime);

        // player always faces camera forward direction
        targetRotation = cameraController.PlanarRotation;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, roationspeed * Time.deltaTime);

        animator.SetFloat("moveZ", moveAmount, 0.2f, Time.deltaTime);
        
    }

    public void SetControl( bool hasControl)
    {
        this.hasControl = hasControl;
        //characterController.enabled = hasControl;
        if(!hasControl)
        {
            
            targetRotation = transform.rotation;
            yspeed = 0f;
        }
    }
    


    private void OnDrawGizmosSelected()
    {
       Gizmos.color = new Color(0,1,0,0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
    }

    public float RotationSpeed => roationspeed;

    
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Payload") && isgrounded)
        {
            isOnPayload = true;
            payload = hit.gameObject.GetComponentInParent<PayloadController>();
        }
    }
    void GroundCheck()
    {
        bool wasGrounded = isgrounded;
        isgrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);

        // reset payload when in air
        if (!isgrounded)
        {
            isOnPayload = false;
            payload = null;
        }
    }
    public void SetCamera(CameraController cam)
    {
        cameraController = cam;
    }
}
