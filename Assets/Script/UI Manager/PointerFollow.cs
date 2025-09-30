using UnityEngine;
using UnityEngine.InputSystem; // Needed to use InputAction.CallbackContext

public class PointerFollow : MonoBehaviour
{
    private Camera cam;
    private Vector3 mousePos;
    [SerializeField] private PauseGame pauseGame;

    // New variable to store the Look input from gamepad (Right Stick)
    private Vector2 lookInput;

    void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        if (!pauseGame.isPaused)
        {
            // Check if the gamepad stick is being used
            if (lookInput.magnitude > 0.1f)
            {
                // Aim using the gamepad stick vector
                AimGamepadStick(lookInput);
            }
            else
            {
                // Fallback to mouse point aiming if gamepad stick is idle
                AimMousePoint();
            }
        }
    }

    public void AimMousePoint()
    {
        // Calculate the vector from the object to the mouse
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePos - transform.position;

        // Rotate the object
        float rotationZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }

    public void AimGamepadStick(Vector2 input)
    {
        // The gamepad stick input is already the direction vector (rotation) we need
        Vector3 rotation = new Vector3(input.x, input.y, 0f);

        // Rotate the object based on the gamepad stick direction
        float rotationZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }

    // New public method to receive Look input from the Input System
    public void Look(InputAction.CallbackContext context)
    {
        // Read the 2D vector value (from Right Stick or Delta Pointer)
        lookInput = context.ReadValue<Vector2>();
    }
}