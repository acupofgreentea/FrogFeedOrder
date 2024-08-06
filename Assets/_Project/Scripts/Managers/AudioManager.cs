using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [SerializeField] private AudioMixer audioMixer;

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

    private void OnSoundChanged()
    {
        audioMixer.SetFloat("Volume", DataManager.Sound ? 0f : -80f);
    }
}