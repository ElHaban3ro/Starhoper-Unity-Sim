using TMPro;
using UnityEngine;

public class IMUText : MonoBehaviour
{
    public TextMeshProUGUI textObject;
    public imu imuComponent;

    void Start()
    {
        textObject = GetComponent<TextMeshProUGUI>();
        imuComponent = FindFirstObjectByType<imu>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        textObject.text = $"Acceleration: {imuComponent.acceleration}\n" +
                          $"Gyro: {imuComponent.gyro}\n" +
                          $"Magnet: {imuComponent.magnet}";        
    }
}
