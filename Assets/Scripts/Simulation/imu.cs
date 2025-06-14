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

        // Acelerómetro simulado.
        Vector3 accel = (rb.linearVelocity - lastVelocity) / dt - Physics.gravity;
        acceleration = accel + Random.insideUnitSphere * accelNoise;

        Debug.Log("Aceleración: " + acceleration.y + " |||| Aceleración invertida: " + -acceleration.y);

        Vector3 imuAccel = new(
            acceleration.z,
            -acceleration.x,
            acceleration.y
        );
        acceleration = imuAccel;

        lastVelocity = rb.linearVelocity;


        // Giroscopio simulado: devuelve velocidad angular (rad/s) con ruido, como Vector3.
        gyro = rb.angularVelocity + Random.insideUnitSphere * gyroNoise;



        // Magnetómetro simulado.
        Vector3 magneticNorth = Vector3.forward; // Assuming a fixed magnetic north direction.
        Vector3 localNorth = transform.InverseTransformDirection(magneticNorth);
        magnet = localNorth + Random.insideUnitSphere * magnetNoise;


        // Crea un rayo hacia el norte del mundo (Vector3.forward)
        Vector3 campoMagneticoMundo = Vector3.forward;
        Debug.DrawRay(transform.position, transform.TransformDirection(campoMagneticoMundo) * 2, Color.cyan, 0.1f);

        // Convierte el vector del mundo al espacio local del sensor
        magnet = transform.InverseTransformDirection(campoMagneticoMundo);

        
    }

}
