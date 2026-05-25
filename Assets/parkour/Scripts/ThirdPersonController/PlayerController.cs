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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void Awake()
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
        float moveAmount = Mathf.Clamp01( Mathf.Abs(h) +  Mathf.Abs(v));
        var moveInput = new Vector3(h, 0, v).normalized;
        var movedir = cameraController.PlanarRotation * moveInput;
        GroundCheck();
        Debug.Log( "is grounded = "+ isgrounded);
        if (!hasControl)
        {
            return;
        }
        if(hasControl)
        {
            FaceMouseDirection();
            if (isOnPayload && payload != null && isgrounded)
                characterController.Move(payload.CurrentVelocity * Time.deltaTime);
        }
        if (isgrounded)
        {
            yspeed = -0.5f;

            if (Input.GetButtonDown("Jump"))
                yspeed = jumpForce;
        }
        
        else
        {
          yspeed += Physics.gravity.y * Time.deltaTime;
        }
        var velocity = movedir * moveSpeed;
        velocity.y = yspeed;
        characterController.Move(velocity * Time.deltaTime);
       
        if (moveAmount > 0)
        {
            
            targetRotation = Quaternion.LookRotation(movedir);
        }

       transform.rotation =  Quaternion.RotateTowards(transform.rotation, targetRotation,roationspeed * Time.deltaTime);
        animator.SetFloat("moveAmount", moveAmount,0.2f,Time.deltaTime);
    }
    
    public void SetControl( bool hasControl)
    {
        this.hasControl = hasControl;
        //characterController.enabled = hasControl;
        if(!hasControl)
        {
            animator.SetFloat("moveAmount", 0f);
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

    void FaceMouseDirection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            Vector3 lookTarget = hit.point;
            lookTarget.y = transform.position.y; // keep player upright
            Vector3 direction = lookTarget - transform.position;

            if (direction.magnitude > 0.1f)
            {
                targetRotation = Quaternion.LookRotation(direction);
            }
        }
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Payload") && isgrounded)
        {
            isOnPayload = true;
            payload = hit.gameObject.GetComponent<PayloadController>();
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
}
