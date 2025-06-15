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
    public Vector3 eulerAngles = Vector3.zero; // Euler angles for the IMU orientation


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
        
        eulerAngles = new Vector3(
            NormalizeAngle(transform.eulerAngles.z),
            NormalizeAngle(transform.eulerAngles.x),
            NormalizeAngle(transform.eulerAngles.y)
        );

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

        Debug.DrawRay(transform.position, transform.forward * 2, Color.cyan);

    }

    float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle > 180f)
            angle -= 360f;
        return angle;
    }

}