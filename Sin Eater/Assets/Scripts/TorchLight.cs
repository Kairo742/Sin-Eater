using UnityEngine;
using static UnityEngine.ParticleSystem;

public class TorchLight : Interactable
{
    private bool _hasLightLeft = true;
    private float  _resetTimer;

    private float LightIntensity = 1f;
    [SerializeField] private float _lightAmountForPlayer = 4, _randomSpeed, _startingIntensity = 1f, _flicksPerSecond = 3f, _flickerIntensity = 0.2f, _startResetTimer = 5f;
    private Light _light;
    private ParticleSystem _particles;
    private float _time;


    void Start()
    {
        _light = transform.parent.GetComponentInChildren<Light>();
        _particles = transform .parent.GetComponentInChildren<ParticleSystem>();
        _resetTimer = _startResetTimer;
    }

    void Update()
    {
        if (_hasLightLeft)      //Normal Light Working
        {



            _time += Time.deltaTime * (1 - Random.Range(-_randomSpeed, _randomSpeed)) * Mathf.PI;
            LightIntensity = _startingIntensity + Mathf.Sin(_time * _flicksPerSecond) * _flickerIntensity;

            _light.intensity = LightIntensity;


        }
        else     //Light is Off
        {
            if (_resetTimer > 0)
            {
                _resetTimer -= Time.deltaTime;
                _light.intensity = _startingIntensity *  ( 1f - (_resetTimer / _startResetTimer));        //Light slowly turns back on
            }
            else     //Turn Light Back On
            {
                ResetLight(); 

                _resetTimer = _startResetTimer;
            }
        }
    }


    public void NoLightLeft()
    {
        if (!_hasLightLeft) return;

        _particles.Stop();
        var x = _particles.main;
        x.loop = false;

        gameObject.layer = LayerMask.NameToLayer("Default");

        _hasLightLeft = false;
    }

    private void ResetLight()
    {
        var x = _particles.main;
        x.loop = true;
        _particles.Play();

        gameObject.layer = LayerMask.NameToLayer("Light Source");

        _hasLightLeft = true;
    }

    public override void Interact()
    {
        if (!_hasLightLeft) return;

        PlayerLightSystem.Instance.AddLightAmount(_lightAmountForPlayer);
        NoLightLeft();
    }
}
