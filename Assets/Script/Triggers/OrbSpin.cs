using UnityEngine;

public class OrbSpin : MonoBehaviour
{
    public float rotationSpeed = 200f;

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
