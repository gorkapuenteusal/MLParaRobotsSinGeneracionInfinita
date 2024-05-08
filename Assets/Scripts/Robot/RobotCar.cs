using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class RobotCar : MonoBehaviour
{
    private const float groundCheckDistance = 1f; // Distancia para chequear el suelo

    public float gas;
    public float maxAcceleration;
    public float maxAngularAcceleration;
    public AnimationCurve accelerationCurve;
    public AnimationCurve angularAccelerationCurve;
    public LayerMask groundLayer; // Capa del suelo
    public Transform raycasterTransform;
    public Material wheelMaterial;
    public bool followingWithCamera;

    private float currentAcceleration = 0f;
    private float currentAngularAcceleration = 0f;
    private Rigidbody rb;
    private float vTimePressed = 0f; // Tiempo que la tecla ha estado presionada
    private float hTimePressed = 0f;
    private float timeSinceStoped = 0f;
    private bool overtuned = false;
    private bool grounded = false;
    private float vInput = 0f;
    private float hInput = 0f;

    private MapGenerator mapGenerator;
    private bool outOfBounds = true;
    private bool freezed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mapGenerator = FindFirstObjectByType<MapGenerator>();
        if (mapGenerator == null)
        {
            Debug.LogError("MapGenerator not found in the scene!");
            this.enabled = false;
        }
    }

    void Update()
    {
        CheckIfGrounded();
        CheckIfOvertuned();
        CheckIfOutOfMap();

        vInput = Input.GetAxis("Vertical");
        hInput = Input.GetAxis("Horizontal");
        UpdateTimePressed();

        if (outOfBounds)
        {
            wheelMaterial.color = Color.magenta;
            if (!freezed) SetFreezed(true);
        }
        else
        {
            if (freezed) SetFreezed(false);
            if (overtuned)
                wheelMaterial.color = Color.gray;
            else if (grounded && gas > 0)
            {
                wheelMaterial.color = Color.green;
            }
            else
            {
                wheelMaterial.color = Color.red;
                currentAcceleration = 0; // Reset acceleration if not grounded
            }
        }
    }

    void FixedUpdate() {
        if (vInput != 0)
            ConsumeGas();
            
        if (grounded && gas > 0 && !overtuned) 
        {
            Accelerate();
            ApplyTorque();
        }
        else 
        {
            currentAcceleration = 0;
            currentAngularAcceleration = 0;
        }
            
    }

    private void Accelerate()
    {
        currentAcceleration = Mathf.Lerp(currentAcceleration, (vInput == 0) ? 0 : maxAcceleration, accelerationCurve.Evaluate(vTimePressed)) * vInput;
        rb.AddForce(transform.up * currentAcceleration, ForceMode.Acceleration);
    }

    private void ApplyTorque() {
        currentAngularAcceleration = Mathf.Lerp(currentAngularAcceleration, (hInput == 0) ? 0 : maxAngularAcceleration, angularAccelerationCurve.Evaluate(hTimePressed)) * hInput;
        rb.AddTorque(-transform.forward * currentAngularAcceleration, ForceMode.Acceleration);
    }

    private void ConsumeGas()
    {
        if (gas != 0)
        {
            gas -= 10 * Time.deltaTime;
            gas = Mathf.Max(0, gas); // Ensure gas doesn't go below zero
        }
    }

    private void SetFreezed(bool freezed) 
    {
        if (freezed) 
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        rb.isKinematic = freezed;
        this.freezed = freezed;
    }

    private void CheckIfGrounded()
    {
        Vector3 rayStart = raycasterTransform.position; // Punto de inicio del rayo en el centro del objeto
        Vector3 rayDirection = raycasterTransform.forward; // Dirección del rayo hacia abajo

        // Dibuja el rayo en la ventana de Scene para depuración
        Debug.DrawRay(rayStart, rayDirection * groundCheckDistance, Color.red);

        // Dispara un rayo hacia abajo desde el centro del coche
        grounded =  Physics.Raycast(rayStart, rayDirection, groundCheckDistance, groundLayer);
    }

    private void CheckIfOvertuned() 
    {
        if (Vector3.Angle(transform.forward, Vector3.up) <= 110) 
        {
            if (rb.velocity.magnitude < 0.01) 
            {
                timeSinceStoped += Time.deltaTime;
            }
            else
            {
                timeSinceStoped = 0;
            }

            if (timeSinceStoped >= 5f)
            {
                overtuned = true;
            }
        }
        else
        {
            timeSinceStoped = 0;
            overtuned = false;
        }
    }

    private void CheckIfOutOfMap() {
        if (Mathf.Abs(transform.position.x) > mapGenerator.mapSize / 2 || Mathf.Abs(transform.position.z) > mapGenerator.mapSize / 2)
        {
            outOfBounds = true;
        }
        else
        {
            outOfBounds = false;
        }
    }

    private void UpdateTimePressed() 
    {
        if (vInput != 0) vTimePressed += Time.deltaTime;
        else vTimePressed = Mathf.Max(0, vTimePressed - Time.deltaTime);

        if (hInput != 0) hTimePressed += Time.deltaTime;
        else hTimePressed = Mathf.Max(0, hTimePressed - Time.deltaTime);
    }
}
