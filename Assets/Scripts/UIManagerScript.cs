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
    public int selectedNumber = 0;

    void Start()
    {
        dropdown = dropdownMenu.GetComponent<TMP_Dropdown>();
    }


    /// Reference to other scripts


    // TEST STUFF   
    public void DropdownChanged()
    {
        Debug.Log($"Dropdown Changed to {dropdown.value}!");
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
            pauseMenu.SetActive(true);

            Time.timeScale = 0;
            soundMngr.changeVolume(.5f);     //// if we add a volume setting we will need to store volume and minus it 
           
        }
        else
        {
            bool pause = false;
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
            soundMngr.changeVolume(1f);
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
