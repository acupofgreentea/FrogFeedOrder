using Lean.Pool;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    public void Play(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    private void Update()
    {
        if(audioSource.isPlaying) 
            return;
        
        LeanPool.Despawn(this);
    }
}