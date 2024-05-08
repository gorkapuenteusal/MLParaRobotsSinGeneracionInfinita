using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // El objetivo que la cámara seguirá
    public float smoothSpeed = 0.125f; // Velocidad con la que la cámara seguirá al objetivo
    public Vector3 offset; // Offset de la posición del objetivo

    private RobotCar targetCar; // Referencia al script RobotCar del objetivo

    void Start()
    {
        if (target != null)
        {
            targetCar = target.GetComponent<RobotCar>();
            if (targetCar == null)
            {
                Debug.LogError("RobotCar script not found on the target object!");
            }
        }
        else
        {
            Debug.LogError("Target not set for CameraFollow script.");
        }
    }

    void LateUpdate()
    {
        if (targetCar != null && targetCar.followingWithCamera)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            transform.LookAt(target); // Opcional, si quieres que la cámara siempre mire hacia el coche
        }
    }
}
