using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollision : MonoBehaviour
{
    [SerializeField]
    private ParticleSystemCaller splashPrefab;
    [Space]
    [SerializeField]
    private Vector2 minMaxSize = new Vector2(0.1f, 2f);
    [SerializeField]
    [Range(0.1f,100f)]
    private float speedModifier = 1f;

    private BoxCollider _triggerZone;
    private Queue<ParticleSystemCaller> _splashPool = new Queue<ParticleSystemCaller>();

    private void Awake()
    {
        _triggerZone = this.GetComponent<BoxCollider>();
    }

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            _splashPool.Enqueue(CreateSplash());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 point = _triggerZone.ClosestPointOnBounds(other.transform.position);
        float sizeFactor = 0f;
        Rigidbody body = other.attachedRigidbody;
        if(body != null)
        {
            sizeFactor = Mathf.Clamp01(body.velocity.magnitude * body.mass / speedModifier);
        }
        SplashAt(point, sizeFactor);

        if(body != null && body.TryGetComponent(out BodyReset resetter))
        {
            resetter.ResetPosition();
        }
    }

    private void SplashAt(Vector3 position, float sizeFactor)
    {
        ParticleSystemCaller splash = GetSplash();
        splash.transform.position = position;
        splash.transform.localScale = Mathf.Lerp(minMaxSize.x, minMaxSize.y, sizeFactor) * Vector3.one;
        splash.Play(()=>_splashPool.Enqueue(splash));
    }

    private ParticleSystemCaller GetSplash()
    {
        if (_splashPool.Count > 0)
        {
            return _splashPool.Dequeue();
        }
        return CreateSplash();
    }

    private ParticleSystemCaller CreateSplash()
    {
        GameObject go = GameObject.Instantiate(splashPrefab.gameObject, this.transform);
        return go.GetComponent<ParticleSystemCaller>();
    }
}
