using UnityEngine;
using sm = Managers.SoundManager;

public class VolumeControl : MonoBehaviour
{
    AudioSource audioSource;

    float volume = 1f;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = sm._instance.audioController;    
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = volume;
    }

    public void SetVolume(float vol)
    {
        volume = vol;
    }
}
