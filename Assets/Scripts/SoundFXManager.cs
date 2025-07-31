using UnityEngine;

public class SoundFXManager : MonoBehaviour
{

    public static SoundFXManager Instance;
    [SerializeField] private AudioSource soundFXObject;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void PlaySoundClip(AudioClip audioClip, Transform spawnTransform, float volume, float pitch)
    {

        //spawn the game obj
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        //assign the audio clip
        audioSource.clip = audioClip;
        //assign volume
        audioSource.volume = volume;
        //play the sound
        audioSource.Play();
        //set pitch 
        audioSource.pitch = pitch;
        //get length of sound clip
        float clipLength = audioSource.clip.length;
        //destroy the clip
        Destroy(audioSource.gameObject, clipLength);
    }
    public void changeVolume(float volume) /// 0-1
    {
        soundFXObject.volume = volume;
    }
    
}
