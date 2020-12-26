using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemCaller: MonoBehaviour
{
    private ParticleSystem particles;

    private System.Action stoppedCallback;

    private void Awake()
    {
        particles = this.GetComponent<ParticleSystem>();
    }

    public void Play(System.Action onStopped)
    {
        stoppedCallback = onStopped;
        particles.Play(true);
    }

    private void OnParticleSystemStopped()
    {
        stoppedCallback?.Invoke();
    }
}
