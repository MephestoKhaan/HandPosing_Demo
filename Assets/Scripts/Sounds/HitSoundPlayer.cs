using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class HitSoundPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioClipRandomizer clip;

    [SerializeField]
    private float velocityFactor = 0.1f;
    [SerializeField]
    private float velocityThresold = 0.5f;

    private AudioSource _audioSource;
    private float _lastPlayed;

    private const float WAIT_TIME = 0.1f;


    private void Awake()
    {
        _audioSource = this.GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(CanPlay())
        {
            float vel = collision.relativeVelocity.sqrMagnitude;
            if (vel > velocityThresold)
            {
                clip.PlayClip(_audioSource, out float lenght, vel * velocityFactor);
                _lastPlayed = Time.timeSinceLevelLoad;
            }
        }
    }

    private bool CanPlay()
    {
        return Time.timeSinceLevelLoad - _lastPlayed > WAIT_TIME;
    }
}
