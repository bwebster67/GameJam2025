using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class UIManagerScript : MonoBehaviour
{
    /// Our Different Menus
    public GameObject respawnMenu;
    public GameObject pauseMenu;
    public GameObject dropdownMenu;
    public GameObject levelUpMenu;
    public GameObject canvas;
    public TMP_Dropdown dropdown;
    public GameObject StartMenuCanvas;

    public static event Action OnPauseEvent;
    public static event Action OnUnPauseEvent;


    [SerializeField] PlayerHealthController playerHP;
    [SerializeField] GameObject Player;
    [SerializeField] private SoundFXManager soundMngr;
    [SerializeField] private PlayerMusicController playerMusic;
    [SerializeField] private PlayerHealthController PlayerHealthController;

    public static event Action StartClickEvent;
    // Level Up Listener
    private void OnEnable()
    {
        ////Start Menu stuff
        if (canvas != null)
            canvas.SetActive(true);
        else
            Debug.LogWarning("UIManagerScript: Canvas not assigned!");


        PlayerMusicController.LevelUpEvent += HandleLevelUp;
        LevelUpUI.OnFinishLevelUpSequence += InvokeUnpause;
    }

    public void clickPlay()
    {
        //Time.timeScale = 1;

        //canvas.SetActive(true);
        //StartMenuCanvas.SetActive(false);
        //StartClickEvent.Invoke();
    }
    public void restart()
    {

        ReloadCurrentScene();
    }
    public void ReloadCurrentScene()
    {
        //string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("GameScene");
        //StartClickEvent.Invoke();/// starts beat
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
        LevelUpUI levelUpUI = levelUpMenuGO.GetComponent<LevelUpUI>();
        levelUpUI.canvas = canvas.GetComponent<Canvas>();
        Time.timeScale = 0;
        OnPauseEvent.Invoke();
    }
    public int selectedNumber = 0;

    void Start()
    {
        //PlayerHealthController = GetComponent<PlayerHealthController>();

       // StartClickEvent.Invoke();/// starts beat
        if (canvas == null)
        {
            GameObject found = GameObject.FindWithTag("Canvas");
            if (found != null)
                canvas = found;
            else
                Debug.LogError("UIManagerScript: Could not find Canvas in scene.");
        }

        if (canvas != null)
            canvas.SetActive(true);

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
        respawnMenu.gameObject.SetActive(false);
        PlayerHealthController.isDead = false;
        SceneManager.LoadScene("StartMenuScene");
        Time.timeScale = 1;

    }
    //////////////////////////////////////////////////

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("paused");
            PauseMenu();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("restart");
            restart();
        }

    }

    public void openRespawnMenu()
    {
        respawnMenu.gameObject.SetActive(true);
    }

    
}
