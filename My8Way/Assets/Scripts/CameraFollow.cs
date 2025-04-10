using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform player;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;

    private Vector3 velocity = Vector3.zero;
    void Start()
    {
        
    }

    void LateUpdate()
    {
       if (player != null)
        {
            Vector3 desiredPosition = player.position + offset;
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
            transform.position = smoothedPosition;

        }
         else
        {
            Debug.LogError("Player reference is null!");
        }
    }

    void Update()
    {
        
    }
}
