using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class engine : MonoBehaviour
{
    public Rigidbody droneRigidbody;
    public float thrust = 10f;
    public float maxThrust = 20f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        droneRigidbody = FindFirstObjectByType<imu>().GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Space key is pressed, applying thrust to the drone.");
            // Acción cuando se mantiene presionada la barra espaciadora
            droneRigidbody.AddForceAtPosition(thrust * Time.fixedDeltaTime * transform.up, transform.position, ForceMode.Force);
        }
    }
}
