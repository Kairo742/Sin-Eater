using UnityEngine;

public class PlayerLightSystem : MonoBehaviour
{
    public float LightAmount = 0f;          //Best Values are : 1 - 20
    public float PassiveLightDecay = 0.5f;      //Minus this value from Light Amount every second


    [SerializeField] private Light _playerLight;


    void Update()
    {
        if (LightAmount > 0f)
        {
            LightAmount -= (PassiveLightDecay * Time.deltaTime);    //Light Decay
            _playerLight.intensity = LightAmount + 0.5f;
        }
    }
}
