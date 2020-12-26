using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SlideSoundPlayer : MonoBehaviour
{
    [SerializeField]
    private Vector2 minMaxPitch;
    [SerializeField]
    private float maxVolume;
    [SerializeField]
    private float velocityThresold = 0.5f;
    [SerializeField]
    private float velocityFactor = 0.1f;
    [SerializeField]
    private float dampness = 10f;

    private AudioSource _audioSource;

    private bool _isPlaying = false;

    private Vector3 _lastPosition;

    private void Awake()
    {
        _audioSource = this.GetComponent<AudioSource>();
        _lastPosition = this.transform.position;
    }

    private void Start()
    {
        _audioSource.loop = true;
        _audioSource.playOnAwake = false;
        _audioSource.Stop();
        _isPlaying = false;
    }

    private void Update()
    {
        float velocity = (this.transform.position - _lastPosition).sqrMagnitude;
        _lastPosition = this.transform.position;

        if (velocity > velocityThresold 
            && !_isPlaying)
        {
            _audioSource.Play();
            _isPlaying = true;
        }

        if (_isPlaying)
        {
            velocity *= velocityFactor;
            float desiredVolume = Mathf.Lerp(0f, maxVolume, velocity);
            _audioSource.volume = Mathf.Lerp(_audioSource.volume, desiredVolume, dampness * Time.deltaTime);

            float desiredPitch = Mathf.Lerp(minMaxPitch.x, minMaxPitch.y, velocity);
            _audioSource.pitch = Mathf.Lerp(_audioSource.pitch, desiredPitch, dampness * Time.deltaTime);
        }

        if(_isPlaying 
            && _audioSource.volume == 0f)
        {
            _audioSource.Pause();
            _isPlaying = false;
        }
    }
}
