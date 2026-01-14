using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float roationspeed = 500f;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset ;
    [SerializeField] LayerMask groundLayer;

    bool isgrounded;
    float yspeed ;
    bool hasControl =  true;
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
        if (isgrounded)
        {
            yspeed = -0.5f;
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
    void GroundCheck()
    {
        isgrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
      
    }
    public void SetControl( bool hasControl)
    {
        this.hasControl = hasControl;
        //characterController.enabled = hasControl;
        if(!hasControl)
        {
            animator.SetFloat("moveAmount", 0f);
            targetRotation = transform.rotation;
            yspeed = 1f;
        }
    }
    


    private void OnDrawGizmosSelected()
    {
       Gizmos.color = new Color(0,1,0,0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
    }

    public float RotationSpeed => roationspeed;
}
