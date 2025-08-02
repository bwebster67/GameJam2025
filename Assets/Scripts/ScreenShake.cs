using Unity.Cinemachine;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cineCam;
    private CinemachineBasicMultiChannelPerlin perlin;

    public float amplitude = 2f;
    public float frequency = 2f;
    public float duration = 0.3f;
    private float timer = 0f;

    private void Awake()
    {
        // Get the Perlin component directly from children
        perlin = GetComponent<CinemachineBasicMultiChannelPerlin>();

        if (perlin == null)
        {
            Debug.LogError("Perlin component not found! Make sure it's added to the camera.");
        }
    }

    public void Shake()
    {
        Debug.Log("Shaken");
        if (perlin == null) {
            Debug.Log("Perlin is null");
            return;
        }
        Debug.Log("Perlin not null");
        perlin.AmplitudeGain = amplitude;
        perlin.FrequencyGain = frequency;
        timer = duration;
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f && perlin != null)
            {
                perlin.AmplitudeGain = 0;
                perlin.FrequencyGain = 0;
            }
        }
    }
}
