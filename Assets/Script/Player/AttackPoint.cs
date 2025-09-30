using UnityEngine;

public class Attack : MonoBehaviour
{
    private Camera cam;

    // Awake is kept to initialize the camera
    private void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // New generic method to aim using a target position (used for mouse)
    public void AimAtPosition(Vector3 targetPosition)
    {
        // Calculate direction from the attack point's current position to the target
        Vector3 rotation = targetPosition - transform.position;

        // Use this direction to rotate
        AimInDirection(rotation);
    }

    // New generic method to aim using a direction vector (used for gamepad)
    public void AimInDirection(Vector3 direction)
    {
        float rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }

    // Keep AimMousePoint for LaunchAttack.cs compatibility, but make it call the new generic method
    public void AimMousePoint()
    {
        // This is still needed for mouse aiming compatibility if you haven't changed the Input.mousePosition logic
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        AimAtPosition(mousePos);
    }
}