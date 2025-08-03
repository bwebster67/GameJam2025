using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{


    public Slider slider;
    [SerializeField] float smoothSpeed = 1.5f;
    public PlayerHealthController playerHealth;
    public GameObject player;

    void Start()
    {
        slider.maxValue = playerHealth.maxHealth;
        slider.value = playerHealth.health;
    }

    private void Update()
    {
        // slider.value = Mathf.Lerp(slider.value, playerHealth.health, Time.deltaTime * smoothSpeed);
        playerHealth = player.GetComponent<PlayerHealthController>();
        slider.maxValue = playerHealth.maxHealth;
        slider.value = playerHealth.health;
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
        // slider.value = health; 
       // playerHealth.setHealthBarText(health, playerHealth.maxHealth);
    }

}
