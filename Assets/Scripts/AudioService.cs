using UnityEngine;

public class AudioService : MonoBehaviour
{
    [SerializeField] private RandomAudioPicker miniExplosion;
    [SerializeField] private RandomAudioPicker bigExplosion;
    [SerializeField]private RandomAudioPicker bulletExplosion;
    [SerializeField]private RandomAudioPicker cannonSound;

    [SerializeField] private AudioSource audioEffectsSource;

    [SerializeField] private AudioClip errorClip;
    [SerializeField] private AudioClip countdown;

    public static AudioService Instance;


    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }

        Instance = this;
    }
    
    public void PlayMiniExplosion()
    {
        miniExplosion?.PlayRandomClipOneShot();
    }

    public void PlayBigExplosion()
    {
        bigExplosion?.PlayRandomClipOneShot();
        
    }

    
    public void PlayBulletSound()
    {
        bulletExplosion?.PlayRandomClipOneShot();
        
    }
    public void PlayCannonSound()
    {
        cannonSound?.PlayRandomClipOneShot();
        
    }

    public void PlayError()
    {
        audioEffectsSource?.PlayOneShot(errorClip);
    }

    public void PlayCountdown()
    {
        audioEffectsSource?.PlayOneShot(countdown);
    }
}
