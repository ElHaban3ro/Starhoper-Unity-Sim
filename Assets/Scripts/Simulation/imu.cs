using Unity.VisualScripting;
using UnityEngine;

public class imu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Rigidbody rb;

    // Noise
    public float accelNoise = 0.2f;
    public float gyroNoise = 0.1f;
    public float magnetNoise = 0.05f;


    // IMU Data
    public Vector3 acceleration;
    public Vector3 gyro;
    public Vector3 magnet;

    public Vector3 lastVelocity;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lastVelocity = rb.linearVelocity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        // =================================
        // ACELERÓMETRO
        // =================================
        Vector3 accel = (rb.linearVelocity - lastVelocity) / dt - Physics.gravity;
        acceleration = accel + Random.insideUnitSphere * accelNoise;

        Vector3 imuAccel = new(
            acceleration.z,
            -acceleration.x,
            acceleration.y
        );
        acceleration = imuAccel;
        lastVelocity = rb.linearVelocity;
        

        // =================================
        // GIRÓSCOPIO
        // =================================
        Vector3 angularVelocity = rb.angularVelocity; // en rad/s

        Vector3 imuGyro = new Vector3(
            -angularVelocity.z,    // X ← Z (roll)
            -angularVelocity.x,   // Y ← -X (pitch)
            -angularVelocity.y    // Z ← -Y (yaw)
        );
        gyro = imuGyro;


        // =================================
        // MAGNETÓMETRO
        // =================================
        Vector3 magneticNorth = Vector3.forward; // Assuming a fixed magnetic north direction.
        Vector3 localNorth = transform.InverseTransformDirection(magneticNorth);
        magnet = localNorth + Random.insideUnitSphere * magnetNoise;

        Vector3 imuMagnet = new Vector3(
            magnet.z,    // X ← Z
            -magnet.x,   // Y ← -X
            -magnet.y    // Z ← -Y
        );
        magnet = imuMagnet;

        Debug.DrawRay(transform.position, transform.TransformDirection(magneticNorth) * 2, Color.cyan);
        
    }

}
