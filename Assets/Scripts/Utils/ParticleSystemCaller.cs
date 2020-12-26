using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemCaller: MonoBehaviour
{
    private ParticleSystem particles;

    private System.Action stoppedCallback;

    public UnityEngine.Events.UnityEvent OnPlayed;

    private void Awake()
    {
        particles = this.GetComponent<ParticleSystem>();
    }

    public void Play(System.Action onStopped)
    {
        stoppedCallback = onStopped;
        particles.Play(true);
        OnPlayed?.Invoke();
    }

    private void OnParticleSystemStopped()
    {
        stoppedCallback?.Invoke();
    }
}
