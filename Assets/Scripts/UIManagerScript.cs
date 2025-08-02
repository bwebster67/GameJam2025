using UnityEngine;
using Unity.UI;
using UnityEngine.Android;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class UIManagerScript : MonoBehaviour
{
    /// Our Different Menus
    public GameObject respawnMenu;
    public GameObject pauseMenu;
    public GameObject dropdownMenu;
    public TMP_Dropdown dropdown;


    [SerializeField] PlayerHealthController playerHP;
    [SerializeField] GameObject Player;
    [SerializeField] private SoundFXManager soundMngr;
    [SerializeField] private PlayerMusicController playerMusic;
  

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
    
    
    /// enables pause menu and sets time scale to 0
    public void PauseMenu()
    {
   

        if (pauseMenu.activeSelf != true)
        {
            bool pause = true;
            soundMngr.changeVolume(.5f); //// if we add a volume setting we will need to store volume and minus it 
            playerMusic.changeMusicVolume(.5f);
            pauseMenu.SetActive(true);

            Time.timeScale = 0;
            soundMngr.changeVolume(.5f); //// if we add a volume setting we will need to store volume and minus it 
            playerMusic.changeMusicVolume(.5f);
           
        }
        else
        {
            bool pause = false;
            soundMngr.changeVolume(1f);
            playerMusic.changeMusicVolume(1f);
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
           
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
