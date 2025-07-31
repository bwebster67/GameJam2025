using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{


    public Slider slider;
    [SerializeField] float smoothSpeed = 1.5f;
    public PlayerHealthController playerHealth;

    private void Update()
    {
        slider.value = Mathf.Lerp(slider.value, playerHealth.health, Time.deltaTime * smoothSpeed);
    }
    public void setMaxHealth(float maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
        playerHealth.maxHealth = maxHealth;
        playerHealth.health = maxHealth;
       // playerHealth.setHealthBarText(playerHealth.health, maxHealth);
    }
    public void setHealth(float health)
    {

        slider.value = Mathf.Lerp(slider.value, health, Time.deltaTime * smoothSpeed);
       // playerHealth.setHealthBarText(health, playerHealth.maxHealth);
    }

}
