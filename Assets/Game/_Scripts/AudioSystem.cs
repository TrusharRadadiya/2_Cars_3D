using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    [SerializeField] private AudioSource _soundFxSource;

    public static AudioSystem Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void PlaySound(AudioClip clip)
    {
        _soundFxSource.PlayOneShot(clip);
    }
}
