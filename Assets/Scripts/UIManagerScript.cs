using UnityEngine;
using Unity.UI;
using UnityEngine.Android;

public class UIManagerScript : MonoBehaviour
{
    /// Our Different Menus
    public GameObject respawnMenu;
    public GameObject pauseMenu;


    /// Reference to other scripts
    [SerializeField] PlayerHealthController playerHP;
    [SerializeField] private SoundFXManager soundMngr;
    
    
   
  

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
