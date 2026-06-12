using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// Main Player Controller - Handles player input, movement, and interactions
/// Free Fire Style Controls
/// </summary>
public class PlayerController : MonoBehaviourPunCallbacks
{
    [Header("Player Settings")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 8f;
    public float jumpForce = 5f;
    public float groundDrag = 5f;
    public float airDrag = 2f;

    [Header("Ground Check")]
    public float playerHeight = 2f;
    public LayerMask groundLayer;
    private bool isGrounded;

    [Header("References")]
    private Rigidbody rb;
    private Camera playerCamera;
    private WeaponSystem weaponSystem;
    private PlayerStats playerStats;

    private float horizontalInput = 0f;
    private float verticalInput = 0f;
    private bool isSprinting = false;
    private bool gameEnded = false;
    private Vector3 moveDirection;
    private float currentSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();
        weaponSystem = GetComponent<WeaponSystem>();
        playerStats = GetComponent<PlayerStats>();

        if (!photonView.IsMine)
        {
            playerCamera.enabled = false;
            GetComponent<AudioListener>().enabled = false;
        }
    }

    private void Update()
    {
        if (!photonView.IsMine || gameEnded) return;

        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);

        GetInput();
        HandleMovement();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.GetMouseButton(0))
        {
            if (weaponSystem != null)
                weaponSystem.Fire();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (weaponSystem != null)
                weaponSystem.Reload();
        }

        isSprinting = Input.GetKey(KeyCode.LeftShift);
        SpeedControl();
        rb.drag = isGrounded ? groundDrag : airDrag;
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void HandleMovement()
    {
        moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        moveDirection.Normalize();

        currentSpeed = isSprinting ? sprintSpeed : moveSpeed;
        rb.AddForce(moveDirection * currentSpeed * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > currentSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * currentSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    public void TakeDamage(float damage, string damageSource = "Unknown")
    {
        if (playerStats != null)
        {
            playerStats.TakeDamage(damage);

            if (playerStats.GetHealth() <= 0)
            {
                Die(damageSource);
            }
        }
    }

    public void Die(string killedBy = "Unknown")
    {
        Debug.Log($"[PlayerController] {photonView.Owner.NickName} was killed by {killedBy}");

        if (photonView.IsMine)
        {
            photonView.RPC("RPC_Die", RpcTarget.All, killedBy);
            GameManager.Instance.PlayerEliminated(photonView.Owner);
        }
    }

    [PunRPC]
    void RPC_Die(string killedBy)
    {
        gameEnded = true;
        rb.isKinematic = true;
    }

    public void SetGameEnded(bool ended)
    {
        gameEnded = ended;
    }

    public Player GetPlayer()
    {
        return photonView.Owner;
    }

    public WeaponSystem GetWeaponSystem()
    {
        return weaponSystem;
    }

    public PlayerStats GetPlayerStats()
    {
        return playerStats;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (otherPlayer == photonView.Owner)
        {
            Destroy(gameObject);
        }
    }
}