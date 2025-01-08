using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; 
    public Vector3 offset = new Vector3(0, 2, -4); 
    public float smoothSpeed = 0.125f; 

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 desiredPosition = player.position + player.forward * offset.z + Vector3.up * offset.y;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            transform.position = smoothedPosition;

            transform.LookAt(player.position + Vector3.up * 1.5f); 
        }
    }
}
