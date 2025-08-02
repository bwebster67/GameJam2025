using UnityEngine;
using Unity.UI;
using UnityEngine.Android;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using System;

public class UIManagerScript : MonoBehaviour
{
    /// Our Different Menus
    public GameObject respawnMenu;
    public GameObject pauseMenu;
    public GameObject dropdownMenu;
    public GameObject levelUpMenu;
    public GameObject canvas;
    public TMP_Dropdown dropdown;

    public static event Action OnPauseEvent;
    public static event Action OnUnPauseEvent;

    [SerializeField] PlayerHealthController playerHP;
    [SerializeField] GameObject Player;
    [SerializeField] private SoundFXManager soundMngr;
    [SerializeField] private PlayerMusicController playerMusic;

    // Level Up Listener
    private void OnEnable()
    {
        PlayerMusicController.LevelUpEvent += HandleLevelUp;
        LevelUpUI.OnFinishLevelUpSequence += InvokeUnpause;
    }

    private void OnDisable()
    {
        PlayerMusicController.LevelUpEvent += HandleLevelUp;
        LevelUpUI.OnFinishLevelUpSequence -= InvokeUnpause;
    }

    public void HandleLevelUp()
    {
        GameObject levelUpMenuGO = Instantiate(levelUpMenu, canvas.transform);
        RectTransform rect = levelUpMenuGO.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.anchoredPosition = Vector2.zero; // Center in parent canvas
        }
        LevelUpUI levelUpUI = levelUpMenu.GetComponent<LevelUpUI>();
        levelUpUI.canvas = canvas;
        Time.timeScale = 0;
        OnPauseEvent.Invoke();
    }

    public int selectedNumber = 0;

    void Start()
    {
        dropdown = dropdownMenu.GetComponent<TMP_Dropdown>();
    }


    /// Reference to other scripts


    // TEST STUFF   
    public void OnButtonClicked()
    {
        Debug.Log($"Updating note slot at index {dropdown.value}!");
        PlayerMusicController playerMusicController = Player.GetComponent<PlayerMusicController>();
        playerMusicController.ChangeToCircleAttack(dropdown.value);
    }
  

    //////////////////////    FUNCTIONS ////////////////////////////
 
    public void enableMenu(GameObject menu) 
    {
        menu.SetActive(true);
    }
    public void disableMenu(GameObject menu)
    {
        menu.SetActive(false);
    }

    public void InvokeUnpause()
    {
        Time.timeScale = 1;
        OnUnPauseEvent.Invoke();
    }
    
    /// enables pause menu and sets time scale to 0
    public void PauseMenu()
    {


        if (pauseMenu.activeSelf != true)
        {
            bool pause = true;
            soundMngr.changeVolume(.5f); //// if we add a volume setting we will need to store volume and minus it 
            // playerMusic.changeMusicVolume(.5f);
            pauseMenu.SetActive(true);

            Time.timeScale = 0;
            soundMngr.changeVolume(.5f); //// if we add a volume setting we will need to store volume and minus it 
            // playerMusic.changeMusicVolume(.5f);
            OnPauseEvent.Invoke();

        }
        else
        {
            bool pause = false;
            soundMngr.changeVolume(1f);
            // playerMusic.changeMusicVolume(1f);
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
            OnUnPauseEvent.Invoke();

        }
    }


    ///////////////-------- FUNCTIONS ON BUTTON PRESS --------///////////////

    public void onRespawnClick()
    {
        playerHP.Respawn();
    }
    //////////////////////////////////////////////////

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("paused");
            PauseMenu();
        }

    }
}
