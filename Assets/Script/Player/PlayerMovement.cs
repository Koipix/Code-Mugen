using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private PauseGame pauseGame;
    private Rigidbody2D rb;
    private SpriteRenderer playerSprite;
    private Vector3 mousePos;
    private Vector2 moveInput;
    private Vector2 lookInput; // Add variable for look input
    private Animator animator; //Animator Component
    private Camera cam;

    private bool DashOffCooldown = true;
    public float currMoveSpeed;

    //checks if player is on hub or not, required for the Transition Script
    public bool isOnHub = false;

    void Start() // https://discussions.unity.com/t/temporary-buff-scripts-and-timers/110542
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        playerSprite = GetComponent<SpriteRenderer>();

        currMoveSpeed = playerStats.MoveSpeed;
    }

    void Update()
    {
        rb.linearVelocity = moveInput * currMoveSpeed;

        if (!pauseGame.isPaused)
        {
            // Check if gamepad stick is being used for aiming
            if (lookInput.magnitude > 0.1f) // Use gamepad aim if stick is moved
            {
                AimGamepadStick();
            }
            else // Otherwise, use mouse point aiming
            {
                AimMousePoint();
            }
        }
    }

    //check Animator for more information..
    public void AimMousePoint()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePos - transform.position;

        //flip horizontantly based on the mouse position
        playerSprite.flipX = rotation.x < -0.5f;
    }

    // New method for gamepad stick aiming
    public void AimGamepadStick()
    {
        // For simplicity, we'll only use the lookInput for horizontal flipping
        // More complex aiming (e.g., rotating a weapon) would require calculating
        // an angle based on lookInput.
        playerSprite.flipX = lookInput.x < -0.5f;
    }

    //Play animation for the player whichever state it is, using Input System Package
    public void Move(InputAction.CallbackContext context)
    {
        animator.SetBool("isWalking", true);

        //when keys are released
        if (context.canceled)
        {
            animator.SetBool("isWalking", false);

        }
        moveInput = context.ReadValue<Vector2>();
    }

    // New public method to receive Look input
    public void Look(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void Dash(InputAction.CallbackContext context)
    {

        if (DashOffCooldown == true && context.performed)
        {
            // add a duration for the dash and also uhh cooldown ig, also the changing of rb.linearVelocity doesn't work so fix that
            DashOffCooldown = false;
            float DashDuration = 0.2f;
            int DashCooldown = 3; // longer cooldowns maybe, this is way too short and easily usable to get out of tight spots
            currMoveSpeed = 25f;
            Invoke("DashStop", DashDuration);
            Invoke("ResetDashCooldown", DashCooldown);

        }
    }

    public void DashStop()
    {
        currMoveSpeed = playerStats.MoveSpeed;
    }

    public void ResetDashCooldown()
    {
        DashOffCooldown = true;
    }
}