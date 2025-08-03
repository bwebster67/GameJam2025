using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    public GameObject StartMenuCanvas;
    public GameObject startButton;
    public AudioClip beep;

    public SceneTransitionManager transitionManager;
    public void StartGame()
    {
        SoundManagerScriptStart.Instance.PlaySoundClip(beep, transform, 1f, 1f);
        StartMenuCanvas.SetActive(false);
        transitionManager.LoadSceneWithFade("GameScene");
    }


}
