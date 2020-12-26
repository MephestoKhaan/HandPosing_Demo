using UnityEngine;
using Unity.Burst;
using UnityEngine.ParticleSystemJobs;

[RequireComponent(typeof(ParticleSystem))]
public class DisableParticleRoll : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private Job _job = new Job();

    private float _lastRotation;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        Camera camera = Camera.main;
        if (camera == null)
        {
            return;
        }

        float currentRotation = camera.transform.rotation.eulerAngles.z;
        _job.rotationDifference = (currentRotation - _lastRotation) * Mathf.Deg2Rad;
        _lastRotation = currentRotation;
    }

    void OnParticleUpdateJobScheduled()
    {
        if (_job.rotationDifference != 0.0f)
        {
            _job.ScheduleBatch(_particleSystem, 4096);
        }
    }

    [BurstCompile]
    struct Job : IJobParticleSystemParallelForBatch
    {
        public float rotationDifference;

        public void Execute(ParticleSystemJobData particles, int startIndex, int count)
        {
            var rotations = particles.rotations.z;
            for (int i = startIndex; i < startIndex + count; i++)
            {
                rotations[i] += rotationDifference;
            }
        }
    }
}
