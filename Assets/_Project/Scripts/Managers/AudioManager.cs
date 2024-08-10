using System;
using Lean.Pool;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private AudioPlayer audioPlayer;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        DataManager.OnSoundChanged += OnSoundChanged;
        
        OnSoundChanged();
    }

    private void OnDisable()
    {
        DataManager.OnSoundChanged -= OnSoundChanged;
    }

    public void PlaySound(AudioClip audioClip)
    {
        if(!DataManager.Sound)
            return;
        
        AudioPlayer player = LeanPool.Spawn(audioPlayer);
        player.Play(audioClip);
    }

    private void OnSoundChanged()
    {
        audioMixer.SetFloat(Constants.VOLUME_KEY, DataManager.Sound ? 0f : -80f);
    }
}