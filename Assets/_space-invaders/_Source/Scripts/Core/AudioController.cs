using UnityEngine;

public class AudioController : MonoBehaviour
{
    private readonly float defaultTempo = 1f;
    public static AudioController Instance;
    public int PitchChangeStep => _pitchChangeSteps;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _soundSource;
    [SerializeField] private int _pitchChangeSteps = 5;
    [SerializeField] private float _maxPitch = 5.5f;

    private float pitchChange;

    internal float Tempo { get; private set; }

    internal void PlayMusic() => _musicSource.Play();
    internal void StopPlaying() => _musicSource.Stop();

    internal void IncreasePitch()
    {
        if (_musicSource.pitch >= _maxPitch)
        {
            return;
        }

        _musicSource.pitch = Mathf.Clamp(_musicSource.pitch + pitchChange, 1, _maxPitch);
        Tempo = Mathf.Pow(2, pitchChange) * Tempo;
    }

    internal void PlaySound(AudioClip audioClip) => _soundSource.PlayOneShot(audioClip);

    private void OnEnable()
    {
        _musicSource.pitch = 1f;
        Tempo = defaultTempo;
        pitchChange = _maxPitch / _pitchChangeSteps;
    }

    private void OnDisable()
    {
        StopPlaying();
    }
}
