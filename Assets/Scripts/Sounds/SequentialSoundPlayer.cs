using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SequentialSoundPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioClipRandomizer clipRandomizer;
    [SerializeField]
    private Vector2 minMaxWaitTime;

    private AudioSource _audioPlayer;

    private void Awake()
    {
        _audioPlayer = this.GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        PlaySound();    
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void PlaySound()
    {
        if(this.enabled)
        {
            clipRandomizer.PlayClip(_audioPlayer, out float lenght);
            float waitTime = lenght + Random.Range(minMaxWaitTime.x, minMaxWaitTime.y);
            Invoke("PlaySound", waitTime);
        }
    }


}