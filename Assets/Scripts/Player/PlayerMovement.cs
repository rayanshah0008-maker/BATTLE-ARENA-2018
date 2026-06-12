using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun
{
    private Rigidbody rb;
    private CharacterController characterController;
    
    public float moveSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpForce = 5f;
    public float groundDrag = 5f;
    public float airDrag = 2f;
    public float groundDragMultiplier = 10f;
    
    private bool isGrounded;
    private bool isSprinting;
    private bool isCrouching;
    private bool isProne = false;
    
    private Vector3 moveDirection;
    private float horizontalInput;
    private float verticalInput;
    private float playerHeight = 2f;
    
    private LayerMask groundLayer;
    private float slopeMultiplier = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.drag = groundDrag;
            rb.freezeRotation = true;
        }
        groundLayer = LayerMask.GetMask("Ground");
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        // Ground check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f);
        
        HandleInput();
        ControlDrag();
        LimitSpeed();
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        MovePlayer();
    }

    void HandleInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        
        isSprinting = Input.GetKey(KeyCode.LeftShift);
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
        }
        
        if (Input.GetKeyDown(KeyCode.X))
        {
            isProne = !isProne;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void MovePlayer()
    {
        // Calculate move direction
        moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        
        float currentSpeed = moveSpeed;
        
        if (isSprinting && !isCrouching && !isProne)
        {
            currentSpeed = sprintSpeed;
        }
        else if (isCrouching)
        {
            currentSpeed *= 0.6f;
        }
        else if (isProne)
        {
            currentSpeed *= 0.4f;
        }

        // Slope handling
        currentSpeed *= slopeMultiplier;

        if (rb != null)
        {
            rb.AddForce(moveDirection.normalized * currentSpeed * groundDragMultiplier, ForceMode.Force);
        }
    }

    void Jump()
    {
        if (rb != null)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    void ControlDrag()
    {
        if (rb == null) return;
        
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    void LimitSpeed()
    {
        if (rb == null) return;
        
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        float currentSpeedLimit = moveSpeed;
        
        if (isSprinting && !isCrouching && !isProne)
        {
            currentSpeedLimit = sprintSpeed;
        }
        else if (isCrouching)
        {
            currentSpeedLimit = moveSpeed * 0.6f;
        }
        else if (isProne)
        {
            currentSpeedLimit = moveSpeed * 0.4f;
        }

        if (flatVel.magnitude > currentSpeedLimit)
        {
            Vector3 limitedVel = flatVel.normalized * currentSpeedLimit;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public bool IsSprinting()
    {
        return isSprinting;
    }

    public bool IsCrouching()
    {
        return isCrouching;
    }

    public bool IsProne()
    {
        return isProne;
    }
}
