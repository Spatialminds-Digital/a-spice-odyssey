
using UnityEngine;

public class RandomAudioPicker : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private bool playOnStart = false;
    [SerializeField] private bool loop = false;

    void Start()
    {
        if(playOnStart) PlayRandomClip();
    }

    public void PlayRandomClip()
    {
        if(audioClips.Length <=0 || audioSource == null) return;

        audioSource.loop = loop;
        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.Play();
    }

    public void PlayRandomClipOneShot()
    {
        if(audioClips.Length <=0 || audioSource == null) return;

        audioSource.loop = false;
        audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)]);
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }
}
