using UnityEngine;

public class Flare : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem flaresParticles;
    [SerializeField]
    private ParticleSystem smokeParticles;
    [SerializeField]
    private AudioSource flareSound;

    [SerializeField]
    private Collider fireCollider;

    public bool IsPlaying { get; private set; }

    private void Awake()
    {
        fireCollider.enabled = IsPlaying;
    }

    public void Play(bool play)
    {
        if (play != IsPlaying)
        {
            IsPlaying = play;
            if (play)
            {
                flaresParticles.Play(true);
                flareSound.Play();
            }
            else
            {
                flaresParticles.Stop(true);
                flareSound.Pause();
                smokeParticles.Play();
            }
            fireCollider.enabled = play;
        }
    }
}
