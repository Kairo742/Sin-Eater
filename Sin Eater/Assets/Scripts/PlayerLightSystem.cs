using Unity.VisualScripting;
using UnityEngine;

public class PlayerLightSystem : MonoBehaviour
{
    public float LightAmount = 0f;          //Best Values are : 1 - 20
    public float PassiveLightDecay = 0.5f;      //Minus this value from Light Amount every second
    [SerializeField] private float _startChangeSpeedTimer = 2f;
    private float _changeSpeedTimer, _changeSpeed;
    [SerializeField] private float _maxLight = 30, _minimumLight = 0.5f;

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
        if (LightAmount > _minimumLight)
        {
            LightAmount -= (PassiveLightDecay * Time.deltaTime);    //Light Decay
            _playerLight.intensity = LightAmount;
        }



        if(_changeSpeedTimer > 0)
        {
            LightAmount += _changeSpeed * Time.deltaTime;


            _changeSpeedTimer -= Time.deltaTime;
        }

        if (LightAmount > _maxLight)
        {
            LightAmount = _maxLight;
        }
    }

    public void AddLightAmount(float amount)
    {
        _changeSpeed = amount / _startChangeSpeedTimer;

        _changeSpeedTimer = _startChangeSpeedTimer;
    }
}
