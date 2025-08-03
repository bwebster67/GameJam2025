using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{

    public Slider slider;
    [SerializeField] float smoothSpeed = 1.5f;
    private PlayerMusicController playerMusicController;

    private void Start()
    {
        // find player tag
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
        {
            playerMusicController = playerGO.GetComponent<PlayerMusicController>();
        }
    }
    private void Update()
    {
        // slider.value = Mathf.Lerp(slider.value, playerMusicController.playerExperience, Time.deltaTime * smoothSpeed);
        slider.value = playerMusicController.playerExperience;
        slider.maxValue = playerMusicController.levelUpThreshold;
    }
}
