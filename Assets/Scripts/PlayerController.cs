using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;           // Reference to the Rigidbody component
    [SerializeField] private float _speed = 5f;       // Movement speed
    [SerializeField] private float _turnSpeed = 360f; // Turning speed (degrees per second)
    private Vector3 _input;                          // Stores player input for movement
    private Quaternion _targetRotation;             // The target rotation for the player
    private float _rotationThreshold = 0.1f;        // Threshold for direction changes to trigger rotation

    private void Update()
    {
        GatherInput(); // Collect player input
        CalculateTargetRotation(); // Calculate the target rotation based on input
    }

    private void FixedUpdate()
    {
        Move(); // Apply movement
        SmoothRotate(); // Smoothly rotate the player toward the target direction
    }

    /// <summary>
    /// Collects input from the player.
    /// </summary>
    private void GatherInput()
    {
        // Get input from WASD or arrow keys
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        // Normalize the input to avoid jittering when moving diagonally
        if (_input.magnitude > 1f)
        {
            _input.Normalize();
        }
    }

    /// <summary>
    /// Calculates the target rotation for the player based on input direction.
    /// </summary>
    private void CalculateTargetRotation()
    {
        if (_input != Vector3.zero) // Only calculate if there is movement input
        {
            // Rotate the input vector by 45 degrees to align with the isometric perspective
            var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
            var skewedInput = matrix.MultiplyPoint3x4(_input);

            // Calculate the direction the player should face
            var relative = (transform.position + skewedInput) - transform.position;

            // Only update the target rotation if the direction has changed significantly
            if (Vector3.Angle(transform.forward, relative) > _rotationThreshold)
            {
                _targetRotation = Quaternion.LookRotation(relative, Vector3.up);
            }
        }
    }

    /// <summary>
    /// Smoothly rotates the player to face the target rotation.
    /// </summary>
    private void SmoothRotate()
    {
        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, _turnSpeed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Moves the player in the isometric direction.
    /// </summary>
    private void Move()
    {
        // Rotate the input vector by 45 degrees to align with the isometric perspective
        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        var skewedInput = matrix.MultiplyPoint3x4(_input);

        // Move the player based on the rotated input
        _rb.MovePosition(transform.position + skewedInput * _speed * Time.fixedDeltaTime);
    }
}
