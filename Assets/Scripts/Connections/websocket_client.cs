using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NativeWebSocket;
using System.Threading.Tasks;

public class MotorsForces
{
    public float m1;
    public float m2;
    public float m3;
    public float m4;

}
public class websocket_client : MonoBehaviour
{
    private imu imuComponent;

    private Rigidbody droneRigidbody;

    public engine motor1;
    public engine motor2;
    public engine motor3;
    public engine motor4;

    private float m1 = 0f;
    private float m2 = 0f;
    private float m3 = 0f;
    private float m4 = 0f;


    public WebSocket ws;
    public string serverUrl = "ws://localhost:3030"; // URL del servidor WebSocket


    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(motor1.transform.position, 0.15f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(motor2.transform.position, 0.15f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(motor3.transform.position, 0.15f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(motor4.transform.position, 0.15f);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        imuComponent = FindFirstObjectByType<imu>();
        droneRigidbody = imuComponent.GetComponent<Rigidbody>();
        await Connect();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        ws.DispatchMessageQueue();
#endif
        ws.SendText(JsonUtility.ToJson(imuComponent));

        droneRigidbody.AddForceAtPosition(m1 * Time.fixedDeltaTime * motor1.transform.up, motor1.transform.position, ForceMode.Force);
        droneRigidbody.AddForceAtPosition(m2 * Time.fixedDeltaTime * motor2.transform.up, motor2.transform.position, ForceMode.Force);
        droneRigidbody.AddForceAtPosition(m3 * Time.fixedDeltaTime * motor3.transform.up, motor3.transform.position, ForceMode.Force);
        droneRigidbody.AddForceAtPosition(m4 * Time.fixedDeltaTime * motor4.transform.up, motor4.transform.position, ForceMode.Force);

    }

    public async Task Connect()
    {
        ws = new WebSocket(serverUrl);

        ws.OnOpen += () =>
        {
            Debug.Log("Conexión abierta a " + serverUrl);
        };

        ws.OnError += (e) =>
        {
            Debug.Log("Error: " + e);
        };

        ws.OnClose += (e) =>
        {
            Debug.Log("Conexión cerrada: " + e);
        };

        ws.OnMessage += (bytes) =>
        {
            string message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("Instrucciones Recibidas: " + message);

            MotorsForces forces = JsonUtility.FromJson<MotorsForces>(message);

            // Aplicar fuerzas a los motores.
            m1 = forces.m1;
            m2 = forces.m2;
            m3 = forces.m3;
            m4 = forces.m4;
        };

        await ws.Connect();



    }

    public async void WsSendMessage(string message)
    {
        if (ws.State == WebSocketState.Open)
        {
            await ws.SendText(message);
        }
    }

    private async void OnApplicationQuit()
    {
        await ws.Close();
    }
}
