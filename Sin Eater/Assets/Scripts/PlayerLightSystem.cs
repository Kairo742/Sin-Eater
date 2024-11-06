using Unity.VisualScripting;
using UnityEngine;

public class PlayerLightSystem : MonoBehaviour
{
    public float LightAmount = 0f;          //Best Values are : 1 - 20
    public float PassiveLightDecay = 0.5f;      //Minus this value from Light Amount every second
    [SerializeField] private float _startChangeSpeedTimer = 2f;
    private float _changeSpeedTimer, _changeSpeed;

    [SerializeField] private Light _playerLight;

    public static PlayerLightSystem Instance;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    void Update()
    {
        if (LightAmount > 0f)
        {
            LightAmount -= (PassiveLightDecay * Time.deltaTime);    //Light Decay
            _playerLight.intensity = LightAmount + 0.5f;
        }



        if(_changeSpeedTimer > 0)
        {
            LightAmount += _changeSpeed * Time.deltaTime;


            _changeSpeedTimer -= Time.deltaTime;
        }
    }

    public void AddLightAmount(float amount)
    {
        _changeSpeed = amount / _startChangeSpeedTimer;

        _changeSpeedTimer = _startChangeSpeedTimer;
    }
}
