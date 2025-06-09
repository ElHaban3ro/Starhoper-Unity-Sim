using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NativeWebSocket;
using System.Threading.Tasks;
public class websocket_client : MonoBehaviour
{
    private imu imuComponent;
    public WebSocket ws;
    public string serverUrl = "ws://localhost:3030"; // URL del servidor WebSocket

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        imuComponent = FindFirstObjectByType<imu>();
        await Connect();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        ws.DispatchMessageQueue();
#endif
        ws.SendText(JsonUtility.ToJson(imuComponent));
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
