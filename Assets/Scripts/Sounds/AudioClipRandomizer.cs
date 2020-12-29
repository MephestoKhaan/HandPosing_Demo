using UnityEngine;

[CreateAssetMenu(menuName = "HandPose Demo/Audio Clip Randomizer")]
public class AudioClipRandomizer : ScriptableObject
{
    [SerializeField]
    private AudioClip[] clips;

    [SerializeField]
    [Range(0f, 1f)]
    private float noise = 0f;

    [SerializeField]
    private Vector2 minMaxPitch = new Vector2(0.8f, 1.2f);

    [SerializeField]
    private Vector2 minMaxVolume = new Vector2(0.5f, 1f);

    public void PlayClip(AudioSource source, out float clipLenght, float range = -1f)
    {
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        float pitch;
        float volume;

        if (range >= 0f)
        {
            float clampedRange = Mathf.Clamp01(range);
            float min = Mathf.Max(-noise, -clampedRange);
            float max = Mathf.Min(noise, 1f - clampedRange);
            pitch = Mathf.Lerp(minMaxPitch.x, minMaxPitch.y, clampedRange + Random.Range(min, max));
            volume = Mathf.Lerp(minMaxVolume.x, minMaxVolume.y, clampedRange + Random.Range(min, max));
        }
        else
        {
            pitch = Random.Range(minMaxPitch.x, minMaxPitch.y);
            volume = Random.Range(minMaxVolume.x, minMaxVolume.y);
        }

        source.pitch = pitch;
        source.PlayOneShot(clip, volume);

        clipLenght = ClipLenght(clip, pitch);
    }


    private float ClipLenght(AudioClip clip, float pitch)
    {
        return clip.length * (2f - pitch) + 1f;
    }
}
