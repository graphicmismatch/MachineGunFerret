using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _player;    // Reference to the player's Transform
    [SerializeField] private Vector3 _offset;     // Offset of the camera relative to the player
    [SerializeField] private float _smoothSpeed = 5f; // Smoothing speed for the camera's movement

    private void LateUpdate()
    {
        FollowPlayer();
    }

    /// <summary>
    /// Smoothly follows the player while maintaining the desired offset.
    /// </summary>
    private void FollowPlayer()
    {
        if (_player == null)
        {
            Debug.LogWarning("Player Transform is not assigned in CameraFollow!");
            return;
        }

        // Target position based on the player's position and the offset
        Vector3 targetPosition = _player.position + _offset;

        // Smoothly interpolate the camera's position to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, _smoothSpeed * Time.deltaTime);
    }
}
