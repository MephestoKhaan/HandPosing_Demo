using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomSoundPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioClipRandomizer clipRandomizer;

    private AudioSource _audioPlayer;

    private void Awake()
    {
        _audioPlayer = this.GetComponent<AudioSource>();
    }

    public void PlayRandom()
    {
        clipRandomizer.PlayClip(_audioPlayer, out float lenght);
    }
}
