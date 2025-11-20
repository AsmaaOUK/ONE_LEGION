using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform target; // The player
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 0, -10);
    
    [Header("Boundaries")]
    public bool useBoundaries = true;
    public float minX = 0f;
    public float maxX = 40f;
    
    void LateUpdate()
    {
        if (target == null) return;
        
        // Calculate desired position
        Vector3 desiredPosition = target.position + offset;
        
        // Apply boundaries
        if (useBoundaries)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
        }
        
        // Smooth follow
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
        // Keep Y position fixed (don't follow up/down)
        smoothedPosition.y = offset.y;
        
        transform.position = smoothedPosition;
    }
}